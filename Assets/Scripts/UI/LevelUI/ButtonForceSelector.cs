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

        //void Update()
        //{
        //    if (EventSystem.current.currentSelectedGameObject == null)
        //    {
        //        if (firstSelected != null && firstSelected.gameObject.activeInHierarchy && firstSelected.interactable)
        //        {
        //            EventSystem.current.SetSelectedGameObject(firstSelected.gameObject);
        //        }
        //    }
        //}

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