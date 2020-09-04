using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GDG;

namespace danielnyan
{
    public class WitchMovement : MonoBehaviour
    {
        [SerializeField]
        private Transform handPosition;

        private MovementController movementController;
        private WitchAnimator charAnimator;
        private Character character;

        private int isMoving; //if isMoving is -1, it is moving left, if 1, it is moving right, else it is not moving
        private bool isJumping;
        private bool isGrounded;

        [SerializeField]
        public float movementSpeed;

        [SerializeField]
        public float jumpForce;

        [SerializeField]
        private GameObject flamethrower;
        [SerializeField]
        private GameObject nuke;

        private bool isBusy = false;
        private GameObject currentAction;
        private string currentActionString;

        // Start is called before the first frame update
        private void Start()
        {
            movementController = GetComponent<MovementController>();
            charAnimator = GetComponent<WitchAnimator>();
            character = GetComponent<Character>();
            character.CharacterInjuredEvent.AddListener(GetHit);
        }

        // Update is called once per frame
        private void Update()
        {
            if (!isBusy)
            {
                if (Input.GetAxisRaw("Vertical") > 0 && isGrounded)
                {
                    isJumping = true;
                }
                if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    isMoving = 1;
                }
                else if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    isMoving = -1;
                }
                else
                {
                    isMoving = 0;
                }
            }
            if (Input.GetKeyDown(KeyCode.K) && currentAction == null && isGrounded)
            {
                movementController.Stationary();
                movementController.MovementEnabled = false;
                isBusy = true;
                charAnimator.StartThrust();
                currentAction = Instantiate(flamethrower, handPosition);
                currentActionString = "flamethrower";
                currentAction.transform.parent = null;
            }

            if (Input.GetKeyUp(KeyCode.K) && currentActionString == "flamethrower")
            {
                charAnimator.EndThrust();
                InterruptCast();
            }

            if (Input.GetKeyDown(KeyCode.N) && currentAction == null && isGrounded)
            {
                movementController.Stationary();
                movementController.MovementEnabled = false;
                isBusy = true;
                charAnimator.StartNuke();
                currentAction = Instantiate(nuke, handPosition);
                currentAction.transform.position = transform.position - new Vector3(0.1f, 0f, 0f);
                currentActionString = "nuke";
                currentAction.transform.parent = null;
            }

            if (Input.GetKeyUp(KeyCode.N) && currentActionString == "nuke")
            {
                charAnimator.EndNuke();
                InterruptCast();
            }
        }

        private void FixedUpdate()
        {
            if (isJumping)
            {
                movementController.Jump(jumpForce);
                charAnimator.Jump();
                charAnimator.Move(false);
                charAnimator.Ground(false);
                isGrounded = false;
                isJumping = false;
            }

            if (isMoving < 0)
            {
                movementController.MoveLeft(movementSpeed);
                charAnimator.faceLeft();
                if (isGrounded)
                {
                    charAnimator.Move();
                }
            }
            else if (isMoving > 0)
            {
                movementController.MoveRight(movementSpeed);
                charAnimator.faceRight();
                if (isGrounded)
                {
                    charAnimator.Move();
                }
            }
            else
            {
                movementController.Stationary();
                charAnimator.Move(false);
            }

        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            CheckGrounded();
        }

        public void CheckGrounded()
        {
            Vector2[] rays = { new Vector2(0f, 0f), new Vector2(0.5f, 0f), new Vector2(-0.5f, 0f), new Vector2(0.25f, 0f), new Vector2(-0.25f, 0f) };
            Collider2D collider2d = GetComponent<Collider2D>();
            float distance = collider2d.bounds.extents.y + 0.1f;
            float rayLength = (new Vector2(-0.5f, distance)).magnitude;
            int hits = 0;

            foreach (Vector2 displace in rays)
            {
                RaycastHit2D[] hitArray = new RaycastHit2D[3];
                collider2d.Raycast(new Vector2(0f, -distance) + displace, hitArray, rayLength, 1 << LayerMask.NameToLayer("Ground"));

                if (hitArray[0].collider != null && hitArray[0].collider.tag == "Ground")
                {
                    hits++;
                }
            }
            if (hits > rays.Length / 2)
            {
                if (!isGrounded)
                {
                    charAnimator.Ground();
                }
                isGrounded = true;
            }
        }

        private void GetHit(Character character, int i)
        {
            if (currentAction != null)
            {
                InterruptCast();
            }
        }

        private void InterruptCast()
        {
            if (currentActionString == "nuke")
            {
                charAnimator.EndNuke();
            }
            if (currentAction != null)
            {
                currentAction.GetComponent<ContinuousProjectile>().KillProjectile();
            }
            currentAction = null;
            currentActionString = "";
            isBusy = false;
            movementController.MovementEnabled = true;
        }
    }
}
