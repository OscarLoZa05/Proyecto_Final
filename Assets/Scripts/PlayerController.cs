using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    //Componentes
    private CharacterController _controller;
    private Animator _animator;
    private Transform _mainCamera;

    //Inputs
    public Vector2 _moveValue;
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _interactAction;
    private InputAction _healthPotionAction;
    private InputAction _manaPotionAction;
    private InputAction _dashAction;
    
    //Movimiento 
    private float _playerSpeed = 5;
    private float _playerForce = 2;
    private float _pushForce = 10;
    private float _smoothTime = 0.2f;
    private float _turnSmoothVelocity;

    //Suelo
    [Header("Ground")]
    public Transform _sensorPosition;
    public LayerMask _groundLayer;
    public float _sensorRadius;

    //Gravedad
    [Header("Gravity")]
    public Vector3 _playerGravity;
    private float _gravity = -9.81f;

    //Interact
    [Header("Interact")]
    public Transform _interactionPosition;
    public Vector3 _interactionRadius;

    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        //_animator = GetComponentInChildren<Animator>();

        _moveAction = InputSystem.actions["Move"];
        _jumpAction = InputSystem.actions["Jump"];
        _interactAction = InputSystem.actions["Interact"];
        _healthPotionAction = InputSystem.actions["Potions"];
        _manaPotionAction = InputSystem.actions["Potions"];
        _dashAction = InputSystem.actions["Dash"];

        _mainCamera = Camera.main.transform;
    }
    void Update()
    {
        _moveValue = _moveAction.ReadValue<Vector2>();

        //Acciones
        if (_jumpAction.WasPerformedThisFrame() && IsGrounded())
        {
            Jump();
        }
        if (_interactAction.WasPressedThisFrame())
        {
            Interact();
        }
        if(_dashAction.WasPressedThisFrame())
        {
            Dash();
        }

        //Pociones
        if(_healthPotionAction.WasPressedThisFrame() && GameManager.instance.healthPotion > 0)
        {
            GameManager.instance.RestHealthPotion();
            Debug.Log(GameManager.instance.healthPotion);    
        }
        if(_manaPotionAction.WasPressedThisFrame() && GameManager.instance.manaPotion > 0)
        {
            GameManager.instance.RestManaPotion();
            Debug.Log(GameManager.instance.manaPotion);    
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
        return Physics.CheckSphere(_sensorPosition.position, _sensorRadius, _groundLayer);
    }

    void Interact()
    {
        Collider[] objectsToGrab = Physics.OverlapBox(_interactionPosition.position, _interactionRadius);
            foreach (Collider item in objectsToGrab)
            {
                if(item.gameObject.layer == LayerMask.NameToLayer("Interactable"))
                {
                    IInteractable interactableObject = item.GetComponent<IInteractable>();
                    if(interactableObject != null)
                    {
                        interactableObject.Interact(); 
                    }
                }
            }
    }

    void Dash()
    {
        _controller.Move(Vector3.forward);    
    }

    /*IEnumerator Dash()
    {
        isDashing = true;
        float startTime = Time.time;

        while (Time.time < startTime + _dashTime)
        {
            //Temporalmente para sacar el movimiento
            //Vector3 direction = new Vector3(_moveValue.x, 0, _moveValue.y);
            _characterController.Move(_lastMoveDirection * _dashSpeed * Time.deltaTime);
            yield return null;
        }

        isDashing = false;
    }*/

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_sensorPosition.position, _sensorRadius);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(_interactionPosition.position, _interactionRadius);
    }
}
