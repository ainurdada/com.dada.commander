using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dada.Commander.UI.Core
{
    internal class PopUpHelper : MonoBehaviour
    {
        public bool IsShowed { get; private set; }
        public bool IsSelected { get; private set; }
        [SerializeField] GameObject itemPrefab;
        public int commandCount;
        public Color itemOriginalColor;
        public Color itemSelectionColor;
        public float fontSize;
        [HideInInspector] public string selectedCommand;

        List<string> _commands;
        List<GameObject> _items;
        public int selectedItemIndex = -1;

        private void Awake()
        {
            CleanAndHide();
        }

        public void Show(List<string> commands)
        {
            if (commands.Count < 2) return;
            if(_items == null) _items = new();
            if(commands.Count > commandCount) _commands = commands.GetRange(0, commandCount);
            else _commands = commands;

            foreach (string command in _commands)
            {
                GameObject item = Instantiate(itemPrefab, transform);

                Image image = item.GetComponent<Image>();
                image.color = itemOriginalColor;

                TextMeshProUGUI text = item.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                text.text = command;
                text.fontSize = fontSize;

                _items.Add(item);
            }

            IsShowed = true;
        }
        public void CleanAndHide()
        {
            for(int i = transform.childCount - 1; i >= 0; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            if(_items != null) _items.Clear();
            selectedItemIndex = -1;
            selectedCommand = null;
            IsShowed = false;
            IsSelected = false;
        }

        public void SelectNextItem()
        {
            SelectItem(selectedItemIndex + 1);
        }
        public void SelectPreviousItem()
        {
            SelectItem(selectedItemIndex - 1);
        }

        void SelectItem(int index)
        {
            if(selectedItemIndex != -1) _items[selectedItemIndex].GetComponent<Image>().color = itemOriginalColor;

            selectedItemIndex = Mathf.Clamp(index, -1, _items.Count - 1);

            if(selectedItemIndex == -1)
            {
                IsSelected = false;
                return;
            }

            _items[selectedItemIndex].GetComponent<Image>().color = itemSelectionColor;
            selectedCommand = _items[selectedItemIndex].transform.GetChild(0).GetComponent<TMP_Text>().text;
            IsSelected = true;
        }
    }
}

