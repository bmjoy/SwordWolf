using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asakuma
{
    [CreateAssetMenu(menuName = "A.I/Enemy Actions/attack Action")]
    public class EnemyAttackAction : EnemyAction
    {
        [SerializeField] private bool canCombo;

        public EnemyAttackAction comboAction;
        public int attackScore { get; private set; } = 3;
        public float recoveryTime { get; private set; } = 2f;

        public float maximumAttackAngle { get; private set; } = 35f;
        public float minimumAttackAngle { get; private set; } = -35f;

        public float minimumDistanceNeededToAttack { get; private set; } = 0f;
        public float maximumDistanceNeededToAttack { get; private set; } = 3f;
    }
}