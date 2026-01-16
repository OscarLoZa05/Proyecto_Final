using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class PlayerAim : MonoBehaviour
{
    private InputAction _lockAction;

    private CinemachineCamera freeLook;
    private Transform lookAtPivot;

    [SerializeField] private float _detectionRange = 25f;
    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private float lookAheadDistance = 2.5f;
    [SerializeField] private float lookHeight = 1.6f;

    private Transform _enemyTarget;

    public float distanceOfEnemyLocked;

    void Awake()
    {
        _lockAction = InputSystem.actions["Lock"];

        freeLook = Object.FindFirstObjectByType<CinemachineCamera>();

        lookAtPivot = new GameObject("LookAtPivot").transform;

        enemyLayer = LayerMask.GetMask("Enemy");
    }

    void Update()
    {
        if (_lockAction.WasPressedThisFrame())
        {
            FindClosestEnemy();
        }

        if (_enemyTarget != null)
        {
            Vector3 dir = (_enemyTarget.position - transform.position).normalized;

            lookAtPivot.position =
                transform.position +
                dir * lookAheadDistance +
                Vector3.up * lookHeight;

            distanceOfEnemyLocked =
                (_enemyTarget.position - transform.position).sqrMagnitude;

            if (distanceOfEnemyLocked > _detectionRange * _detectionRange)
            {
                freeLook.LookAt = null;
                _enemyTarget = null;
            }
        }
    }

    void FindClosestEnemy()
    {
        Collider[] enemies =
            Physics.OverlapSphere(transform.position, _detectionRange, enemyLayer);

        float minDist = float.MaxValue;
        _enemyTarget = null;

        foreach (var enemy in enemies)
        {
            float dist =
                (enemy.transform.position - transform.position).sqrMagnitude;

            if (dist < minDist)
            {
                minDist = dist;
                _enemyTarget = enemy.transform;
            }
        }

        if (_enemyTarget != null)
        {
            freeLook.LookAt = lookAtPivot;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _detectionRange);

        if (_enemyTarget != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, _enemyTarget.position);
        }
    }
}
