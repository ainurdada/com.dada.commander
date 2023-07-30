using TMPro;
using UnityEngine;

namespace Dada.Commander.UI
{
    internal class Preview : MonoBehaviour
    {
        [Header("Log")]
        [SerializeField] TextMeshProUGUI command;
        string commandText;
        Color commandColor;
        string prefix;
        Color prefixColor;

        [SerializeField] TextMeshProUGUI common;
        [SerializeField] TextMeshProUGUI error;

        public string PrefixText
        {
            set
            {
                prefix = value;
                ChangeCommandText();
            }
        }
        public string CommandText
        {
            set
            {
                commandText = value;
                ChangeCommandText();
            }
        }
        public Color CommandColor
        {
            set
            {
                commandColor = value;
                ChangeCommandText();
            }
        }
        public Color PrefixColor
        {
            set
            {
                prefixColor = value;
                ChangeCommandText();
            }
        }

        void ChangeCommandText()
        {
            command.text = $"<color=#{ColorUtility.ToHtmlStringRGB(prefixColor)}>{prefix}</color> " +
                    $"<color=#{ColorUtility.ToHtmlStringRGB(commandColor)}>{commandText}</color>";
        }
        public void SetCommonColor(Color color)
        {
            common.text = $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>log result</color>";
        }
        public void SetErrorColor(Color color)
        {
            error.text = $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>error</color>";
        }
    }
}
