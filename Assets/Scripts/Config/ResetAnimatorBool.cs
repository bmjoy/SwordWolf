using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAnimatorBool : StateMachineBehaviour
{

    [SerializeField] private string isInvulnerable = "isInvulnerable";
    [SerializeField] private bool isInvulnerableStatus = false;

    [SerializeField] private string isInteractingBool = "isInteracting";
    [SerializeField] private bool isInteractingStatus = false;

    [SerializeField] private string isFiringSpellBool = "isFiringSpell";
    [SerializeField] private bool isFiringSpellStatus = false;

    [SerializeField] private string isRotatingWithRootMotion = "isRotatingWithRootMotion";
    [SerializeField] private bool isRotatingWithRootMotionStatus = false;

    [SerializeField] private string canRotateBool = "canRotate";
    [SerializeField] private bool canRotateStatus = true;


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(isInteractingBool, isInteractingStatus);
        animator.SetBool(isFiringSpellBool, isFiringSpellStatus);
        animator.SetBool(isRotatingWithRootMotion, isRotatingWithRootMotionStatus);
        animator.SetBool(canRotateBool, canRotateStatus);
        animator.SetBool(isInvulnerable, isInvulnerableStatus);
    }
}
