using Dada.Commander;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dada.Commander.Ui
{
    internal class ConsoleUI : MonoBehaviour
    {
        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] TMP_InputField inputField;
        [SerializeField] RectTransform logContent;
        [SerializeField] TMP_Text logHelper;
        [Space]
        public Image logBackground;
        public Image inputBackGround;

        #region Instance
        static ConsoleUI instance;
        public static ConsoleUI Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion

        TextMeshProUGUI logText;

        [HideInInspector] public bool hideAtTheBeginning= true;

        [HideInInspector] public string cmdColor = "#fff";
        [HideInInspector] public string commandPrefix = ">";
        [HideInInspector] public string prefixColor = "fff";

        bool isShowed;

        private void Awake()
        {
            instance = this;

            inputField.onSubmit.AddListener(EnterCommand);
            inputField.onValueChanged.AddListener(WriteHelper);
            logText = logContent.GetComponent<TextMeshProUGUI>();
            logText.text = "";
            logContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);
            StartCoroutine(FirstFrame());
        }

        private IEnumerator FirstFrame()
        {
            yield return new WaitForEndOfFrame();
            ClearConsole();
            if (hideAtTheBeginning) Hide();
        }

        public void EnterCommand(string command)
        {
            if (command.Length == 0) return;
            inputField.text = "";
            logHelper.text = "";
            AddLog(new[] { $"<color={prefixColor}>{commandPrefix}</color> <color={cmdColor}>{command}</color>" });
            Commander.ApplyCommand(command, out List<string> result);

            AddLog(result.ToArray());
        }

        public void FillAuto()
        {
            if (isShowed)
            {
                inputField.text = Commander.GetSimilarCommand(inputField.text);
                inputField.MoveToEndOfLine(false, false);
            }
        }
        public void WriteHelper(string message)
        {
            if (message.Length > 0) logHelper.text = Commander.GetSimilarCommand(message);
            else logHelper.text = "";
        }

        public void AddLog(string[] text)
        {
            foreach (string str in text)
            {
                logText.text += str + '\n';
            }
            logContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, logText.preferredHeight);
        }

        public void ChangeOpenCloseState()
        {
            if (!isShowed)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        public void Show()
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
            isShowed = true;
        }

        public void Hide()
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
            isShowed = false;
        }

        [ConsoleCommand(commandName = "clear",
            description = "clear console logs")]
        public void ClearConsole()
        {
            logText.text = "";
            logContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);
        }
    }
}