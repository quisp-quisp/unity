using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpSpeed;
    [SerializeField] private float _g = -9.81f;
    [SerializeField] private float _turnTime;
    [SerializeField] private Transform _camera;

    private CharacterController _characterController;
    private Vector3 _moveDirection;
    private Vector3 _jumpDirection;
    private float _turnSpeed;

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 direction = context.ReadValue<Vector2>();
            _moveDirection.x = direction.x;
            _moveDirection.z = direction.y;
        }
        else if (context.canceled)
        {
            _moveDirection = Vector3.zero;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //if (context.performed)
        //{
        //    if (_characterController.isGrounded)
        //    {
        //        _jumpDirection = Vector3.up * _jumpSpeed;
        //    }
        //}
    }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (_moveDirection.magnitude > 0)
        {
            Move();
        }
    }

    private void Move()
    {
        // unity angles start from forward and go clockwise
        float moveAngleRelativeCamera = Mathf.Atan2(_moveDirection.x, _moveDirection.z) * Mathf.Rad2Deg;
        moveAngleRelativeCamera += _camera.eulerAngles.y; // move relative to camera

        Vector3 moveDirectionRelativeCamera = Quaternion.Euler(0, moveAngleRelativeCamera, 0) * Vector3.forward;
        _characterController.Move(moveDirectionRelativeCamera * _moveSpeed * Time.deltaTime);

        Turn(moveAngleRelativeCamera);
    }

    private void Turn(float moveAngleRelativeCamera)
    {
        // smooth turning motion
        float faceAngleRelativeCamera = Mathf.SmoothDampAngle(transform.eulerAngles.y, moveAngleRelativeCamera, ref _turnSpeed, _turnTime);
        transform.rotation = Quaternion.Euler(0, faceAngleRelativeCamera, 0);
    }
}
