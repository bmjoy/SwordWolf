using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asakuma
{
    public class PlayerAttacker : MonoBehaviour
    {
        CameraHandler cameraHandler;
        PlayerAnimatorManager animatorHandler;
        PlayerEquipmentManager playerEquipmentManager;
        PlayerManager playerManager;
        PlayerStats playerStats;
        PlayerInventory playerInventory;
        InputHandler inputHandler;
        WeaponSlotManager weaponSlotManager;

        public string lastAttack;

        LayerMask backStabLayer = 1 << 12;
        LayerMask riposteLayer = 1 << 13;



        private void Awake()
        {
            cameraHandler = FindObjectOfType<CameraHandler>();
            animatorHandler = GetComponent<PlayerAnimatorManager>();
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            playerManager = GetComponentInParent<PlayerManager>();
            playerStats = GetComponentInParent<PlayerStats>();
            playerInventory = GetComponentInParent<PlayerInventory>();
            weaponSlotManager = GetComponent<WeaponSlotManager>();
            inputHandler = GetComponentInParent<InputHandler>();
        }

        public void HandleWeaponCombo(WeaponItem weapon)
        {
            if (playerStats.currentStamina <= 0)
                return;

            if (inputHandler.comboFlag)
            {
                animatorHandler.anim.SetBool("canDoCombo", false);

                if (lastAttack == weapon.oh_light_attack_01)
                {
                    animatorHandler.PlayTargetAnimation(weapon.oh_light_attack_02, true);
                }
                else if (lastAttack == weapon.th_light_attack_01)
                {
                    animatorHandler.PlayTargetAnimation(weapon.th_light_attack_02, true);
                }
            }
        }


        public void HandleLightAttack(WeaponItem weapon)
        {
            if (playerStats.currentStamina <= 0)
                return;

            weaponSlotManager.attackingWeapon = weapon;

            if (inputHandler.twoHandFlag)
            {
                animatorHandler.PlayTargetAnimation(weapon.th_light_attack_01, true);
                lastAttack = weapon.th_light_attack_01;
            }
            else
            {
                animatorHandler.PlayTargetAnimation(weapon.oh_light_attack_01, true);
                lastAttack = weapon.oh_light_attack_01;
            }
        }


        public void HandleHeavyAttack(WeaponItem weapon)
        {
            if (playerStats.currentStamina <= 0)
                return;

            weaponSlotManager.attackingWeapon = weapon;

            if (inputHandler.twoHandFlag)
            {

            }
            else
            {
                animatorHandler.PlayTargetAnimation(weapon.oh_heavy_attack_01, true);
                lastAttack = weapon.oh_heavy_attack_01;

            }
        }


        #region Input Actions
        public void HandleRBAction()
        {
            if (playerInventory.rightWeapon.isMeleeWeapon)
            {
                PerformRBMeleeAction();  //  Handle Melee Action
            }
            else if (playerInventory.rightWeapon.isSpellCaster || playerInventory.rightWeapon.isFaithCaster || playerInventory.rightWeapon.isPyroCaster)
            {
                PerformRBMagicAction(playerInventory.rightWeapon); //  Handle Magic Action
            }

        }

        public void HandleLBAction()
        {
            PerformLBBlockingAction();
        }

        public void HandleLTAction()
        {
            if (playerInventory.leftWeapon.isShieldWeapon)
            {
                PerformLTWeaponArt(inputHandler.twoHandFlag);
            }
            else if (playerInventory.leftWeapon.isMeleeWeapon)
            {
                // do a light attack
            }
        }
        #endregion


        #region Attack Actions
        private void PerformRBMeleeAction()
        {
            if (playerManager.canDoCombo)
            {
                inputHandler.comboFlag = true;
                HandleWeaponCombo(playerInventory.rightWeapon);
                inputHandler.comboFlag = false;
            }
            else
            {
                if (playerManager.isInteracting)
                    return;

                if (playerManager.canDoCombo)
                    return;

                animatorHandler.anim.SetBool("isUsingRightHand", true);
                HandleLightAttack(playerInventory.rightWeapon);
            }

        }

        private void PerformRBMagicAction(WeaponItem weapon)
        {
            if (playerManager.isInteracting)
                return;

            if (weapon.isFaithCaster)
            {
                if (playerInventory.currentSpell != null && playerInventory.currentSpell.isFaithSpell)
                {
                    if (playerStats.currentFocusPoints >= playerInventory.currentSpell.focusPointCost)
                    {
                        playerInventory.currentSpell.AttemptToCastSpell(animatorHandler, playerStats, weaponSlotManager);
                    }
                    else
                    {
                        animatorHandler.PlayTargetAnimation("Spell_False", true);
                    }
                }
            }
            else if (weapon.isPyroCaster)
            {
                if (playerInventory.currentSpell != null && playerInventory.currentSpell.isPyroSpell)
                {
                    if (playerStats.currentFocusPoints >= playerInventory.currentSpell.focusPointCost)
                    {
                        playerInventory.currentSpell.AttemptToCastSpell(animatorHandler, playerStats, weaponSlotManager);
                    }
                    else
                    {
                        animatorHandler.PlayTargetAnimation("Spell_False", true);
                    }
                }
            }
        }

        private void PerformLTWeaponArt(bool isTwoHanding)
        {
            if (playerManager.isInteracting)
                return;

            if (isTwoHanding)
            {
                //  If we are two handing perform weapon  rt for right weapon
            }
            else
            {
                animatorHandler.PlayTargetAnimation(playerInventory.leftWeapon.weapon_art, true);
                //  else perform weapon art for left weapon
            }
        }

        private void SuccessfullyCastSpell()
        {
            playerInventory.currentSpell.SuccessfullyCastSpell(animatorHandler, playerStats, cameraHandler, weaponSlotManager);
            animatorHandler.anim.SetBool("isFiringSpell", true);
        }
        #endregion


        #region Defense Actions
        private void PerformLBBlockingAction()
        {
            if (playerManager.isInteracting)
                return;

            if (playerManager.isBlocking)
                return;

            // dont move
            animatorHandler.PlayTargetAnimation("Block_Start", false, true);
            playerEquipmentManager.OpenBlockingCollider();
            playerManager.isBlocking = true;
        }
        #endregion


        public void AttemptBackStabOrRiposte()
        {
            if (playerStats.currentStamina <= 0)
                return;

            RaycastHit hit;

            //! BackStab
            if (Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position, transform.TransformDirection(Vector3.forward), out hit, 1f, backStabLayer))
            {

                CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
                DamageCollider rightWeapon = weaponSlotManager.rightHandDamageCollider;


                if (enemyCharacterManager != null)
                {
                    if (hit.transform.tag == "Enemy")
                    {
                        if (hit.transform.GetComponent<CharacterStats>().isDead == true)
                            return;
                    }
                    //  Check for team ID(cant backstab friends or yourself)
                    playerManager.transform.position = enemyCharacterManager.backStabCollider.criticalDamageStandPosition.position;

                    Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                    rotationDirection = hit.transform.position - playerManager.transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                    playerManager.transform.rotation = targetRotation;

                    int criticalDamage = playerInventory.rightWeapon.criticalDamageMultiplier * rightWeapon.currentWeaponDamage;
                    enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                    //Debug.Log("backstab");
                    animatorHandler.PlayTargetAnimation("Back_Stab", true);
                    enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Back_Stabbed", true);

                    //  do damage
                }
            }
            //! Riposte
            else if (Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position, transform.TransformDirection(Vector3.forward), out hit, 1f, riposteLayer))
            {
                //  check for team ID
                CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
                DamageCollider rightWeapon = weaponSlotManager.rightHandDamageCollider;

                if (enemyCharacterManager != null && enemyCharacterManager.canBeRiposted)
                {
                    if (hit.transform.tag == "Enemy")
                    {
                        if (hit.transform.GetComponent<CharacterStats>().isDead == true)
                            return;
                    }

                    playerManager.transform.position = enemyCharacterManager.riposteCollider.criticalDamageStandPosition.position;

                    Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                    rotationDirection = hit.transform.position - playerManager.transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                    playerManager.transform.rotation = targetRotation;

                    int criticalDamage = playerInventory.rightWeapon.criticalDamageMultiplier * rightWeapon.currentWeaponDamage;
                    enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                    animatorHandler.PlayTargetAnimation("Riposte", true);
                    enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Riposted", true);

                }
            }

        }
    }
}
