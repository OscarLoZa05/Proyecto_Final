using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerPotions : MonoBehaviour
{

    //Inputs
    private InputAction healthPotionInput;
    private InputAction manaPotionInput;

    //Canvas
    [Header("TextsPotions")]
    public Text manaText;
    public Text healthText;

    //Component
    PlayerController _playerController;
    PlayerAbility _playerAbil;

    //Potions
    [Header("Potions")]
    public int manaPotions = 0;
    public int healthPotions = 0;

    void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _playerAbil = GetComponent<PlayerAbility>();

        healthPotionInput = InputSystem.actions["PotionsHealth"];
        manaPotionInput = InputSystem.actions["PotionsMana"];
    }

    void Start()
    {
        manaText.text = "x 0";
        healthText.text = "x 0";
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
            manaText.text = "x " + manaPotions.ToString();
            _playerController.currentManaBar += 30;
            _playerController.currentManaBar = Mathf.Clamp(_playerController.currentManaBar, 0, _playerController.maxManaBar);
            _playerAbil.UpdateManaBar();
            
        }
        if(Count > 0)
        {
            manaPotions += Count;
            manaText.text = "x " + manaPotions.ToString();
        }
        
        Debug.Log(manaPotions);
    }
    public void HealthPotion(int Count)
    {
        if(Count < 0)
        {
            healthPotions += Count;
            healthText.text = "x " + healthPotions.ToString();
            _playerController.currentHealthBar += 50;
            _playerController.currentHealthBar = Mathf.Clamp(_playerController.currentHealthBar, 0, _playerController.maxHealthBar);
        }
        if(Count > 0)
        {
            healthPotions += Count;
            healthText.text = "x " + healthPotions.ToString();
        }
        
        Debug.Log(healthPotions);
    }
}
