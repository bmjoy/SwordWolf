using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asakuma
{
    public class WeaponHolderSlot : MonoBehaviour
    {
        public Transform parentOverride;
        public WeaponItem currentWeapon;
        public bool isLeftHandSlot;
        public bool isRightHandSlot;
        public bool isBackSlot;


        public GameObject currntWeaponModel;

        public void UnloadWeapon()
        {
            if (currntWeaponModel != null)
            {
                currntWeaponModel.SetActive(false);
            }
        }


        public void UnloadWeaponAndDestory()
        {
            if (currntWeaponModel != null)
            {
                Destroy(currntWeaponModel);
            }
        }


        public void LoadWeaponModel(WeaponItem weaponItem)
        {
            UnloadWeaponAndDestory();

            if (weaponItem == null)
            {
                UnloadWeapon();
                return;
            }

            GameObject model = Instantiate(weaponItem.modelPrefab) as GameObject;
            if (model != null)
            {
                if (parentOverride != null)
                {
                    model.transform.parent = parentOverride;
                }
                else
                {
                    model.transform.parent = transform;
                }

                model.transform.localPosition = Vector3.zero;
                model.transform.localRotation = Quaternion.identity;
                model.transform.localScale = Vector3.one;
            }

            currntWeaponModel = model;
        }

    }
}