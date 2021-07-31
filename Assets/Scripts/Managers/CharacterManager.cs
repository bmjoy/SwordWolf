using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asakuma
{
    public class CharacterManager : MonoBehaviour
    {
        [Header("Lock On Transform")]
        public Transform lockOnTransform;

        [Header("Combat Colliders")]
        public CriticalDamageCollider backStabCollider;
        public CriticalDamageCollider riposteCollider;

        [Header("Combat Flags")]
        public bool canBeRiposted;
        public bool canBeParried;
        public bool isParrying;
        public bool isBlocking;
        public bool isInvulnerable;

        [Header("Movement Flags")]
        public bool isRotatingWithRootMotion;
        public bool canRotate;

        [Header("Spells")]
        public bool isFiringSpell;

        //  damage will be inflicted during an animation event
        //  using in backstab or riposte animation
        public int pendingCriticalDamage;   

    }
}