using System;
using UnityEngine;


namespace Dada.Commander.UI
{
    public static class CommanderUI
    {
        /// <summary>
        /// Called when the console is showed
        /// </summary>
        public static event Action OnShowed
        {
            add
            {
                if (CommanderUIWindow.Instance == null)
                {
                    Debug.LogError("ConsoleUI is not exist");
                    return;
                }
                CommanderUIWindow.Instance.OnShowed += value;
            }
            remove
            {
                if (CommanderUIWindow.Instance == null)
                {
                    Debug.LogError("ConsoleUI is not exist");
                    return;
                }
                CommanderUIWindow.Instance.OnShowed -= value;
            }
        }

        /// <summary>
        /// Called when the console is hided
        /// </summary>
        public static event Action OnHided
        {
            add
            {
                if (CommanderUIWindow.Instance == null)
                {
                    Debug.LogError("ConsoleUI is not exist");
                    return;
                }
                CommanderUIWindow.Instance.OnHided += value;
            }
            remove
            {
                if (CommanderUIWindow.Instance == null)
                {
                    Debug.LogError("ConsoleUI is not exist");
                    return;
                }
                CommanderUIWindow.Instance.OnHided -= value;
            }
        }

        /// <summary>
        /// True if the console is shown and false if not
        /// </summary>
        public static bool WindowIsShowed => CommanderUIWindow.Instance.isShowed;

        /// <summary>
        /// True if the PopUp command window is shown and false if not
        /// </summary>
        public static bool PopUpHelperIsShowed => CommanderUIWindow.Instance.popUpHelper.IsShowed;

        /// <summary>
        /// Open or close console window
        /// </summary>
        public static void ChangeOpenCloseState()
        {
            CommanderUIWindow.Instance.ChangeOpenCloseState();
        }

        /// <summary>
        /// Autofill inputfield by similar command
        /// </summary>
        public static void FillAuto()
        {
            CommanderUIWindow.Instance.FillAuto();
        }

        /// <summary>
        /// Select next command in PopUp command window
        /// </summary>
        public static void SelectNextHelperItem()
        {
            CommanderUIWindow.Instance.SelectNextHelperItem();
        }

        /// <summary>
        /// Select previous command in PopUp command window
        /// </summary>
        public static void SelectPreviousHelperItem()
        {
            CommanderUIWindow.Instance.SelectPreviousHelperItem();
        }

        /// <summary>
        /// Select an older command
        /// </summary>
        public static void SelectNextLastCommand()
        {
            if (!PopUpHelperIsShowed)
                CommanderUIWindow.Instance.SelectNextLastCommand();
        }

        /// <summary>
        /// Select a newer command
        /// </summary>
        public static void SelectPreviousLastCommand()
        {
            if (!PopUpHelperIsShowed)
                CommanderUIWindow.Instance.SelectPreviousLastCommand();
        }
    }
}