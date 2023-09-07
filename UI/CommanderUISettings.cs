using Dada.Commander.Core;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Dada.Commander.UI
{
    [ExecuteAlways]
    public class CommanderUISettings : MonoBehaviour
    {
        #region Instance
        static CommanderUISettings instance;
        public static CommanderUISettings Instance { get { return instance; } }
        #endregion

        [SerializeField] bool hideAtTheStart;

        [Header("Background")]
        [SerializeField] Texture2D logBackgroundTexture;
        [SerializeField] Color logBackgroundColor;
        [SerializeField] Texture2D inputBackgroundTexture;
        [SerializeField] Color inputBackgroundColor;

        [Header("Log settings")]
        [SerializeField] CommandLog commandLog;
        [SerializeField] LogResult logResult;
        [SerializeField, Tooltip("Duplicate log methods into unity console")] bool duplicateLog;

        [Header("Helper settings")]
        [SerializeField] bool showPopUpHelper;
        [SerializeField] Color helperColor;
        [SerializeField] int commandCount;


        #region Properties
        public bool HideAtTheStart
        {
            get { return hideAtTheStart; }
            set
            {
                hideAtTheStart = value;
                CommanderUIWindow.Instance.hideAtTheBeginning = hideAtTheStart;
            }
        }
        public Color LogBackgroundColor
        {
            get { return logBackgroundColor; }
            set
            {
                logBackgroundColor = value;
                CommanderUIWindow.Instance.logBackground.color = logBackgroundColor;
            }
        }
        public Color InputBackgroundColor
        {
            get { return inputBackgroundColor; }
            set
            {
                inputBackgroundColor = value;
                CommanderUIWindow.Instance.inputBackGround.color = inputBackgroundColor;
            }
        }
        public Color CommandColor
        {
            get { return commandLog.commandColor; }
            set
            {
                commandLog.commandColor = value;
                CommanderUIWindow.Instance.cmdColor = $"#{ColorUtility.ToHtmlStringRGB(commandLog.commandColor)}";
            }
        }
        public string CommandPrefix
        {
            get { return commandLog.commandPrefix; }
            set
            {
                commandLog.commandPrefix = value;
                CommanderUIWindow.Instance.commandPrefix = commandLog.commandPrefix;
            }
        }
        public Color LogColor
        {
            get { return CommanderSettings.LogColor; }
            set
            {
                CommanderSettings.LogColor = value;
            }
        }
        public Color LogErrorColor
        {
            get { return CommanderSettings.LogErrorColor; }
            set
            {
                CommanderSettings.LogErrorColor = value;
            }
        }
        public Color LogWarningColor
        {
            get { return CommanderSettings.LogWarningColor; }
            set
            {
                CommanderSettings.LogWarningColor = value;
            }
        }
        public Color PrefixColor
        {
            get { return commandLog.prefixColor; }
            set
            {
                commandLog.prefixColor = value;
                CommanderUIWindow.Instance.prefixColor = $"#{ColorUtility.ToHtmlStringRGB(commandLog.prefixColor)}";
            }
        }
        #endregion

#if UNITY_EDITOR
        public void OnValidate()
        {
            if (!gameObject.activeInHierarchy || !gameObject.activeSelf)
                return;

            if (gameObject.scene.name.Equals(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name))
            {
                ApplyChanges();
            }
        }
#endif
        private IEnumerator Start()
        {
            if (Application.isPlaying)
            {
                instance = this;
                HideAtTheStart = hideAtTheStart;
                yield return new WaitForEndOfFrame();
            }
            ApplyChanges();
        }

        void ApplyChanges()
        {
            if (CommanderUIWindow.Instance != null)
            {
                LogBackgroundColor = logBackgroundColor;
                InputBackgroundColor = inputBackgroundColor;
                CommandColor = commandLog.commandColor;
                CommandPrefix = commandLog.commandPrefix;
                LogColor = logResult.logColor;
                LogErrorColor = logResult.logErrorColor;
                LogWarningColor = logResult.logWarningColor;
                PrefixColor = commandLog.prefixColor;

                CommanderUIWindow.Instance.logBackground.color = logBackgroundColor;
                CommanderUIWindow.Instance.inputBackGround.color = inputBackgroundColor;
                SetTextureToImage(CommanderUIWindow.Instance.logBackground, logBackgroundTexture);
                SetTextureToImage(CommanderUIWindow.Instance.inputBackGround, inputBackgroundTexture);

                void SetTextureToImage(Image image, Texture2D texture)
                {
                    if (texture != null)
                    {
                        Sprite sprite = Sprite.Create(
                            texture,
                            new Rect(0, 0, texture.width, texture.height),
                            new Vector2(0, 0));
                        image.sprite = sprite;
                    }
                    else
                    {
                        image.sprite = null;
                    }
                }

                CommanderUIWindow.Instance.preview.CommandText = "command";
                CommanderUIWindow.Instance.preview.CommandColor = commandLog.commandColor;
                CommanderUIWindow.Instance.preview.PrefixText = commandLog.commandPrefix;
                CommanderUIWindow.Instance.preview.PrefixColor = commandLog.prefixColor;

                CommanderUIWindow.Instance.preview.SetCommonColor(LogColor);
                CommanderUIWindow.Instance.preview.SetErrorColor(LogErrorColor);
                CommanderUIWindow.Instance.preview.SetWarningColor(LogWarningColor);
            }
            CommanderSettings.DuplicateLog = duplicateLog;
        }

        [Serializable]
        class CommandLog
        {
            public Color commandColor;
            public string commandPrefix;
            public Color prefixColor;
        }

        [Serializable]
        class LogResult
        {
            public Color logColor;
            public Color logErrorColor;
            public Color logWarningColor;
        }
    }
}
