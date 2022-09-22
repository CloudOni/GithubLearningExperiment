using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // TODO: Add acceleration & friction to player :)
        // TODO: Add air & ground friction :)
        // TODO: Add CoyoteTime :)
        // TODO: Add double jump :)
        // TODO: Add max vertical velocity 
        // TODO: Add variable jump height

        [Header("Movement")] public float maxMoveSpeedX = 6f;
        public float maxVelocityY = 16f;
        public float acceleration = 1f;
        public float groundFriction = 0.3f;
        public float airFriction = 0.005f;
        private Vector2 _currentVelocity;
        private float _moveSpeed;

        [Header("Jumping")] 
        public float jumpSpeed = 10f;
        public int maxDoubleJumpValue = 1;
        public int doubleJumpValue;
        public float coyoteTime = 0.15f;
        public float jumpTimeCounter;
        public float jumpTime = 0.25f;
        private bool _isCoyoteTime;
        private float _coyoteTimeCounter;
        private bool _isJumping;

        [Header("Component")] private PlayerInput _input;
        private PlayerCollision _collision;
        private Rigidbody2D _rigidbody2D;

        // Start is called before the first frame update
        private void Start()
        {
            _input = GetComponent<PlayerInput>();
            _collision = GetComponent<PlayerCollision>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        private void Update()
        {
            UpdateJumping();
        }

        private void FixedUpdate()
        {
            UpdateMovement();
            if (_collision.IsGroundedBox() && _rigidbody2D.velocity.y < 0f)
            {
                _isJumping = false;
                doubleJumpValue = maxDoubleJumpValue;
            }
        }

        private void UpdateJumping()
        {
            VariableJumpHeight();
            // TODO: Add fine tuned apex gravity control
            if (!_isJumping && !_collision.IsGroundedBox())
            {
                _coyoteTimeCounter += Time.deltaTime;
                _isCoyoteTime = true;
            }
            else
            {
                _coyoteTimeCounter = 0;
                _isCoyoteTime = false;
            }

            if (_input.JumpPressed &&
                (_collision.IsGroundedBox() || (_coyoteTimeCounter > 0.03f && _coyoteTimeCounter < coyoteTime)))
            {
                _rigidbody2D.velocity = Vector2.up * jumpSpeed;
                jumpTimeCounter = jumpTime;
                _isJumping = true;
            }
            else if (_input.JumpPressed && doubleJumpValue > 0)
            {
                _rigidbody2D.velocity = Vector2.up * jumpSpeed;
                _isJumping = true;
                doubleJumpValue--;
                /*
                 
                 */
            }
        }

        private void UpdateMovement()
        {
            // Store Rigidbody2D in _velocity
            _currentVelocity = _rigidbody2D.velocity;
            _currentVelocity.y = Mathf.Clamp(_currentVelocity.y, -maxVelocityY, maxVelocityY);

            // Change the Velocity
            if (_input.MoveVector.x != 0)
            {
                _moveSpeed += _input.MoveVector.x * acceleration;
                _moveSpeed = Mathf.Clamp(_moveSpeed, -maxMoveSpeedX, maxMoveSpeedX);
            }
            else
            {
                // LERP: Linear Interpolation: Variable from A to B over T(time)
                //                                                      If onGround, Set Friction to GroundFriction
                //                                                      If !onGround, Set Friction to AirFriction
                _moveSpeed = Mathf.Lerp(_moveSpeed, 0f, _collision.IsGroundedBox() ? groundFriction : airFriction);
            }

            _currentVelocity.x = _moveSpeed;

            // Return current Velocity into Rigidbody2D.velocity
            _rigidbody2D.velocity = _currentVelocity;
        }

        private void VariableJumpHeight()
        {
            if (_input.JumpValue > 0f && _isJumping)
            {
                if (jumpTimeCounter > 0)
                {
                    _rigidbody2D.velocity = Vector2.up * jumpSpeed;
                    jumpTimeCounter -= Time.deltaTime;
                }
            }
        }
}
