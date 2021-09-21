using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asakuma
{
    public class EnemyStats : CharacterStats
    {
        //Animator animator;
        EnemyManager enemyManager;
        EnemyAnimatorManager enemyAnimatorManager;
        EnemyBossManager enemyBossManager;

        [SerializeField] private LevelComplete levelComplete;
        public UIEnemyHealthBar enemyHealthBar;
        public int soulsAwardedOnDeath { get; private set; } = 50;
        public bool isBoss;
        public bool isDamaged { get; private set; } = false;

        private void Awake()
        {
            enemyManager = GetComponent<EnemyManager>();
            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
            enemyBossManager = GetComponent<EnemyBossManager>();
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            levelComplete = FindObjectOfType<LevelComplete>();
        }

        void Start()
        {
            if (!isBoss)
            {
                enemyHealthBar.SetMaxHealth(maxHealth);
            }
        }



        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }


        public void TakeDamageNoAnimation(int damage)
        {
            if (!isDamaged)
                isDamaged = true;

            currentHealth = currentHealth - damage;
            enemyManager.damageSE.Play();

            if (!isBoss)
            {
                enemyHealthBar.SetHealth(currentHealth);
            }
            else if (isBoss && enemyBossManager != null)
            {
                enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
            }

            //  éÄñSÇµÇΩèÍçá
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
                HandleDeath();  //self
            }
        }


        public override void TakeDamage(int damage, string damageAnimation = "Damage_01")
        {
            if (!isDamaged)
                isDamaged = true;
            if (isDead)
                return;

            base.TakeDamage(damage, damageAnimation = "Damage_01");


            if (!isBoss)
            {
                enemyHealthBar.SetHealth(currentHealth);
            }
            else if (isBoss && enemyBossManager != null)
            {
                enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);

            }
            enemyAnimatorManager.PlayTargetAnimation(damageAnimation, true);
            enemyManager.damageSE.Play();

            if (currentHealth <= 0)
            {
                HandleDeath();
            }
        }

        private void HandleDeath()
        {
            currentHealth = 0;
            enemyAnimatorManager.PlayTargetAnimation("Dying", true);
            isDead = true;
            if (isBoss)
            {
                LevelCompleted();
            }
        }

        public void LevelCompleted()
        {
            levelComplete.BossDefeated();
        }
    }
}