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
    //private InputAction _dashAction;
    
    //Movimiento 
    public float _playerSpeed = 5;
    private float _playerForce = 2;
    private float _pushForce = 10;
    private float _smoothTime = 0.2f;
    private float _turnSmoothVelocity;
    public float _speed;
    public float _speedChangeRate = 10;
    public float targetAngle;

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

    //Jump
    [Header("Jump")]
    public float jumpTimeOut = 0.5f;
    public float fallTimeOut = 0.15f; 
    public float _jumpHeight = 2f;
    
    float _jumpTimeOutDelta;
    float _fallTimeOutDelta;

    [Header("Bars")]
    public float currentManaBar = 100;
    public float maxManaBar = 100;
    public float maxHealthBar = 100;
    public float currentHealthBar = 50;

    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        //_animator = GetComponentInChildren<Animator>();

        _moveAction = InputSystem.actions["Move"];
        _jumpAction = InputSystem.actions["Jump"];
        _interactAction = InputSystem.actions["Interact"];
        //_dashAction = InputSystem.actions["Dash"];

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
        /*if(_dashAction.WasPressedThisFrame())
        {
            Dash();
        }*/
        /*if(_ability1.WasPressedThisFrame())
        {
            WaveAbility();
        }
        if(_ability2.WasPressedThisFrame())
        {
            StartCoroutine(ScaleAbility());
        }*/

        

        Movement();

        Gravity();
    }

    private float _smoothSpeed = 0f;
    void Movement()
    {
        Vector3 direction = new Vector3(_moveValue.x, 0, _moveValue.y);



        float targetSpeed = _playerSpeed;
        
        if(direction == Vector3.zero)
        {
            targetSpeed = 0;
        }

        

        /*float currentSpeed = new Vector3(_controller.velocity.x, 0, _controller.velocity.z).magnitude;
        float speedOffset = 0.1f;

        if(currentSpeed < targetSpeed - speedOffset || currentSpeed > targetSpeed + speedOffset)
        {
            _speed = Mathf.Lerp(currentSpeed, targetSpeed * direction.magnitude, Time.deltaTime * _speedChangeRate);
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = targetSpeed;
        }*/
        _speed = Mathf.SmoothDamp(_speed, targetSpeed * direction.magnitude, ref _smoothSpeed, 0.1f);

        if (direction != Vector3.zero)
        {
            targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _mainCamera.eulerAngles.y;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _smoothTime);
            transform.rotation = Quaternion.Euler(0, smoothAngle, 0);
        }

        Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
        _controller.Move(_speed * Time.deltaTime * moveDirection.normalized  + _playerGravity * Time.deltaTime);
        
    }

        void Jump()
    {
        if(_jumpTimeOutDelta <= 0)
        {
           // _animator.SetBool("Jump", true);
            _playerGravity.y = Mathf.Sqrt(_jumpHeight * -2 * _gravity);
        }
        //_controller.Move(_playerGravity * Time.deltaTime);
    }

    void Gravity()
    {
        //_animator.SetBool("Grounded", IsGrounded());

        if(IsGrounded())
        {
            _fallTimeOutDelta = fallTimeOut;
            
            //_animator.SetBool("Jump", false);
            //_animator.SetBool("Fall", false);
            if(_playerGravity.y < 0)
            {
                _playerGravity.y = -2;
            }

            if(_jumpTimeOutDelta >= 0)
            {
                _jumpTimeOutDelta -= Time.deltaTime;
            }
        }
        else
        {
            _jumpTimeOutDelta = jumpTimeOut;

            if(_fallTimeOutDelta >= 0)
            {
                _fallTimeOutDelta -= Time.deltaTime;
            }
            else
            {
                //_animator.SetBool("Fall", true);
            }
            
            _playerGravity.y += _gravity * Time.deltaTime;
        }
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
                if(item.gameObject.layer == 7)
                {
                    IInteractable interactableObject = item.GetComponent<IInteractable>();
                    if(interactableObject != null)
                    {
                        interactableObject.Interact(); 
                    }
                }
            }
    }

    private float maxDistance = 20;
    private float _playerForceImpulse = 30;
    //public Transform enemigos;

    /*void WaveAbility()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, maxDistance);
            foreach (Collider enemy in enemies)
            {
                if(enemy.transform.gameObject.layer == 7)
                {
                    Rigidbody _enemyRigidBody = enemy.GetComponent<Rigidbody>();

                    Vector3 directionToEnemy = (enemy.transform.position - transform.position).normalized;

                    float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                    float forceMultiplier = Mathf.Clamp01(1 - (distanceToEnemy / maxDistance));

                    //float distanceNeeded = maxDistance - distanceToEnemy;
                    //float impulseNeeded = (distanceNeeded)
                    directionToEnemy.y = 0;

                    Vector3 force = (directionToEnemy * forceMultiplier);

                    _enemyRigidBody.AddForce(0, 2, 0, ForceMode.Impulse);
                    _enemyRigidBody.AddForce(force * _playerForceImpulse, ForceMode.Impulse);
                    

                    Debug.Log(force);
                }
            }
    }*/

    /*IEnumerator ScaleAbility()
    {
        Debug.Log("HOla");
        _playerSpeed = 20;
        yield return new WaitForSeconds(5);
        _playerSpeed = 5;
    }*/

    /*void Dash()
    {
        _controller.Move(Vector3.forward);    
    }*/

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

    /*void Dash()
    {
        _controller.Move()
    }*/







    //void TakeDamage(int Damage)
    /*{
        healthBar =- Damage;
    }*/

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_sensorPosition.position, _sensorRadius);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(_interactionPosition.position, _interactionRadius);
    }
}
