using UnityEngine;
using GDG;

namespace danielnyan
{
    public class WitchExitHurt : StateMachineBehaviour
    {
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.transform.root.GetComponent<MovementController>().MovementEnabled = true;
            animator.transform.root.GetComponent<WitchMovement>().CheckGrounded();
        }
    }
}
