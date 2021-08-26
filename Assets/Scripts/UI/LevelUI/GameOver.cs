using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Asakuma
{
    public class GameOver : MonoBehaviour
    {
        public GameObject gameOverUI;
        public AudioSource gameOverAudio;

        private void Awake()
        {
            
        }
        void Update()
        {

        }

        public void IsGameOver()
        {
            Time.timeScale = 0.7f;
            gameOverUI.SetActive(true);
            gameOverAudio.Play();
            StartCoroutine(GameOverTimeScale());
        }

        IEnumerator GameOverTimeScale()
        {
            yield return new WaitForSeconds(3f);
            Time.timeScale = 0;
        }
    }
}