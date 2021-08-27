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
            fadeIn.SetActive(true);
            RedirectToLevel.redirectToLevel = 3;
            RedirectToLevel.nextLevel = 4;
            StartCoroutine(FadeInOff());
        }

        IEnumerator FadeInOff()
        {
            yield return new WaitForSeconds(1);
            fadeIn.SetActive(false);
        }
    }
}