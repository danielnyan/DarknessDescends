using UnityEngine;

namespace danielnyan
{
    public class WitchDuringJump : StateMachineBehaviour
    {
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.transform.root.GetComponent<WitchMovement>().CheckGrounded();
        }
    }
}
