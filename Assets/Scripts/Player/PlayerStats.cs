using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Asakuma
{
    public class PlayerStats : CharacterStats
    {
        PlayerManager playerManager;
        GameOver gameOver;
        HealthBar healthBar;
        StaminaBar staminaBar;
        FocusPointsBar focusPointsBar;
        PlayerAnimatorManager animatorHandler;

        [SerializeField] private float staminaRegenerationAmount = 1;
        [SerializeField] private float staminaRegenTimer = 0;
        [SerializeField] private bool canDamage = true;
        [SerializeField] private float continuousDamageTime = 0.5f;
        [SerializeField] private AudioSource damageSE;
        [SerializeField] private AudioSource guardSE;

        private void Awake()
        {
            playerManager = GetComponent<PlayerManager>();
            healthBar = FindObjectOfType<HealthBar>();
            staminaBar = FindObjectOfType<StaminaBar>();
            focusPointsBar = FindObjectOfType<FocusPointsBar>();
            animatorHandler = GetComponentInChildren<PlayerAnimatorManager>();

            gameOver = FindObjectOfType<GameOver>();
        }

        void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetCurrentHealth(currentHealth);

            maxStamina = SetMaxStaminaFromStaminaLevel();
            staminaBar.SetMaxStamina(maxStamina);
            currentStamina = maxStamina;

            maxFocusPoints = SetMaxFocusPointFromFocusLevel();
            currentFocusPoints = maxFocusPoints;
            focusPointsBar.SetMaxFocusPoints(maxFocusPoints);
            focusPointsBar.SetCurrentFocusPoints(currentFocusPoints);

        }


        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        private float SetMaxStaminaFromStaminaLevel()
        {
            maxStamina = staminaLevel * 10;
            return maxStamina;
        }

        private float SetMaxFocusPointFromFocusLevel()
        {
            maxFocusPoints = focusLevel * 10;
            return maxFocusPoints;
        }


        public override void TakeDamage(int damage, string damageAnimation = "Damage_01")
        {
            if (playerManager.isInvulnerable)
                return;
            if (playerManager.isParrying)
                return;
            if (!canDamage || currentHealth <= 0)
                return;

            base.TakeDamage(damage, damageAnimation = "Damage_01");
            canDamage = false;
            healthBar.SetCurrentHealth(currentHealth);

            if (!playerManager.isBlocking)
            {
                animatorHandler.PlayTargetAnimation(damageAnimation, true);
                damageSE.Play();
            }
            else
                guardSE.Play();

            StartCoroutine(WaitForDamage());

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                animatorHandler.PlayTargetAnimation("Dying", true);
                isDead = true;

                gameOver.IsGameOver();
            }
        }


        public void TakeDamageNoAnimation(int damage)
        {
            if (playerManager.isInvulnerable)
                return;
            if (playerManager.isParrying)
                return;
            if (!canDamage || currentHealth <= 0)
                return;

            currentHealth = currentHealth - damage;
            damageSE.Play();
            StartCoroutine(WaitForDamage());

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
            }
        }

        IEnumerator WaitForDamage()
        {
            yield return new WaitForSeconds(continuousDamageTime);
            canDamage = true;
        }

        public void TakeStaminaDamage(int damage)
        {
            currentStamina = currentStamina - damage;
            staminaBar.SetCurrentStamina(currentStamina);
        }

        public void RegenerateStamina()
        {
            if (playerManager.isInteracting)
            {
                staminaRegenTimer = 0;
            }
            else
            {
                staminaRegenTimer += Time.deltaTime;

                if (currentStamina < maxStamina && staminaRegenTimer > 1f)
                {
                    currentStamina += staminaRegenerationAmount * Time.deltaTime;
                    staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
                }
            }
        }

        public void HealPlayer(int healAmount)
        {
            currentHealth = currentHealth + healAmount;

            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }

            healthBar.SetCurrentHealth(currentHealth);
        }

        public void DeductFocusPoints(int focusPoints)
        {
            currentFocusPoints = currentFocusPoints - focusPoints;

            if (currentFocusPoints < 0)
            {
                currentFocusPoints = 0;
            }

            focusPointsBar.SetCurrentFocusPoints(currentFocusPoints);
        }

        public void AddSouls(int souls)
        {
            soulCount = soulCount + souls;
        }
    }
}