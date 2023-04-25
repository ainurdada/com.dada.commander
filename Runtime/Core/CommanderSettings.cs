using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dada.Commander.Core
{
    public static class CommanderSettings
    {
        public static string LogErrorColor
        {
            get
            {
                return CommanderManager.Instance.errorColor;
            }
            set
            {
                CommanderManager.Instance.errorColor = value;
            }
        }
        public static string LogTextColor
        {
            get
            {
                return CommanderManager.Instance.textColor;
            }
            set
            {
                CommanderManager.Instance.textColor = value;
            }
        }
    }
}
