using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Asakuma
{

    public class RedirectToLevel : MonoBehaviour
    {
        public static int redirectToLevel = 3;
        public static int nextLevel;

        void Update()
        {
            if (redirectToLevel == 3)
            {
                SceneManager.LoadScene(redirectToLevel);
            }
        }
    }
}