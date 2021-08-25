using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Asakuma
{
    public class PauseGame : MonoBehaviour
    {
        public bool gamePaused = false;
        public AudioSource levelMusic;
        public GameObject pauseMenu;
        public AudioSource pauseJingle;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown("joystick button 6"))
            {
                if (gamePaused == false)
                {
                    pauseJingle.Play();
                    pauseMenu.SetActive(true);

                    Time.timeScale = 0;
                    gamePaused = true;
                    Cursor.visible = true;
                    levelMusic.Pause();
                }
                else
                {
                    pauseJingle.Pause();
                    pauseMenu.SetActive(false);

                    levelMusic.UnPause();
                    Cursor.visible = false;
                    gamePaused = false;
                    Time.timeScale = 1;

                }
            }
        }

        public void ResumeGame()
        {
            pauseMenu.SetActive(false);
            levelMusic.UnPause();
            Cursor.visible = false;
            gamePaused = false;
            Time.timeScale = 1;
        }

        public void RestartLevel()
        {
            pauseMenu.SetActive(false);
            levelMusic.UnPause();
            Cursor.visible = false;
            gamePaused = false;
            Time.timeScale = 1;
            SceneManager.LoadScene(2);
        }

        public void QuitToMenu()
        {
            pauseMenu.SetActive(false);
            levelMusic.UnPause();
            Cursor.visible = false;
            gamePaused = false;
            Time.timeScale = 1;
            SceneManager.LoadScene(1);
        }
    }
}