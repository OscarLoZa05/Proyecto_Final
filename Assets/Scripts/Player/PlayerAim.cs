using UnityEngine;
using UnityEngine.InputSystem;

public class RaycastToEnemy : MonoBehaviour
{
    [SerializeField] private float _detectionRange = 10f;
    [SerializeField] private LayerMask enemyLayer;

    private InputAction _lockAction;

    [SerializeField] private Transform _enemyTarget;

    void Awake()
    {
        _lockAction = InputSystem.actions["Lock"];
        enemyLayer = LayerMask.GetMask("Enemy");
    }

    void Update()
    {
        if(_lockAction.WasPressedThisFrame())
        {
            FindClosestEnemy();
            
            if (_enemyTarget == null)
            return;

            Vector3 directionToEnemy = (_enemyTarget.position - transform.position).normalized;

            if (Physics.Raycast(transform.position, directionToEnemy, out RaycastHit hit, _detectionRange, enemyLayer))
            {
                Debug.Log("Raycast ha golpeado a: " + hit.collider.name);
            }
            }
        

        
    }

    void FindClosestEnemy()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, _detectionRange, enemyLayer);

        float minDist = float.MaxValue;
        _enemyTarget = null;

        for (int i = 0; i < enemies.Length; i++)
        {
            float dist = (enemies[i].transform.position - transform.position).sqrMagnitude;

            if (dist < minDist)
            {
                minDist = dist;
                _enemyTarget = enemies[i].transform;
            }
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