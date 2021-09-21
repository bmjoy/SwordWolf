using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Asakuma
{
    public class WeaponInventorySlot : MonoBehaviour
    {
        PlayerInventory playerInventory;
        WeaponSlotManager weaponSlotManager;
        UIManager uiManager;
        InputHandler inputHandler;

        public Image icon;
        WeaponItem item;

        private void Awake()
        {
            inputHandler = FindObjectOfType<InputHandler>();
            playerInventory = FindObjectOfType<PlayerInventory>();
            weaponSlotManager = FindObjectOfType<WeaponSlotManager>();
            uiManager = FindObjectOfType<UIManager>();
        }

        public void AddItem(WeaponItem newItem)
        {
            item = newItem;
            icon.sprite = item.itemIcon;
            icon.enabled = true;
            gameObject.SetActive(true);
        }

        public void ClearInventorySlot()
        {
            item = null;
            icon.sprite = null;
            icon.enabled = false;
            gameObject.SetActive(false);
        }

        public void EquipThisItem()
        {   
            if (uiManager.rightHandslot01Selected)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInRightHandSlots[0]);   //  現在のアイテムを追加
                playerInventory.weaponsInRightHandSlots[0] = item;  //  新しく入手したインベントリを装備する
                playerInventory.weaponsInventory.Remove(item);  //  インベントリからこのアイテムを削除
            }
            else if (uiManager.rightHandslot02Selected)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInRightHandSlots[1]);
                playerInventory.weaponsInRightHandSlots[1] = item;
                playerInventory.weaponsInventory.Remove(item);
            }
            else if (uiManager.leftHandslot01Selected)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInLeftHandSlots[0]);
                playerInventory.weaponsInLeftHandSlots[0] = item;
                playerInventory.weaponsInventory.Remove(item);
            }
            else if (uiManager.leftHandslot02Selected)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInLeftHandSlots[1]);
                playerInventory.weaponsInLeftHandSlots[1] = item;
                playerInventory.weaponsInventory.Remove(item);
            }
            else
            {
                Time.timeScale = 1;
                uiManager.hudWindow.SetActive(true);
                inputHandler.inventoryFlag = false;
                return;
            }

            playerInventory.rightWeapon = playerInventory.weaponsInRightHandSlots[playerInventory.currentRightWeaponIndex];
            playerInventory.leftWeapon = playerInventory.weaponsInLeftHandSlots[playerInventory.currentLeftWeaponIndex];

            weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
            weaponSlotManager.LoadWeaponOnSlot(playerInventory.leftWeapon, true);

            uiManager.equipmentWindowUI.LoadWeaponsOnEquipmentScreen(playerInventory);
            uiManager.ResetAllSelectedSlots();
            uiManager.hudWindow.SetActive(true);
            inputHandler.inventoryFlag = false;

        }
    }
}