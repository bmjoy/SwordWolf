using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Asakuma
{
    public class DamageCollider : MonoBehaviour
    {
        public CharacterManager characterManager;
        public PlayerManager playerManager ;
        Collider damageCollider;
        

        public bool enabledDamageColliderOnStartUp = false;
        public int currentWeaponDamage = 25;

        private void Awake()
        {
            playerManager = FindObjectOfType<PlayerManager>();
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = enabledDamageColliderOnStartUp;
        }

        public void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }
        public void DisableDamageCollider()
        {
            damageCollider.enabled = false;
        }


        private void OnTriggerEnter(Collider collision)
        {
            if (collision.tag == "Player")
            {
                PlayerStats playerStats = collision.GetComponent<PlayerStats>();
                CharacterManager enemyCharacterManager = collision.GetComponent<CharacterManager>();
                BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();

                if (enemyCharacterManager != null)
                {
                    if (enemyCharacterManager.isParrying)
                    {
                        characterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Parried", true); //  Check here if you are parriable
                        playerManager.parrySE.Play();
                        damageCollider.enabled = false;
                        return;
                    }
                    else if (shield != null && enemyCharacterManager.isBlocking)
                    {
                        //シールドダメージをパーセントで割る
                        float physicalDamageAfterBlock = currentWeaponDamage - (currentWeaponDamage * shield.blockingPhysicalDamageAbsorption) / 100;

                        if (playerStats != null)
                        {
                            playerStats.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), "Block_Guard");
                            return;
                        }
                    }
                }

                if (playerStats != null)
                {
                    playerStats.TakeDamage(currentWeaponDamage);
                }
            }

            if (collision.tag == "Enemy")
            {
                EnemyStats enemyStats = collision.GetComponent<EnemyStats>();
                CharacterManager enemyCharacterManager = collision.GetComponent<CharacterManager>();
                BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();


                if (enemyCharacterManager != null)
                {
                    if (enemyCharacterManager.isParrying)
                    {
                        characterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Parried", true);
                        damageCollider.enabled = false;
                        return;
                    }
                    else if (shield != null && enemyCharacterManager.isBlocking)
                    {
                        //シールドダメージをパーセントで割る
                        float physicalDamageAftertBlock = currentWeaponDamage - (currentWeaponDamage * shield.blockingPhysicalDamageAbsorption) / 100;

                        if (enemyStats != null)
                        {
                            enemyStats.TakeDamage(Mathf.RoundToInt(physicalDamageAftertBlock), "Block_Guard");
                            return;
                        }
                    }
                }

                if (enemyStats != null)
                {
                    if (enemyStats.isBoss)
                    {
                        enemyStats.TakeDamageNoAnimation(currentWeaponDamage);
                    }
                    else
                    {
                        enemyStats.TakeDamage(currentWeaponDamage);
                    }
                }
            }

            if (collision.tag == "Illusionary Wall")
            {
                IllusionaryWall illusionaryWall = collision.GetComponent<IllusionaryWall>();
                illusionaryWall.wallHasBeenHit = true;
            }
        }
    }
}