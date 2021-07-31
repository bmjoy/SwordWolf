using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Asakuma
{
    public class CameraHandler : MonoBehaviour
    {
        InputHandler inputHandler;
        PlayerManager playerManager;

        public Transform targetTransform;
        public Transform cameraTransform;
        public Transform cameraPivotTransform;
        private Transform myTransform;
        private Vector3 cameraTransformPosition;
        public LayerMask ignoreLayers;
        public LayerMask environmentLayer;

        private Vector3 cameraFollowVelocity = Vector3.zero;

        public static CameraHandler singleton;

        public float lookSpeed = 0.1f;
        public float followSpeed = 0.1f;
        public float pivotSpeed = 0.03f;

        private float targetPosition;
        private float defaultPosition;
        private float lookAngle;
        private float pivotAngle;
        public float minimumPivot = -35;
        public float maximumPivot = 35;

        public float cameraShereRradius = 0.2f;
        public float cameraCollisionOffset = 0.2f;
        public float minimumCollisionOffset = 0.2f;
        public float lockedPivotPosition = 2.25f;
        public float unlockedPivotPosition = 1.65f;

        public CharacterManager currentLockOnTarget;

        List<CharacterManager> availableTargets = new List<CharacterManager>();
        public CharacterManager nearestLockOnTarget;
        public CharacterManager leftLockTarget;
        public CharacterManager rightLockTarget;
        public float maximumLockOnDistance = 30;


        private void Awake()
        {
            singleton = this;
            myTransform = transform;
            defaultPosition = cameraTransform.localPosition.z;
            ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
            targetTransform = FindObjectOfType<PlayerManager>().transform;
            inputHandler = FindObjectOfType<InputHandler>();
            playerManager = FindObjectOfType<PlayerManager>();
        }

        private void Start()
        {
            environmentLayer = LayerMask.NameToLayer("Environment");
        }


        public void FollowTarget(float delta)
        {
            Vector3 targetPosition = Vector3.SmoothDamp(myTransform.position, targetTransform.position, ref cameraFollowVelocity, delta / followSpeed);
            // Vector3 targetPosition = Vector3.Lerp(myTransform.position, targetTransform.position, delta / followSpeed);
            //!  カメラがカクカクになるため修正

            myTransform.position = targetPosition;

            HandleCameraCollisions(delta);
        }


        public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
        {
            if (inputHandler.lockOnFlag == false && currentLockOnTarget == null)
            {
                lookAngle += (mouseXInput * lookSpeed) / delta;
                pivotAngle -= (mouseYInput * pivotSpeed) / delta;
                pivotAngle = Mathf.Clamp(pivotAngle, minimumPivot, maximumPivot);

                Vector3 rotation = Vector3.zero;
                rotation.y = lookAngle;
                Quaternion targetRotation = Quaternion.Euler(rotation);
                myTransform.rotation = targetRotation;

                rotation = Vector3.zero;
                rotation.x = pivotAngle;

                targetRotation = Quaternion.Euler(rotation);
                cameraPivotTransform.localRotation = targetRotation;
            }
            else
            {
                //float velocity = 0;

                Vector3 dir = currentLockOnTarget.transform.position - transform.position;
                dir.Normalize();
                dir.y = 0;

                Quaternion targetRotation = Quaternion.LookRotation(dir);
                transform.rotation = targetRotation;

                dir = currentLockOnTarget.transform.position - cameraPivotTransform.position;
                dir.Normalize();

                targetRotation = Quaternion.LookRotation(dir);
                Vector3 eulerAngle = targetRotation.eulerAngles;
                eulerAngle.y = 0;
                cameraPivotTransform.localEulerAngles = eulerAngle;
            }
        }


        private void HandleCameraCollisions(float delta)
        {
            targetPosition = defaultPosition;
            RaycastHit hit;
            Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
            direction.Normalize();

            if (Physics.SphereCast(cameraPivotTransform.position, cameraShereRradius, direction, out hit, Mathf.Abs(targetPosition), ignoreLayers))
            {
                float dis = Vector3.Distance(cameraPivotTransform.position, hit.point);
                targetPosition = -(dis - cameraCollisionOffset);
            }

            if (Mathf.Abs(targetPosition) < minimumCollisionOffset)
            {
                targetPosition = -minimumCollisionOffset;
            }

            cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta / 0.2f);
            cameraTransform.localPosition = cameraTransformPosition;
        }


        public void HandleLockOn()
        {
            #region user comment
            //    availableTargets.Clear();

            //    float shortestDistance = Mathf.Infinity;
            //    float shortestDistanceOfLeftTarget = Mathf.Infinity;
            //    float shortestDistanceOfRightTarget = Mathf.Infinity;

            //    Collider[] colliders = Physics.OverlapSphere(targetTransform.position, 26);

            //    if (colliders.Length > 0)
            //    {
            //        for (int i = 0; i < colliders.Length; i++)
            //        {
            //            CharacterManager character = colliders[i].GetComponent<CharacterManager>();

            //            if (character != null)
            //            {
            //                Vector3 lockTargetDirection = character.transform.position - transform.position;

            //                float distanceFromTarget = Vector3.Distance(transform.position, character.transform.position);
            //                float viewableAngle = Vector3.Angle(lockTargetDirection, transform.forward);
            //                RaycastHit hit;

            //                //Make sure we don't lock onto ourselves and the target is on screen and we are close enough to lock on
            //                if (character.transform.root != targetTransform.transform.root && viewableAngle > -50 && viewableAngle < 50 && distanceFromTarget <= maximumLockOnDistance)
            //                {
            //                    if (Physics.Linecast(playerManager.lockOnTransform.position, character.lockOnTransform.position, out hit))
            //                    {
            //                        Debug.DrawLine(playerManager.lockOnTransform.position, character.lockOnTransform.position);

            //                        if (hit.transform.gameObject.layer == enviromentLayer)
            //                        {
            //                            //Cannot lock onto target, object in the way
            //                        }
            //                        else
            //                        {
            //                            availableTargets.Add(character);
            //                        }
            //                    }
            //                }
            //            }
            //        }

            //        if (availableTargets.Count > 0)
            //        {
            //            for (int k = 0; k < availableTargets.Count; k++)
            //            {
            //                float distFromTarget = Vector3.Distance(transform.position, availableTargets[k].transform.position);

            //                if (distFromTarget < shortestDistance)
            //                {
            //                    shortestDistance = distFromTarget;
            //                    nearestLockOnTarget = availableTargets[k].lockOnTransform;
            //                }

            //                if (inputHandler.lockOnFlag)
            //                {
            //                    //This is the big change, from enemy position to player
            //                    Vector3 relativePlayerPostion = transform.InverseTransformPoint(availableTargets[k].transform.position);

            //                    var distanceFromLeftTarget = 1000f; //currentLockOnTarget.transform.position.x - availableTargets[k].transform.position.x;
            //                    var distanceFromRightTarget = 1000f; //currentLockOnTarget.transform.position.x + availableTargets[k].transform.position.x;

            //                    if (relativePlayerPostion.x < 0.00)
            //                    {
            //                        distanceFromLeftTarget = Vector3.Distance(currentLockOnTarget.position, availableTargets[k].transform.position);
            //                    }

            //                    else if (relativePlayerPostion.x > 0.00)
            //                    {
            //                        distanceFromRightTarget = Vector3.Distance(currentLockOnTarget.position, availableTargets[k].transform.position);
            //                    }

            //                    if (relativePlayerPostion.x < 0.00 && distanceFromLeftTarget < shortestDistanceOfLeftTarget)
            //                    {
            //                        shortestDistanceOfLeftTarget = distanceFromLeftTarget;
            //                        leftLockTarget = availableTargets[k].lockOnTransform;
            //                    }

            //                    if (relativePlayerPostion.x > 0.00 && distanceFromRightTarget < shortestDistanceOfRightTarget)
            //                    {
            //                        shortestDistanceOfRightTarget = distanceFromRightTarget;
            //                        rightLockTarget = availableTargets[k].lockOnTransform;
            //                    }
            //                }
            //            }
            //        }

            //        else
            //        {
            //            Debug.Log("no lock on targets found A");
            //        }
            //    }

            //    else
            //    {
            //        Debug.Log("no lock on targets found B");
            //    }
            //}

            #endregion

            float shortestDistance = Mathf.Infinity;
            float shortestDistanceOfLeftTarget = -Mathf.Infinity;
            float shortestDistanceOfRightTarget = Mathf.Infinity;

            Collider[] colliders = Physics.OverlapSphere(targetTransform.position, 26); //半径26ユニット

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager character = colliders[i].GetComponent<CharacterManager>();

                if (character != null)
                {
                    Vector3 lockTargetDirection = character.transform.position - targetTransform.position;
                    float distanceFromTarget = Vector3.Distance(targetTransform.position, character.transform.position);
                    float viewableAngle = Vector3.Angle(lockTargetDirection, cameraTransform.forward);
                    RaycastHit hit;

                    if (character.transform.root != targetTransform.transform.root && viewableAngle > -50 && viewableAngle < 50 && distanceFromTarget <= maximumLockOnDistance)
                    {
                        if (Physics.Linecast(playerManager.lockOnTransform.position, character.lockOnTransform.position, out hit))
                        {
                            Debug.DrawLine(playerManager.lockOnTransform.position, character.lockOnTransform.position);

                            if (hit.transform.gameObject.layer == environmentLayer)
                            {
                                Debug.Log("えんば");
                            }
                            else
                            {
                                availableTargets.Add(character);
                            }
                        }
                    }
                }
            }

            for (int k = 0; k < availableTargets.Count; k++)
            {
                float distanceFromTarget = Vector3.Distance(targetTransform.position, availableTargets[k].transform.position);

                if (distanceFromTarget < shortestDistance)
                {
                    shortestDistance = distanceFromTarget;
                    nearestLockOnTarget = availableTargets[k];
                }

                if (inputHandler.lockOnFlag)
                {   //敵が右か左か確認
                    //Vector3 relativeEnemyPosition = currentLockOnTarget.transform.InverseTransformPoint(availableTargets[k].transform.position);
                    //var distanceFromLeftTarget = currentLockOnTarget.transform.position.x - availableTargets[k].transform.position.x;
                    //var distanceFromRightTarget = currentLockOnTarget.transform.position.x + availableTargets[k].transform.position.x;

                    Vector3 relativeEnemyPosition = inputHandler.transform.InverseTransformPoint(availableTargets[k].transform.position);
                    var distanceFromLeftTarget = relativeEnemyPosition.x;
                    var distanceFromRightTarget = relativeEnemyPosition.x;


                    if (relativeEnemyPosition.x <= 0.00 && distanceFromLeftTarget > shortestDistanceOfLeftTarget && availableTargets[k] != currentLockOnTarget)
                    {
                        shortestDistanceOfLeftTarget = distanceFromLeftTarget;
                        leftLockTarget = availableTargets[k];
                    }

                    else if (relativeEnemyPosition.x >= 0.00 && distanceFromRightTarget < shortestDistanceOfRightTarget && availableTargets[k] != currentLockOnTarget)
                    {
                        shortestDistanceOfRightTarget = distanceFromRightTarget;
                        rightLockTarget = availableTargets[k];
                    }
                }
            }

        }
        public void ClearLockOnTargets()
        {
            availableTargets.Clear();
            nearestLockOnTarget = null;
            currentLockOnTarget = null;
        }


        public void SetCameraHeight()
        {
            Vector3 velocity = Vector3.zero;
            Vector3 newLockedPosition = new Vector3(0, lockedPivotPosition);
            Vector3 newUnlockedPosition = new Vector3(0, unlockedPivotPosition);

            if (currentLockOnTarget != null)
            {
                cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newLockedPosition, ref velocity, Time.deltaTime);
            }
            else
            {
                cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newUnlockedPosition, ref velocity, Time.deltaTime);
            }
        }
    }
}
