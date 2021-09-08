using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asakuma
{
    public class EnemyLocomotionManager : MonoBehaviour
    {
        EnemyManager enemyManager;
        EnemyAnimatorManager enemyAnimatorManager;

        [SerializeField] private CapsuleCollider characterCollider;
        [SerializeField] private CapsuleCollider characterCollisionBlockerCollider;

        [SerializeField] private LayerMask detectionLayer;


        private void Awake()
        {
            enemyManager = GetComponent<EnemyManager>();
            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        }


        private void Start()
        {
            Physics.IgnoreCollision(characterCollider, characterCollisionBlockerCollider, true);

        }
    }
}