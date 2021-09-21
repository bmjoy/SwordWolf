using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asakuma
{
    public class SpellItem : Item
    {
        public GameObject spellWarmUpFX;
        public GameObject spellCastFX;
        public string spellAnimation;

        [Header("Spell Cost")]
        public int focusPointCost;

        [Header("Spell Type")]
        public bool isFaithSpell;
        public bool isMagicSpell;
        public bool isPyroSpell;

        [Header("Spell Description")]
        [TextArea]
        public string spellDescription;


        public virtual void AttemptToCastSpell
            (PlayerAnimatorManager animatorHandler, PlayerStats playerStats, WeaponSlotManager weaponSlotManager)
        {
            //Debug.Log("You attempt to cast a spell");

        }

        public virtual void SuccessfullyCastSpell
            (PlayerAnimatorManager animatorHandler, PlayerStats playerStats, CameraHandler cameraHandler, WeaponSlotManager weaponSlotManager)
        {
            //Debug.Log("You succenssfully cast a spell");
            playerStats.DeductFocusPoints(focusPointCost);
        }
    }
}