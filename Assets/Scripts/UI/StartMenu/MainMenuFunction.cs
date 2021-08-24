using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Asakuma
{
    public class MainMenuFunction : MonoBehaviour
    {
        [SerializeField] private AudioSource playButton;
        [SerializeField] private AudioSource buttonPress;

        private void Start()
        {
            Cursor.visible = true;
        }
        public void PlayGame()
        {
            playButton.Play();
            RedirectToLevel.redirectToLevel = 3;
            SceneManager.LoadScene(2);
        }

        public void PlayCredit()
        {
            buttonPress.Play();
            SceneManager.LoadScene(4);
        }

        public void QuitGame()
        {
            buttonPress.Play();
            Application.Quit();
        }
    }
}