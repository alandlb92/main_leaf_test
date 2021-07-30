using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 25;
    [SerializeField] private float sensitivity = 50;

    private Rigidbody _rigidBody;
    private Transform _cameraTransform;

    private float _headRotation = 0f;
    private float _maxVelocity = 1;

    private const float MIN_HEAD_ROTATION = -90;
    private const float MAX_HEAD_ROTATION = 90;

    public float Velocity
    {
        get
        {
            Vector2 velocityXy = new Vector2(_rigidBody.velocity.x, _rigidBody.velocity.z);
            return velocityXy.magnitude / _maxVelocity;
        }
    }
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _cameraTransform = Camera.main.transform;
        _maxVelocity = 2 * movementSpeed;
    }

    private void Start()
    {
        _headRotation = 0;
    }

    private void Update()
    {
        WalkMovement();
        RotateAroundMovement();
    }

    private void RotateAroundMovement()
    {
        float mouseMovementX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseMovementY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime * -1f;
        transform.Rotate(0f, mouseMovementX, 0f);
        _headRotation += mouseMovementY;
        _headRotation = Mathf.Clamp(_headRotation, MIN_HEAD_ROTATION, MAX_HEAD_ROTATION);
        _cameraTransform.localEulerAngles = new Vector3(_headRotation, 0f, 0f);
    }

    private void WalkMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        float velocityMultiply = Input.GetAxisRaw("Run") + 1;
        Vector3 moveBy = transform.right * horizontal + transform.forward * vertical;
        Vector3 velocity = moveBy.normalized * movementSpeed * velocityMultiply;
        velocity.y = _rigidBody.velocity.y;
        _rigidBody.velocity = velocity;
    }
}
