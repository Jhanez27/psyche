using UnityEngine;

namespace Characters.Handlers
{
    public class CharactersAnimationHandler : MonoBehaviour
    {
        private Animator myAnimator;

        private void Awake()
        {
            myAnimator = GetComponent<Animator>();
        }

        public void SetIdle()
        {
            myAnimator.SetFloat("MoveX", 0f);
            myAnimator.SetFloat("MoveY", 0f);
            myAnimator.SetFloat("MoveMagnitude", 0f);
        }

        public void SetMovement(Vector2 movement, Vector2 lastMovement)
        {
            myAnimator.SetFloat("MoveX", movement.x);
            myAnimator.SetFloat("MoveY", movement.y);
            myAnimator.SetFloat("LastMoveX", lastMovement.x);
            myAnimator.SetFloat("LastMoveY", lastMovement.y);
            myAnimator.SetFloat("MoveMagnitude", movement.sqrMagnitude);
        }
    }
}