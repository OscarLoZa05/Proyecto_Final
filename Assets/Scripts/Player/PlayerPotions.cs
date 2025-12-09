using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPotions : MonoBehaviour
{

    //Inputs
    private InputAction healthPotionInput;
    private InputAction manaPotionInput;

    //Component
    PlayerController _playerController;

    //Potions
    [Header("Potions")]
    public int manaPotions = 0;
    public int healthPotions = 0;

    void Awake()
    {
        _playerController = GetComponent<PlayerController>();

        healthPotionInput = InputSystem.actions["PotionsHealth"];
        manaPotionInput = InputSystem.actions["PotionsMana"];
    }

    // Update is called once per frame
    void Update()
    {
        if(manaPotionInput.WasPressedThisFrame() && manaPotions >= 1)
        {
            ManaPotion(-1);
        }
        if(healthPotionInput.WasPressedThisFrame() && healthPotions >= 1)
        {
            HealthPotion(-1);
        }
    }

    public void ManaPotion(int Count)
    {
        if(Count < 0)
        {
            manaPotions += Count;
            _playerController.manaBar += 30;
        }
        if(Count > 0)
        {
            manaPotions += Count;
        }
        
        Debug.Log(manaPotions);
    }
    public void HealthPotion(int Count)
    {
        if(Count < 0)
        {
            healthPotions += Count;
            _playerController.currenthealthBar += 50;
        }
        if(Count > 0)
        {
            healthPotions += Count;
        }
        
        Debug.Log(healthPotions);
    }
}
