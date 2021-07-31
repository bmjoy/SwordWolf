using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asakuma
{
    public class PlayerEffectsManager : MonoBehaviour
    {
        PlayerStats playerStats;
        WeaponSlotManager weaponSlotManager;

        //  the particles that will play of the current effent that is effecting the player(drinking estus, poison etc)
        public GameObject currentParticleFX;
        public GameObject instantiatedFXModel;
        public int amountBeHealed;

        private void Awake()
        {
            playerStats = GetComponentInParent<PlayerStats>();
            weaponSlotManager = GetComponent<WeaponSlotManager>();
        }

        public void HealPlayerFromEffect()
        {
            playerStats.HealPlayer(amountBeHealed);
            GameObject healParticles = Instantiate(currentParticleFX, playerStats.transform);
            Destroy(instantiatedFXModel.gameObject);
            weaponSlotManager.LoadBothWeaponOnSlots();
        }
    }
}