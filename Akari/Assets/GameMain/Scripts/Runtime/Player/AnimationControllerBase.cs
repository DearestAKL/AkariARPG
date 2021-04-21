using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Akari
{
    public class AnimationControllerBase
    {
        private Animator animator;
        private bool isLocomotion = false;


        public AnimationControllerBase()
        {

        }

        public void SetAnimator(Animator animator)
        {
            this.animator = animator;
        }

        public void PlayMove(float velocity)
        {
            //if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Locomotion"))
            //{
            //    animator.Play("Locomotion");
            //}

            animator.SetFloat("Velocity", velocity);
        }

        public void PlayJump()
        {
            animator.Play("Jump");
        }

        public void PlayLightAttack(int index = 1)
        {
            animator.Play($"Attack{index}"); 
        }
        public void PlayHeavyAttack()
        {
            animator.Play("Attack5"); 
        }
    }
}
