using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class PlayerAbility : MonoBehaviour
{

    //Inputs
    private InputAction _ability1;
    private InputAction _ability2;
    private InputAction _ability3;

    //Canvas
    [Header("Canvas")]
    public Image imageAbility1;
    public Image imageAbility2;
    public Image imageAbility3;


    //WaterAbilities
    [Header("Ability1")]
    public float cooldownAbility1 = 15;
    public float currentCooldown1 = 15;
    public bool ability1Used = false;
    private float maxDistance = 10;
    private float _playerForceImpulse = 20;

    [Header("Ability2")]
    public float cooldownAbility2 = 10;
    public float currentCooldown2 = 10;
    public bool ability2Used = false;

    //FireAbilities
    [Header("Ability3")]
    public float cooldownAbility3 = 10;
    public float currentCooldown3 = 10;
    public bool ability3Used = false;


    PlayerController _playerController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _playerController = GetComponent<PlayerController>();

        _ability1 = InputSystem.actions["WaterAttack"];
        _ability2 = InputSystem.actions["WaterState"];
        _ability3 = InputSystem.actions["FireState"];
    }

    // Update is called once per frame
    void Update()
    {

        //fireAttackRange  = transform.rotation * rangeAttack;

        if(_ability1.WasPressedThisFrame() && ability1Used == false && _playerController.manaBar >= 15)
        {
            WaveAttack();
            ManaUsed(15);
            ability1Used = true;
            currentCooldown1 = 0;
            imageAbility1.fillAmount = 0;
        }
        if(_ability2.WasPressedThisFrame() && ability2Used == false && _playerController.manaBar >= 20)
        {
            StartCoroutine(WaterState());
            ManaUsed(25);
            ability2Used = true;
            currentCooldown2 = 0;
            imageAbility2.fillAmount = 0;
        }
        if(_ability3.WasPressedThisFrame() && ability3Used == false && _playerController.manaBar >= 50)
        {
            StartCoroutine(FireState());
            ManaUsed(50);
            ability3Used = true;
            currentCooldown3 = 0;
            imageAbility3.fillAmount = 0;
        }

        if(ability1Used)
        {
            currentCooldown1 += Time.deltaTime;

            float progressAbility1 = currentCooldown1 / cooldownAbility1;

            imageAbility1.fillAmount = progressAbility1;

            if(currentCooldown1 >= cooldownAbility1)
            {
                ability1Used = false;
            }
        }
        if(ability2Used)
        {
            currentCooldown2 += Time.deltaTime;

            float progressAbility2 = currentCooldown2 / cooldownAbility2;

            imageAbility2.fillAmount = progressAbility2;

            if(currentCooldown2 >= cooldownAbility2)
            {
                ability2Used = false;
            }
        }
        if(ability3Used)
        {
            currentCooldown3 += Time.deltaTime;

            float progressAbility3 = currentCooldown3 / cooldownAbility3;

            imageAbility3.fillAmount = progressAbility3;

            if(currentCooldown3 >= cooldownAbility3)
            {
                ability3Used = false;
            }
        }
    }

    void WaveAttack()
    {
        Debug.Log("Habilidad 1 Usada");
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
    }

    IEnumerator WaterState()
    {
        Debug.Log("Habilidad 2 Usada");
        _playerController._playerSpeed = 20;
        yield return new WaitForSeconds(5);
        _playerController._playerSpeed = 5;
    }

    /*void FireAttack()
    {

        
        Collider[] enemies = Physics.OverlapBox(fireOrigin.transform.position, fireAttackRange); 
            foreach (Collider enemy in enemies)
            {
                if(enemy.transform.gameObject.layer == 7)
                {
                    Debug.Log("Le he dado");
                }
            }
    }*/

    IEnumerator FireState()
    {
        for (int i = 0; i < 5; i++)
        {
            _playerController.currenthealthBar += 10;
            _playerController.currenthealthBar = Mathf.Clamp(_playerController.currenthealthBar, 0, _playerController.maxHealthBar);
            yield return new WaitForSeconds(2);
            
        }
    }

    void ManaUsed(int ManaWasted)
    {
        _playerController.manaBar -= ManaWasted;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }
}
