using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace shadowBasic.Components.Text
{
    public class TextComponent : Component
    {
        private readonly List<ITextCollection> _collections;
        private readonly List<Tuple<ITextCollection, MethodInfo>> _textBindMethods;

        public TextComponent(KeybinderCore core, params ITextCollection[] collections) 
            : base(core)
        {
            _collections = new List<ITextCollection>(collections);
            _textBindMethods = new List<Tuple<ITextCollection, MethodInfo>>();
        }

        public override void Start()
        {
            foreach (var collection in _collections)
            {
                _textBindMethods.AddRange(new List<MethodInfo>(collection
                    .GetType()
                    .GetMethods()
                    .Where(method => !method.IsStatic && method.CustomAttributes
                    .Where(attribute => attribute.AttributeType == typeof(TextAttribute))
                    .Count() != 0))
                    .Select(method => new Tuple<ITextCollection, MethodInfo>(collection, method)));
            }
        }

        public override void Stop()
        {

        }

        protected override void InitializeAssembly(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                _textBindMethods.AddRange(new List<MethodInfo>(type
                    .GetMethods()
                    .Where(method => method.IsStatic && method.CustomAttributes
                    .Where(attribute => attribute.AttributeType == typeof(TextAttribute))
                    .Count() != 0))
                    .Select(method => new Tuple<ITextCollection, MethodInfo>(null, method)));
            }
        }

        public bool CheckCall(string command)
        {
            var args = command.Split(' ');

            if (args.Length == 0)
                return false;

            foreach (var tuple in _textBindMethods)
            {
                var collection = tuple.Item1;
                var method = tuple.Item2;
                var textAttributes = method.GetCustomAttributes<TextAttribute>();
                var conditionalAttributes = method.GetCustomAttributes<ConditionalAttribute>();

                foreach (var textAttribute in textAttributes)
                {
                    if (String.Compare(textAttribute.Command, args[0], true) == 0)

                    {
                        if (ConditionalAttribute.CanExecute(conditionalAttributes))

                        {
                            var argsList = new List<string>(args.Skip(1));
                            if (argsList.Count >= textAttribute.ArgumentCount)
                            {
                                if (method.ReturnType == typeof(bool) || method.ReturnType == typeof(Task<bool>))
                                {
                                    if(textAttribute.IsAsync(method))
                                    {
                                        Task.Run(async () =>
                                        {
                                            var returnValue = await method.InvokeAsync<bool>(collection, new object[] { argsList.ToArray() });

                                            if (!returnValue)
                                                ChatUtil.ShowUsage($"Benutzung: {args[0]} {textAttribute.Arguments}");
                                        });
                                    }
                                    else
                                    {
                                        Task.Run(() =>
                                        {
                                            var returnValue = (bool)method.Invoke(collection, new object[] { argsList.ToArray() });

                                            if (!returnValue)
                                                ChatUtil.ShowUsage($"Benutzung: {args[0]} {textAttribute.Arguments}");
                                        });
                                    }
                                }
                                else
                                {
                                    if (textAttribute.IsAsync(method))
                                        Task.Run(async () => await method.InvokeAsync(collection, new object[] { argsList.ToArray() }));
                                    else
                                        Task.Run(() => method.Invoke(collection, new object[] { argsList.ToArray() }));
                                }
                            }
                            else
                                ChatUtil.ShowUsage($"Benutzung: {args[0]} {textAttribute.Arguments}");


                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}
