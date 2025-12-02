using UnityEngine;
using UnityEngine.InputSystem;

public class PotionPlayer : MonoBehaviour
{

    private InputAction _healthPotionAction;
    private InputAction _manaPotionAction;
    
    void Awake()
    {
        _healthPotionAction = InputSystem.actions["PotionsHealth"];
        _manaPotionAction = InputSystem.actions["PotionsMana"];
    }

    // Update is called once per frame
    void Update()
    {
        //Pociones
        if(_healthPotionAction.WasPressedThisFrame() && GameManager.instance.healthPotion > 0)
        {
            GameManager.instance.HealthPotion(-1);
            Debug.Log(GameManager.instance.healthPotion + ": HealthPotion");    
        }
        if(_manaPotionAction.WasPressedThisFrame() && GameManager.instance.manaPotion > 0)
        {
            GameManager.instance.ManaPotion(-1);
            Debug.Log(GameManager.instance.manaPotion + ": ManaPotion");    
        }
    }
}
