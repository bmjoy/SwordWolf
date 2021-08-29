using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Asakuma
{
    public class GameOver : MonoBehaviour
    {
        //public bool isGameOver = false;
        public GameObject gameOverUI;
        public AudioSource levelMusic;
        public AudioSource gameOverAudio;

        private void Awake()
        {
            gameOverUI.SetActive(false);
        }
        void Update()
        {

        }

        public void IsGameOver()
        {
            //isGameOver = true;
            //Time.timeScale = 0.7f;
            levelMusic.Pause();
            gameOverAudio.Play();
            Cursor.visible = true;
            StartCoroutine(GameOverTimeDelay());
        }

        IEnumerator GameOverTimeDelay()
        {
            yield return new WaitForSeconds(3f);
            gameOverUI.SetActive(true);
            Time.timeScale = 0;

            //yield return new WaitForSeconds(2.5f);
        }
    }
}