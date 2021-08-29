using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Asakuma
{
    public class ScrollCredits : MonoBehaviour
    {
        public GameObject creditsRun;
        void Start()
        {
            StartCoroutine(RollCredits());
        }
        IEnumerator RollCredits()
        {
            yield return new WaitForSeconds(0.5f);
            creditsRun.SetActive(true);
            yield return new WaitForSeconds(8f);
            SceneManager.LoadScene(1);
        }

    }
}