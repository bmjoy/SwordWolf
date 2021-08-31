using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Asakuma
{
    public class PlayerManager : CharacterManager
    {
        InputHandler inputHandler;
        Animator anim;
        CameraHandler cameraHandler;
        PlayerStats playerStats;
        PlayerAnimatorManager playerAnimatorManager;
        PlayerLocomotion playerLocomotion;

        InteractableUI interactableUI;
        public GameObject interactableUIGameObject; //UI popup
        public GameObject itemInteractableGameObject;
        public AudioSource parrySE;


        public bool isInteracting;

        [Header("Player Flags")]
        public bool isSprinting;
        public bool isInAir;
        public bool isGrounded;
        public bool canDoCombo;
        public bool isUsingRightHand;
        public bool isUsingLeftHand;
    

        private void Awake()
        {
            cameraHandler = FindObjectOfType<CameraHandler>();
            backStabCollider = GetComponentInChildren<CriticalDamageCollider>();
            inputHandler = GetComponent<InputHandler>();
            playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
            anim = GetComponentInChildren<Animator>();
            playerStats = GetComponent<PlayerStats>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
            interactableUI = FindObjectOfType<InteractableUI>();
        }


        void Update()
        {
            float delta = Time.deltaTime;

            isInteracting = anim.GetBool("isInteracting");  
            canDoCombo = anim.GetBool("canDoCombo");
            isUsingRightHand = anim.GetBool("isUsingRightHand");
            isUsingLeftHand = anim.GetBool("isUsingLeftHand");
            isInvulnerable = anim.GetBool("isInvulnerable");
            isFiringSpell = anim.GetBool("isFiringSpell");

            anim.SetBool("isBlocking", isBlocking);
            anim.SetBool("isInAir", isInAir);
            anim.SetBool("isDead", playerStats.isDead);

            inputHandler.TickInput(delta);
            playerAnimatorManager.canRotate = anim.GetBool("canRotate");
            playerLocomotion.HandleRollingAndSprinting(delta);
            playerLocomotion.HandleJumping();
            playerLocomotion.HandleRotation(delta);
            playerStats.RegenerateStamina();

            CheckForInteractableObject();
        }



        private void FixedUpdate()
        {
            float delta = Time.deltaTime;

            playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
            playerLocomotion.HandleMovement(delta);
        }

        private void LateUpdate()
        {
            inputHandler.rollFlag = false;
            inputHandler.rb_Input = false;
            inputHandler.rt_Input = false;
            inputHandler.lt_Input = false;

            inputHandler.d_Pad_Up = false;
            inputHandler.d_Pad_Down = false;
            inputHandler.d_Pad_Left = false;
            inputHandler.d_Pad_Right = false;
            
            inputHandler.a_Input = false;
            inputHandler.jump_Input = false;
            inputHandler.inventory_Input = false;

            float delta = Time.deltaTime;

            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
            }

            if (isInAir)
            {
                playerLocomotion.inAirTimer = playerLocomotion.inAirTimer + Time.deltaTime;
            }
        }


        #region Player Interactions

        public void CheckForInteractableObject()
        {
            RaycastHit hit;

            if (Physics.SphereCast(transform.position, 0.3f, transform.forward, out hit, 1f, cameraHandler.ignoreLayers))
            {
                if (hit.collider.tag == "Interactable")
                {
                    Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                    if (interactableObject != null)
                    {
                        string interactableText = interactableObject.interactableText;
                        interactableUI.interactableText.text = interactableText;
                        //  Set the UI Text to the Interactable Object's Text
                        interactableUIGameObject.SetActive(true);   //  Set the Text pop up to true

                        if (inputHandler.a_Input)
                        {
                            hit.collider.GetComponent<Interactable>().Interact(this);
                        }
                    }
                }
            }
            else
            {
                if (interactableUIGameObject != null)
                {
                    interactableUIGameObject.SetActive(false);
                }

                if (itemInteractableGameObject != null && inputHandler.a_Input)
                {
                    itemInteractableGameObject.SetActive(false);
                }
            }
        }

        public void OpenChestInteraction(Transform playerStandsHereWhenOpeningChest)
        {
            playerLocomotion.rigidbody.velocity = Vector3.zero; // stops the player from ice skating
            transform.position = playerStandsHereWhenOpeningChest.transform.position;
            playerAnimatorManager.PlayTargetAnimation("Open_Chest", true);
        }

        public void PassThroughFogWallInteraction(Transform fogWallEntrance)
        {
            playerLocomotion.rigidbody.velocity = Vector3.zero; //  stop the player from ice skating

            //  wanna make sure we face the fog wall first
            Vector3 rotationDirection = fogWallEntrance.transform.forward;
            Quaternion turnRotation = Quaternion.LookRotation(rotationDirection);
            transform.rotation = turnRotation;
            //  rotate over time so it does not look as rigid

            playerAnimatorManager.PlayTargetAnimation("Pass_Through_Fog", true);
        }

        #endregion
    }
}