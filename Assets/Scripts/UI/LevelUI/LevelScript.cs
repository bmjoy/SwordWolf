using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asakuma
{
    public class LevelScript : MonoBehaviour
    {
        public GameObject fadeIn;
        void Start()
        {
            StartCoroutine(FadeInOff());
        }

        IEnumerator FadeInOff()
        {
            yield return new WaitForSeconds(1);
            fadeIn.SetActive(false);
        }
    }
}