using System.Collections;
using UnityEngine;

namespace Characters.Handlers
{
    public class CharactersMovementHandler : MonoBehaviour
    {
        [Header("Character Physics")]
        [SerializeField] private float moveSpeed = 1f;
        [SerializeField] private float dashSpeed = 4f;

        [Header("Character Components")]
        [SerializeField] private TrailRenderer myTrailRenderer;

        public Vector2 Movement { get; private set; }
        public Vector2 LastMovement { get; private set; }
        private Rigidbody2D rb;

        private float startingMovementSpeed;
        private bool isDashing = false;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            startingMovementSpeed = moveSpeed;
            Movement = LastMovement = Vector2.zero;
        }

        public void SetMovement(Vector2 input)
        {
            Movement = input.normalized;

            if (Movement.sqrMagnitude > 0)
            {
                LastMovement = Movement;
            }
        }

        public void Move()
        {
            rb.MovePosition(rb.position + Movement * (moveSpeed * Time.fixedDeltaTime));
        }

        public void Dash()
        {
            if (isDashing) return;
            Debug.Log("Yes");


            isDashing = true;
            moveSpeed *= dashSpeed;

            if (myTrailRenderer != null)
            {
                myTrailRenderer.emitting = true;
            }

            StartCoroutine(EndDashRoutine());
        }

        private IEnumerator EndDashRoutine()
        {
            float dashTime = .2f;
            float dashCD = 0.25f;
            yield return new WaitForSeconds(dashTime);
            moveSpeed = startingMovementSpeed;
            myTrailRenderer.emitting = false;
            yield return new WaitForSeconds(dashCD);
            isDashing = false;
        }
    }
}