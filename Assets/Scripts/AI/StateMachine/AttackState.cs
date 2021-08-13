using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asakuma
{
    public class AttackState : State
    {
        public RotateTowardsTargetState rotateTowardsTargetState;
        public CombatStanceState combatStanceState;
        public PursueTargetState pursueTargetState;
        public EnemyAttackAction currentAttack;

        bool willDoComboOnNextAttack = false;
        public bool hasPerformedAttack = false;

        //  攻撃スコアに基づいて、多数の攻撃の中から1つを選択する
        //  選んだ攻撃が、角度や距離が悪くて使えない場合は、新たな攻撃を選ぶ
        //  攻撃が可能な場合、動きを止めてターゲットを攻撃する
        //  攻撃の回復時間に合わせて、回復タイマーを設定する
        //  戦闘態勢の状態を返す
        

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

            RotateTowardsTargetWhilstAttacking(enemyManager);

            if (distanceFromTarget > enemyManager.maximumAggroRadius)
            {
                return pursueTargetState;
            }

            if (willDoComboOnNextAttack && enemyManager.canDoCombo)
            {
                AttackTargetWithCombo(enemyAnimatorManager, enemyManager);
            }

            if (!hasPerformedAttack)
            {
                AttackTarget(enemyAnimatorManager, enemyManager);
                RollForComboChance(enemyManager);
            }

            if (willDoComboOnNextAttack && hasPerformedAttack)
            {
                return this; //  コンボを決めるために上に戻る
            }

            return rotateTowardsTargetState;    
        }


        private void AttackTarget(EnemyAnimatorManager enemyAnimatorManager, EnemyManager enemyManager)
        {
            enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
            enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
            hasPerformedAttack = true;
        }

        private void AttackTargetWithCombo(EnemyAnimatorManager enemyAnimatorManager, EnemyManager enemyManager)
        {
            willDoComboOnNextAttack = false;
            enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
            enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
            currentAttack = null;
        }

        private void RotateTowardsTargetWhilstAttacking(EnemyManager enemyManager)
        {
            if (enemyManager.canRotate && enemyManager.isInteracting)    //! rotate manually
            {
                Vector3 direction = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
            }
        }


        private void RollForComboChance(EnemyManager enemyManager)
        {
            float comboChance = Random.Range(0, 100);

            //  comboLikelyHoodの値が大きいほどコンボしやすい
            if (enemyManager.allowAIToPerformCombos && comboChance <= enemyManager.comboLikelyHood)
            {
                if (currentAttack.comboAction != null)
                {
                    willDoComboOnNextAttack = true;
                    currentAttack = currentAttack.comboAction;
                }
                else
                {
                    willDoComboOnNextAttack = false;
                    currentAttack = null;
                }
            }
        }

    }
}