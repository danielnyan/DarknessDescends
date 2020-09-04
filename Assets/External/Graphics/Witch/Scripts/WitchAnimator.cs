using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace danielnyan
{
    public class WitchAnimator : CharacterAnimator
    {
        private Coroutine currentRoutine = null;

        public void StartThrust()
        {
            animator.SetTrigger("Thrust Forward");
        }

        public void EndThrust()
        {
            animator.SetTrigger("End Thrust");
        }

        public void StartNuke()
        {
            currentRoutine = StartCoroutine(NukeSequence());
        }

        public void EndNuke()
        {
            StopCoroutine(currentRoutine);
            animator.SetBool("Pray", false);
        }

        private IEnumerator NukeSequence()
        {
            animator.SetBool("Pray", true);
            yield return new WaitForSeconds(3f);
            animator.SetBool("Pray", false);
            animator.SetTrigger("Swipe Up");
        }
    }
}
