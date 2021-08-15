using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asakuma
{
    public class WorldEventManager : MonoBehaviour
    {
        //  fog wall
        public List<FogWall> fogWalls;
        public UIBossHealthBar bossHealthBar;
        public EnemyBossManager boss;


        public bool bossFightIsActive;  
        public bool bossHasBeenAwakened;    
        public bool bossHasBeenDefeated;   

        private void Awake()
        {
            bossHealthBar = FindObjectOfType<UIBossHealthBar>();
        }

        public void ActivateBossFight()
        {
            bossFightIsActive = true;
            bossHasBeenAwakened = true;
            bossHealthBar.SetUIHealthBarToActive();

            //  activate fog wall
            foreach(var fogWall in fogWalls)
            {
                fogWall.ActivateFogWall();
            }
        }

        public void BossHasBeenDefeated()
        {
            bossHasBeenDefeated = true;
            bossFightIsActive = false;

            //  deactivate fog walls
            foreach (var fogWall in fogWalls)
            {
                fogWall.DeactivateFogWall();
            }
        }
    }
}