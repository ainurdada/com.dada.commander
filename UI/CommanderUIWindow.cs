using Codice.Client.BaseCommands;
using Dada.Commander.UI.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Dada.Commander.UI
{
    [ExecuteAlways]
    internal class CommanderUIWindow : MonoBehaviour
    {
        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] TMP_InputField inputField;
        [SerializeField] RectTransform logContent;
        [SerializeField] TMP_Text logHelper;
        public PopUpHelper popUpHelper;
        [Space]
        public Image logBackground;
        public Image inputBackGround;
        [Space]
        public Dada.Commander.UI.Preview preview;

        #region Instance
        static CommanderUIWindow instance;
        static CommanderUIWindow editorInstance;
        public static CommanderUIWindow Instance
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

        public bool isShowed { get; private set; }
        int _previousCommandsIndex = 0;
        bool _writeMode;
        bool _popupHelperMode;
        bool _selectedCommand;

        private void Awake()
        {
            Commander.LogEvent += AddLog;
            if (Application.IsPlaying(gameObject))
            {
                instance = this;
            }
            else
            {
                if (editorInstance != null)
                {
                    DestroyImmediate(editorInstance);
                }
                editorInstance = this;
            }
        }

        private void Start()
        {
            if (Application.IsPlaying(gameObject))
            {
                inputField.onSubmit.AddListener(EnterCommand);
                inputField.onValueChanged.AddListener(str => SelectWriteMode());
                inputField.onValueChanged.AddListener(WriteHelper);
                inputField.onValueChanged.AddListener(ShowPopUpHelper);
                inputField.onFocusSelectAll = false;
                logText = logContent.GetComponent<TextMeshProUGUI>();
                logText.text = "";
                logContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);
                Destroy(preview.gameObject);
                ClearConsole();
                if (hideAtTheBeginning) Hide();
            }
        }

        public void EnterCommand(string command)
        {
            if (popUpHelper == null || popUpHelper.selectedItemIndex == -1)
            {
                _previousCommandsIndex = -1;
                if (command.Length == 0) return;
                inputField.text = "";
                logHelper.text = "";
                AddLog(new[] { $"<color={prefixColor}>{commandPrefix}</color> <color={cmdColor}>{command}</color>" });
                Commander.ApplyCommand(command, out List<string> result);

                AddLog(result.ToArray());
                inputField.ActivateInputField();
            }
            else
            {
                inputField.text = popUpHelper.selectedCommand;
                inputField.MoveToEndOfLine(false, false);
                popUpHelper.CleanAndHide();
                popUpHelper.Show(Commander.GetSimilarCommands(inputField.text));
                EventSystem.current.SetSelectedGameObject(inputField.gameObject, null);
                inputField.OnPointerClick(new PointerEventData(EventSystem.current));
            }
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
        public void ShowPopUpHelper(string message)
        {
            if (message.Length > 0 && _writeMode)
            {
                popUpHelper.CleanAndHide();
                List<string> commands = Commander.GetSimilarCommands(message);
                if (commands != null)
                {
                    popUpHelper.Show(commands);
                }
            }
            else
            {
                popUpHelper.CleanAndHide();
            }
        }
        public void SelectNextHelperItem()
        {
            inputField.DeactivateInputField();
            popUpHelper.SelectNextItem();
            if(popUpHelper.IsSelected) _popupHelperMode = true;
            else _popupHelperMode = false; 
            inputField.caretPosition = inputField.text.Length;
            inputField.ActivateInputField();
        }
        public void SelectPreviousHelperItem()
        {
            inputField.DeactivateInputField();
            popUpHelper.SelectPreviousItem();
            if (popUpHelper.IsSelected) _popupHelperMode = true;
            else _popupHelperMode = false;
            inputField.caretPosition = inputField.text.Length;
            inputField.ActivateInputField();
        }
        public void SelectNextLastCommand()
        {
            if (_previousCommandsIndex == -1) return;
            if (++_previousCommandsIndex == Commander.PreviousCommands.Count) _previousCommandsIndex = -1;
            ShowLastCommand();
        }
        public void SelectPreviousLastCommand()
        {
            if (_previousCommandsIndex == 0) return;
            if (_previousCommandsIndex == -1) _previousCommandsIndex = Commander.PreviousCommands.Count - 1;
            else _previousCommandsIndex--;
            ShowLastCommand();
        }
        void ShowLastCommand()
        {
            inputField.DeactivateInputField();
            _selectedCommand = true;
            _writeMode = false;
            _previousCommandsIndex = Mathf.Clamp(_previousCommandsIndex, -1, Commander.PreviousCommands.Count - 1);
            if (_previousCommandsIndex == -1)
            {
                _selectedCommand = false;
                inputField.text = "";
            }
            else
            {
                inputField.text = Commander.PreviousCommands[_previousCommandsIndex];
            }
            inputField.MoveToEndOfLine(false, false);
            inputField.ActivateInputField();
        }
        void SelectWriteMode()
        {
            if (!_selectedCommand)
                _writeMode = true;
            _selectedCommand = false;
        }

        public void AddLog(string[] text)
        {
            foreach (string str in text)
            {
                logText.text += str + '\n';
            }
            logContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, logText.preferredHeight);
        }

        public void AddLog(string text)
        {
            logText.text += text + '\n';
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