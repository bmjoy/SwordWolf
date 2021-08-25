using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Asakuma
{
    public class ClearGame : MonoBehaviour
    {
        public GameObject levelMusic;
        public AudioSource levelComplete;
        public GameObject fadeOut;

        EnemyStats enemyStats;

        private void Awake()
        {
            enemyStats = GetComponent<EnemyStats>();
        }

        IEnumerator FadeScreen()
        {
            fadeOut.SetActive(true);
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene(4);
        }
    }
}
