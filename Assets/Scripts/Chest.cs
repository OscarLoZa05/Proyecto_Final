using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{

    public PlayerPotions _playerPotions;

    void Awake()
    {
        //_playerPotions = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerPotions>();
    }
    void Update()
    {
        
    }

    public void Interact()
    {
        
        _playerPotions.HealthPotion(1);
        _playerPotions.ManaPotion(1);
    }
}
