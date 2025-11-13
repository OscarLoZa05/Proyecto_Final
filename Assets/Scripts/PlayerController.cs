using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    //Componentes
    private CharacterController _controller;
    //private Animator _animator;
    private Transform _mainCamera;

    //Inputs
    public Vector2 _moveValue;
    private InputAction _moveAction;
    private InputAction _jumpAction;
    
    //Movimiento 
    private float _playerSpeed = 5;
    private float _playerForce = 2;
    private float _pushForce = 10;
    private float _smoothTime = 0.2f;
    private float _turnSmoothVelocity;

    //Suelo
    [Header("Ground")]
    public Transform _sensor;
    public LayerMask _groundLayer;
    public float _sensorRadius;

    //Gravedad
    private float _gravity = -9.81f;
    public Vector3 _playerGravity;

    
    

    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        //_animator = GetComponentInChildren<Animator>();

        _moveAction = InputSystem.actions["Move"];
        _jumpAction = InputSystem.actions["Jump"];

        _mainCamera = Camera.main.transform;
    }
    void Update()
    {
        _moveValue = _moveAction.ReadValue<Vector2>();

        if (_jumpAction.WasPerformedThisFrame() && IsGrounded())
        {
            Jump();
        }

        Movement();

        Gravity();
    }

    void Movement()
    {
      Vector3 direction = new Vector3(_moveValue.x, 0, _moveValue.y);

        /*_animator.SetFloat("Vertical", direction.magnitude);
        _animator.SetFloat("Horizontal", 0);*/

        if (direction != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _mainCamera.eulerAngles.y;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _smoothTime);
            transform.rotation = Quaternion.Euler(0, smoothAngle, 0);

            Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

            _controller.Move(moveDirection.normalized * _playerSpeed * Time.deltaTime);
        }   
    }

    void Jump()
    {

        //_animator.SetBool("IsJumping", true);
        _playerGravity.y = Mathf.Sqrt(_playerForce * -2 * _gravity);

        _controller.Move(_playerGravity * Time.deltaTime);
    }
    
    void Gravity()
    {

        if (!IsGrounded())
        {
            _playerGravity.y += _gravity * Time.deltaTime;
        }
        else if (IsGrounded() && _playerGravity.y < 0)
        {
            //_animator.SetBool("IsJumping", false);
            _playerGravity.y = -9.81f;
        }

        _controller.Move(_playerGravity * Time.deltaTime);
    }

    bool IsGrounded()
    {
        return Physics.CheckSphere(_sensor.position, _sensorRadius, _groundLayer);
    }

    void Hola()
    {
        Collider[] objectsToGrab = Physics.OverlapBox(_hands.position, _handsSensorSize);
        foreach (Collider item in objectsToGrab)
            {
                IIteratable iteratableObject = item.GetComponent<IGrabeable>();

                if (grabeableObject != null)
                {
                    _grabedObject = item.transform;
                    _grabedObject.SetParent(_hands);
                    _grabedObject.position = _hands.position;
                    _grabedObject.rotation = _hands.rotation;
                    _grabedObject.GetComponent<Rigidbody>().isKinematic = true;

                    return;
                }
            }
    }

    void OnDrawGizmos()
    {
       Gizmos.color = Color.white;
       Gizmos.DrawWireSphere(_sensor.position, _sensorRadius); 
    }
}
