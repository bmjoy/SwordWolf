using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asakuma
{
    public class UIManager : MonoBehaviour
    {
        public PlayerInventory playerInventory;
        public EquipmentWindowUI equipmentWindowUI;

        [Header("UI Windows")]
        public GameObject hudWindow;
        public GameObject selectWindow;
        public GameObject equipmentScreenWindow;
        public GameObject weaponInventoryWindow;

        [Header("Equipment Window Slot Selected")]
        public bool rightHandslot01Selected;
        public bool rightHandslot02Selected;
        public bool leftHandslot01Selected;
        public bool leftHandslot02Selected;

        [Header("Weapon Inventory")]
        public GameObject weaponInventorySlotsPrefab;
        public Transform weaponInventorySlotsParent;
        WeaponInventorySlot[] weaponInventorySlots;


        private void Awake()
        {
            //equipmentWindowUI = FindObjectOfType<EquipmentWindowUI>();
        }

        private void Start()
        {
            weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
            equipmentWindowUI.LoadWeaponsOnEquipmentScreen(playerInventory);
        }

        public void UpdateUI()
        {
            #region Weapon Inventory Slots

            for (int i = 0; i < weaponInventorySlots.Length; i++)
            {
                //  インベントリにアイテムを加える
                if (i < playerInventory.weaponsInventory.Count)
                {
                    if (weaponInventorySlots.Length < playerInventory.weaponsInventory.Count)
                    {
                        Instantiate(weaponInventorySlotsPrefab, weaponInventorySlotsParent);
                        weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
                    }

                    weaponInventorySlots[i].AddItem(playerInventory.weaponsInventory[i]);
                }
                //  インベントリを空にする
                else
                {
                    weaponInventorySlots[i].ClearInventorySlot();
                }
            }
            #endregion
        }

        public void OpenSelectWindow()
        {
            selectWindow.SetActive(true);
            Time.timeScale = 0.5f;
        }

        public void CloseSelectWindow()
        {
            selectWindow.SetActive(false);
            Time.timeScale = 1;
        }

        public void CloseAllInventoryWindows()
        {
            ResetAllSelectedSlots();
            weaponInventoryWindow.SetActive(false);
            equipmentScreenWindow.SetActive(false);
            Time.timeScale = 1;
        }

        public void ResetAllSelectedSlots()
        {
            rightHandslot01Selected = false;
            rightHandslot02Selected = false;
            leftHandslot01Selected = false;
            leftHandslot02Selected = false;
            Time.timeScale = 1;
        }
    }
}