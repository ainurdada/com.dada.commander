using Dada.Commander;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Dada.Commander.UI
{
    [ExecuteAlways]
    internal class ConsoleUI : MonoBehaviour
    {
        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] TMP_InputField inputField;
        [SerializeField] RectTransform logContent;
        [SerializeField] TMP_Text logHelper;
        [Space]
        public Image logBackground;
        public Image inputBackGround;
        [Space]
        public Dada.Commander.UI.Preview preview;

        #region Instance
        static ConsoleUI instance;
        static ConsoleUI editorInstance;
        public static ConsoleUI Instance
        {
            get
            {
                if (Application.isPlaying)
                    return instance;

                return editorInstance;
            }
        }
        #endregion

        TextMeshProUGUI logText;

        [HideInInspector] public bool hideAtTheBeginning = true;

        [HideInInspector] public string cmdColor = "#fff";
        [HideInInspector] public string commandPrefix = ">";
        [HideInInspector] public string prefixColor = "fff";

        [HideInInspector] public event Action OnShowed;
        [HideInInspector] public event Action OnHided;

        bool isShowed;

        private void Start()
        {
            if (Application.IsPlaying(gameObject))
            {
                instance = this;

                inputField.onSubmit.AddListener(EnterCommand);
                inputField.onValueChanged.AddListener(WriteHelper);
                logText = logContent.GetComponent<TextMeshProUGUI>();
                logText.text = "";
                logContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);
                StartCoroutine(FirstFrame());
                Destroy(preview.gameObject);
            }
            else
            {
                SerializedObject serialized = new SerializedObject(this);
                if (editorInstance != null)
                {
                    DestroyImmediate(editorInstance);
                }
                editorInstance = this;
            }
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
            inputField.ActivateInputField();
        }

        public void FillAuto()
        {
            if (isShowed)
            {
                string str = Commander.GetSimilarCommand(inputField.text);
                inputField.text = str != null ? str : inputField.text;
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
            inputField.ActivateInputField();
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
            isShowed = true;
            OnShowed?.Invoke();
        }

        public void Hide()
        {
            inputField.DeactivateInputField();
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
            isShowed = false;
            OnHided?.Invoke();
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