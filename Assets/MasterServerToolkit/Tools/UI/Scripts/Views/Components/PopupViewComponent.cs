﻿using MasterServerToolkit.Logging;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace MasterServerToolkit.UI
{
    public class PopupViewComponent : MonoBehaviour, IUIViewComponent
    {
        [Header("Lables Settings"), SerializeField]
        protected TextMeshProUGUI[] lables;

        [Header("Buttons Settings"), SerializeField]
        protected UIButton[] buttons;

        public IUIView Owner { get; set; }

        public virtual void OnOwnerAwake() { }

        public virtual void OnOwnerStart() { }

        public virtual void OnOwnerHide(IUIView owner) { }

        public virtual void OnOwnerShow(IUIView owner) { }

        public virtual void SetLables(params string[] values)
        {
            if (values.Length == 0)
            {
                Logs.Warn($"There is no need to use SetLables method of {name} because of you do not pass any values as its parameters");
                return;
            }

            for (int i = 0; i < values.Length; i++)
            {
                try
                {
                    lables[i].text = values[i];
                }
                catch
                {
                    Logs.Warn($"There is no lable assigned to {name} for value at index {i}");
                }
            }
        }

        public virtual void SetButtonsClick(params UnityAction[] actions)
        {
            if (actions.Length == 0)
            {
                Debug.LogWarning($"There is no need to use SetButtonsClick method of {name} because of you do not pass any action as its parameters");
                return;
            }

            for (int i = 0; i < actions.Length; i++)
            {
                try
                {
                    buttons[i].RemoveAllOnClickListeners();
                    buttons[i].AddOnClickListener(actions[i]);
                }
                catch
                {
                    Debug.LogWarning($"There is no button assigned to {name} for action at index {i}");
                }
            }
        }
    }
}
