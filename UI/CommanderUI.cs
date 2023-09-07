using System;
using UnityEngine;


namespace Dada.Commander.UI
{
    public static class CommanderUI
    {
        // ConsoleUI Events
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


        // ConsoleUI variables
        public static bool WindowIsShowed => CommanderUIWindow.Instance.isShowed;
        public static bool PopUpHelperIsShowed => CommanderUIWindow.Instance.popUpHelper.IsShowed;

        public static void ChangeOpenCloseState()
        {
            CommanderUIWindow.Instance.ChangeOpenCloseState();
        }

        public static void FillAuto()
        {
            CommanderUIWindow.Instance.FillAuto();
        }

        public static void SelectNextHelperItem()
        {
            CommanderUIWindow.Instance.SelectNextHelperItem();
        }
        public static void SelectPreviousHelperItem()
        {
            CommanderUIWindow.Instance.SelectPreviousHelperItem();
        }

        public static void SelectNextLastCommand()
        {
            if (!PopUpHelperIsShowed)
                CommanderUIWindow.Instance.SelectNextLastCommand();
        }
        public static void SelectPreviousLastCommand()
        {
            if (!PopUpHelperIsShowed)
                CommanderUIWindow.Instance.SelectPreviousLastCommand();
        }
    }
}