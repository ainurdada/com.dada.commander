using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Dada.Commander.UI
{
    public static class ConsoleController
    {
        #region ConsoleUI Events
        public static event Action OnShowed
        {
            add
            {
                if (ConsoleUI.Instance == null)
                {
                    Debug.LogError("ConsoleUI is not exist");
                    return;
                }
                ConsoleUI.Instance.OnShowed += value;
            }
            remove
            {
                if (ConsoleUI.Instance == null)
                {
                    Debug.LogError("ConsoleUI is not exist");
                    return;
                }
                ConsoleUI.Instance.OnShowed -= value;
            }
        }
        public static event Action OnHided
        {
            add
            {
                if (ConsoleUI.Instance == null)
                {
                    Debug.LogError("ConsoleUI is not exist");
                    return;
                }
                ConsoleUI.Instance.OnHided += value;
            }
            remove
            {
                if (ConsoleUI.Instance == null)
                {
                    Debug.LogError("ConsoleUI is not exist");
                    return;
                }
                ConsoleUI.Instance.OnHided -= value;
            }
        }
        #endregion

        public static void ChangeOpenCloseState()
        {
            ConsoleUI.Instance.ChangeOpenCloseState();
        }

        public static void FillAuto()
        {
            ConsoleUI.Instance.FillAuto();
        }
    }
}