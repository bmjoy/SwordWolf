using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asakuma
{
    [CreateAssetMenu(menuName = "Spells/Healing Spell")]
    public class HealingSpell : SpellItem
    {
        [SerializeField] private int healAmount;

        public override void AttemptToCastSpell
            (PlayerAnimatorManager animatorHandler, PlayerStats playerStats, WeaponSlotManager weaponSlotManager)
        {
            base.AttemptToCastSpell(animatorHandler, playerStats, weaponSlotManager);
            GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, animatorHandler.transform);
            animatorHandler.PlayTargetAnimation(spellAnimation, true);
            //Debug.Log("Attempting to cast spell");
        }

        public override void SuccessfullyCastSpell
            (PlayerAnimatorManager animatorHandler, PlayerStats playerStats, CameraHandler cameraHandler, WeaponSlotManager weaponSlotManager)
        {
            base.SuccessfullyCastSpell(animatorHandler, playerStats, cameraHandler, weaponSlotManager);
            GameObject instantiatedSpellFX = Instantiate(spellCastFX, animatorHandler.transform);
            playerStats.HealPlayer(healAmount);
            //Debug.Log("Spell cast success");
        }
    }
} 