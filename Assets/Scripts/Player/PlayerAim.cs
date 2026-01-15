using UnityEngine;
using UnityEngine.InputSystem;

public class RaycastToEnemy : MonoBehaviour
{
    [SerializeField] private float range = 10f;
    [SerializeField] private LayerMask enemyLayer;

    private InputAction Fijar;

    private Transform target;

    void Awake()
    {
        Fijar = InputSystem.actions["Lock"];
    }

    void Update()
    {
        if(Fijar.WasPressedThisFrame())
        {
            FindClosestEnemy();
            
            if (target == null)
            return;

            Vector3 direction = (target.position - transform.position).normalized;

            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, range, enemyLayer))
            {
                Debug.Log("Raycast ha golpeado a: " + hit.collider.name);
            }
            }
        

        
    }

    void FindClosestEnemy()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, range, enemyLayer);

        float minDist = float.MaxValue;
        target = null;

        for (int i = 0; i < enemies.Length; i++)
        {
            float dist = (enemies[i].transform.position - transform.position).sqrMagnitude;

            if (dist < minDist)
            {
                minDist = dist;
                target = enemies[i].transform;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);

        if (target != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, target.position);
        }
    }
}