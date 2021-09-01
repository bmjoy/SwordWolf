using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Asakuma
{
    public class ButtonForceSelector : MonoBehaviour
    {
        public Selectable firstSelected;
        void Start()
        {
            if (firstSelected == null)
            {
                firstSelected = GetComponentInChildren<Selectable>();
            }
        }

        private void OnEnable()
        {
            ForceSelect();
        }

        public void ForceSelect()
        {
            EventSystem.current.SetSelectedGameObject(firstSelected.gameObject);
        }
    }
}