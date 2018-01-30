using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace shadowBasic.Components.Chat
{
    public class ChatComponent : Component, IDisposableEx
    {
        private bool _disposed;

        private readonly List<IChatCollection> _collections;
        private readonly List<Tuple<IChatCollection, MethodInfo>> _chatBindMethods;
        private readonly string _chatLogPath;

        private FileInfo _fileInfo;
        private StreamReader _streamReader;
        private FileSystemWatcher _fileSystemWatcher;

        public ChatComponent(KeybinderCore core, params IChatCollection[] collections)
            : base(core)
        {
            _disposed = false;

            _collections = new List<IChatCollection>(collections);
            _chatBindMethods = new List<Tuple<IChatCollection, MethodInfo>>();
            _chatLogPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "GTA San Andreas User Files\\SAMP\\");
        }

        ~ChatComponent()
        {
            Dispose(false);
        }

        public void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            _disposed = true;

            _streamReader?.Dispose();
            _fileSystemWatcher?.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public override void Start()
        {
            foreach (var collection in _collections)
            {
                _chatBindMethods.AddRange(new List<MethodInfo>(collection
                    .GetType()
                    .GetMethods()
                    .Where(method => !method.IsStatic && method.CustomAttributes
                    .Where(attribute => attribute.AttributeType == typeof(ChatAttribute))
                    .Count() != 0))
                    .Select(method => new Tuple<IChatCollection, MethodInfo>(collection, method)));
            }

            _fileInfo = new FileInfo(Path.Combine(_chatLogPath, "chatlog.txt"));

            _streamReader = new StreamReader(new FileStream(Path.Combine(_chatLogPath, "chatlog.txt"), FileMode.Open, FileAccess.Read, FileShare.ReadWrite), Encoding.Default, true);
            _streamReader.ReadToEnd();

            _fileSystemWatcher = new FileSystemWatcher(_chatLogPath, "chatlog.txt");
            _fileSystemWatcher.Changed += ChatLogChanged;
            _fileSystemWatcher.EnableRaisingEvents = true;
        }

        public override void Stop()
        {
            _streamReader.Close();
            _fileSystemWatcher.EnableRaisingEvents = false;
        }

        protected override void InitializeAssembly(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                _chatBindMethods.AddRange(new List<MethodInfo>(type
                    .GetMethods()
                    .Where(method => method.IsStatic && method.CustomAttributes
                    .Where(attribute => attribute.AttributeType == typeof(ChatAttribute))
                    .Count() != 0))
                    .Select(method => new Tuple<IChatCollection, MethodInfo>(null, method)));
            }
        }

        public bool CheckCall(string message)
        {
            bool found = false;

            foreach (var tuple in _chatBindMethods)
            {
                var collection = tuple.Item1;
                var method = tuple.Item2;
                var chatAttributes = method.GetCustomAttributes<ChatAttribute>();
                var conditionalAttributes = method.GetCustomAttributes<ConditionalAttribute>();

                foreach (var chatAttribute in chatAttributes)
                {
                    if(chatAttribute.IsActive(Core) && ConditionalAttribute.CanExecute(conditionalAttributes))
                    {
                        var match = Regex.Match(message, $"^{chatAttribute.Regex}");
                        if (match.Success)
                        {
                            var groups = new List<string>();
                            for (int i = 0; i < match.Groups.Count; i++)
                                groups.Add(match.Groups[i].Value);

                            if (chatAttribute.IsAsync(method))
                                Task.Run(async () => await method.InvokeAsync(collection, new object[] { groups.ToArray() }));
                            else
                                method.Invoke(collection, new object[] { groups.ToArray() });

                            found = true;
                        }
                    }
                }
            }

            return found;
        }

        private void ChatLogChanged(object sender, FileSystemEventArgs e)
        {
            RefreshFile();

            while (!_streamReader.EndOfStream)
            {
                var line = _streamReader.ReadLine();
                if (line != String.Empty)
                {
                    var splitted = line.Split(' ').ToList();
                    var dateTime = DateTime.Parse(splitted[0].Remove(0, 1).Remove(splitted[0].Length - 2));
                    splitted.RemoveAt(0);

                    var joinedMessage = String.Join(" ", splitted);

                    if (joinedMessage != String.Empty)
                    {
                        if (CheckCall(joinedMessage))
                            break;
                    }
                }
            }
        }

        private void RefreshFile()
        {
            _fileInfo.Refresh();
            if (_fileInfo.Length < _streamReader.BaseStream.Position)
            {
                _streamReader.BaseStream.Position = 0;
                _streamReader.DiscardBufferedData();
            }
        }
    }
}
