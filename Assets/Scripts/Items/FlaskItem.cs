using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asakuma
{
    [CreateAssetMenu(menuName = "Items/Consumables/Flask")]
    public class FlaskItem : ConsumableItem
    {
        [Header("Flask Type")]
        public bool estusFlask;
        public bool ashenFlask;

        [Header("Recoverry Amount")]
        public int healthRecoverAmount;
        public int focusPointsRecoverAmount;

        [Header("Recovery FX")]
        public GameObject recoveryFX;

        public override void AttemptToConsumeItem
            (PlayerAnimatorManager playerAnimatorManager, WeaponSlotManager weaponSlotManager, PlayerEffectsManager playerEffectsManager)
        {
            base.AttemptToConsumeItem(playerAnimatorManager, weaponSlotManager, playerEffectsManager);
            GameObject flask = Instantiate(itemModel, weaponSlotManager.rightHandSlot.transform);


            //  体力を回復させFX再生
            playerEffectsManager.currentParticleFX = recoveryFX;
            playerEffectsManager.amountBeHealed = healthRecoverAmount;

            //  フラスコを持たせインスタンス化してドリンクanim再生
            playerEffectsManager.instantiatedFXModel = flask;
            weaponSlotManager.rightHandSlot.UnloadWeapon();

            //  ヒットせずに飲んでいる場合は/回復FXを再生する
        }
    }
}