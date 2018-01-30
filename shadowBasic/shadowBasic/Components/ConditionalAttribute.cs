using shadowBasic.Components.Settings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace shadowBasic.Components
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    class ConditionalAttribute : Attribute
    {
        private readonly string _activationCondition;
        private readonly bool _activationConditionState;

        internal static SettingComponent Settings { get; set; }

        public string ActivationCondition
        {
            get { return _activationCondition; }
        }

        public bool ActivationConditionState
        {
            get { return _activationConditionState; }
        }

        public static bool CanExecute(IEnumerable<ConditionalAttribute> conditionalAttributes)
        {
            if (conditionalAttributes.Count() == 0)
                return true;

            foreach (var attribute in conditionalAttributes)
            {
                if (attribute.CanExecute())
                    return true;
            }

            return false;
        }

        public ConditionalAttribute(string activationCondition = "", bool activationConditionState = true)
        {
            _activationCondition = activationCondition;
            _activationConditionState = activationConditionState;
        }

        public bool CanExecute()
        {
            if (_activationCondition.Length != 0)
            {
                if (Settings.GetPermanentSetting<bool>(_activationCondition) == _activationConditionState)
                    return true;
                else
                    return false;
            }

            return true;
        }
    }
}
