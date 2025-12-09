using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{

    PlayerPotions _playerPotions;

    void Awake()
    {
        _playerPotions = _playerPotions = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerPotions>();
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
