using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Dada.Commander.Ui
{
    public static class ConsoleController
    {
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