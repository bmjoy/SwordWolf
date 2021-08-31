using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asakuma
{
    public class InputHandler : MonoBehaviour
    {
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        public bool b_Input;
        public bool a_Input;
        public bool x_Input;
        public bool y_Input;
        public bool rb_Input;
        public bool rt_Input;
        public bool lt_Input;
        public bool lb_Input;
        public bool criticalAttack_Input;
        public bool jump_Input;
        public bool inventory_Input;
        public bool lockOn_Input;
        public bool right_Stick_Right_Input;
        public bool right_Stick_Left_Input;

        public bool d_Pad_Up;
        public bool d_Pad_Down;
        public bool d_Pad_Left;
        public bool d_Pad_Right;

        // Flag = is doing
        public bool rollFlag;
        public bool twoHandFlag;
        public bool sprintFlag;
        public bool comboFlag;
        public bool lockOnFlag;
        public bool inventoryFlag;
        public float rollInputTimer;

        public bool pause_Game;

        public Transform criticalAttackRayCastStartPoint;

        PlayerControls inputActions;
        PlayerAttacker playerAttacker;
        PlayerInventory playerInventory;
        PlayerManager playerManager;
        PlayerAnimatorManager playerAnimatorManager;
        PlayerEffectsManager playerEffectsManager;
        PlayerStats playerStats;
        BlockingCollider blockingCollider;
        WeaponSlotManager weaponSlotManager;
        CameraHandler cameraHandler;
        PlayerAnimatorManager animatorHandler;
        UIManager uiManager;

        Vector2 movementInput;
        Vector2 cameraInput;


        private void Awake()
        {
            playerAttacker = GetComponentInChildren<PlayerAttacker>();
            playerInventory = GetComponent<PlayerInventory>();
            playerManager = GetComponent<PlayerManager>();
            playerStats = GetComponent<PlayerStats>();
            playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
            playerEffectsManager = GetComponentInChildren<PlayerEffectsManager>();
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
            blockingCollider = GetComponentInChildren<BlockingCollider>();
            cameraHandler = FindObjectOfType<CameraHandler>();
            uiManager = FindObjectOfType<UIManager>();
            animatorHandler = GetComponentInChildren<PlayerAnimatorManager>();
        }


        public void OnEnable()
        {
            
            if (inputActions == null)
            {
                inputActions = new PlayerControls();

                inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
                inputActions.PlayerMovement.LockOnTargetRight.performed += i => right_Stick_Right_Input = true;
                inputActions.PlayerMovement.LockOnTargetLeft.performed += i => right_Stick_Left_Input = true;
                inputActions.PlayerQuickSlots.DPadRight.performed += i => d_Pad_Right = true;
                inputActions.PlayerQuickSlots.DPadLeft.performed += i => d_Pad_Left = true;

                inputActions.PlayerActions.RB.performed += i => rb_Input = true;
                inputActions.PlayerActions.RT.performed += i => rt_Input = true;
                inputActions.PlayerActions.LB.performed += i => lb_Input = true;
                inputActions.PlayerActions.LB.canceled += i => lb_Input = false;
                inputActions.PlayerActions.LT.performed += i => lt_Input = true;
                inputActions.PlayerActions.A.performed += i => a_Input = true;
                inputActions.PlayerActions.X.performed += i => x_Input = true;
                inputActions.PlayerActions.Roll.performed += i => b_Input = true;
                inputActions.PlayerActions.Roll.canceled += i => b_Input = false;

                inputActions.PlayerActions.Jump.performed += i => jump_Input = true;
                inputActions.PlayerActions.Inventory.performed += i => inventory_Input = true;
                inputActions.PlayerActions.LockOn.performed += i => lockOn_Input = true;

                inputActions.PlayerActions.Y.performed += i => y_Input = true;
                inputActions.PlayerActions.CriticalAttack.performed += i => criticalAttack_Input = true;
            }

            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        public void TickInput(float delta)
        {
            HandleMoveInput(delta);
            HandleRollInput(delta);
            HandleCombatInput(delta);
            HandleQuickSlotsInput();
            HandleInventoryInput();
            HandleLockOnInput();
            HandleTwoHandInput();
            HandleCriticalAttackInput();
            HandleUseConsumableInput();
        }

        private void HandleMoveInput(float delta)
        {
            horizontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            mouseX = cameraInput.x;
            mouseY = cameraInput.y;

        }

        private void HandleRollInput(float delta)
        {
            if (b_Input)
            {
                rollInputTimer += delta;    //rollFlag = true;

                if (playerStats.currentStamina <= 0)
                {
                    b_Input = false;
                    sprintFlag = false;
                }

                if (moveAmount > 0.5f && playerStats.currentStamina > 0)
                {
                    sprintFlag = true;
                }
            }
            else
            {
                sprintFlag = false;

                if (rollInputTimer > 0 && rollInputTimer < 0.5f)
                {
                    rollFlag = true;
                }
                rollInputTimer = 0;
            }
        }

        private void HandleCombatInput(float delta)
        {
            //  RB Input handle the RIGHT hand weapon's light attack
            if (rb_Input)
            {
                playerAttacker.HandleRBAction();
            }

            if (rt_Input)
            {
                playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
            }

            if (lt_Input)
            {
                if (twoHandFlag)
                {
                    //  if two handing handle weapon art

                }
                else
                {
                    //  else handle light if melee weapon
                    playerAttacker.HandleLTAction(); 
                }
            }

            if (lb_Input)
            {
                playerAttacker.HandleLBAction();
            }
            else
            {
                playerManager.isBlocking = false;

                if (blockingCollider.blockingCollider.enabled)
                {
                    blockingCollider.DisableBlockingCollider();
                }
            }
        }

        private void HandleQuickSlotsInput()
        {
            if (d_Pad_Right)
            {
                playerInventory.ChangeRightWeapon();
            }
            else if (d_Pad_Left)
            {
                playerInventory.ChangeLeftWeapon();
            }
        }


        private void HandleInventoryInput()
        {
            if (inventory_Input)
            {
                inventoryFlag = !inventoryFlag;

                if (inventoryFlag)
                {
                    uiManager.OpenSelectWindow();
                    uiManager.UpdateUI();
                    uiManager.hudWindow.SetActive(false);
                }
                else
                {
                    uiManager.CloseSelectWindow();
                    uiManager.CloseAllInventoryWindows();
                    uiManager.hudWindow.SetActive(true);
                }
            }
        }

        private void HandleLockOnInput()
        {
            if (lockOn_Input && lockOnFlag == false)
            {
                lockOn_Input = false;
                cameraHandler.HandleLockOn();

                if (cameraHandler.nearestLockOnTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.nearestLockOnTarget;
                    lockOnFlag = true;
                }
            }
            else if (lockOn_Input && lockOnFlag)
            {
                lockOn_Input = false;
                lockOnFlag = false;
                cameraHandler.ClearLockOnTargets();
            }

            if (lockOnFlag && right_Stick_Left_Input)
            {
                right_Stick_Left_Input = false;
                cameraHandler.HandleLockOn();

                if (cameraHandler.leftLockTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.leftLockTarget;
                }
            }

            if (lockOnFlag && right_Stick_Right_Input)
            {
                right_Stick_Right_Input = false;
                cameraHandler.HandleLockOn();

                if (cameraHandler.rightLockTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.rightLockTarget;
                }
            }

            cameraHandler.SetCameraHeight();
        }

        private void HandleTwoHandInput()
        {
            if (y_Input)
            {
                y_Input = false;
                twoHandFlag = !twoHandFlag;

                if (twoHandFlag)
                {
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);   //  enable two handing
                }
                else
                {
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false); //  disable two handing
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.leftWeapon, true);
                }
            }

        }


        private void HandleCriticalAttackInput()
        {
            if (criticalAttack_Input)
            {
                Debug.Log("criticalAttackInput");
                criticalAttack_Input = false;
                playerAttacker.AttemptBackStabOrRiposte();
            }
        }

        private void HandleUseConsumableInput()
        {
            if (x_Input)
            {
                x_Input = false;
                playerInventory.currentConsumable.AttemptToConsumeItem(playerAnimatorManager, weaponSlotManager, playerEffectsManager);
            }
        }
    }
}
