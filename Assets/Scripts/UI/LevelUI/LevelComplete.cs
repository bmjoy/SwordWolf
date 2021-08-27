using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Asakuma
{
    public class LevelComplete : MonoBehaviour
    {
        public AudioSource levelMusic;
        public AudioSource levelCompleteJingle;
        public GameObject victoryText;
        public GameObject fadeOut;

        EnemyStats enemyStats;

        private void Awake()
        {
            victoryText.SetActive(false);
            enemyStats = GetComponent<EnemyStats>();
        }

        public void BossDefeated()
        {
            StartCoroutine(DefeatedBoss());
            StartCoroutine(FadeScreen());
        }

        IEnumerator DefeatedBoss()
        {
            levelMusic.Pause();
            levelCompleteJingle.Play();
            yield return new WaitForSeconds(1);
            victoryText.SetActive(true);
        }
        IEnumerator FadeScreen()
        {
            yield return new WaitForSeconds(8);
            fadeOut.SetActive(true);
            yield return new WaitForSeconds(2);
            SceneManager.LoadScene(4);
        }
    }
}
