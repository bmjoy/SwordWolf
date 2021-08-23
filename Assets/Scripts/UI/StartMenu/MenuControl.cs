using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Asakuma
{
    public class MenuControl : MonoBehaviour
    {
        public void ButtonStart()
        {
            SceneManager.LoadScene(1);
        }

        public void ButtonCredit()
        {
            SceneManager.LoadScene(2);
        }

        public void ButtunQuit()
        {
            Application.Quit();
        }
    }
}