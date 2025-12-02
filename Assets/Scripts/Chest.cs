using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    void Update()
    {
        
    }

    public void Interact()
    {
        GameManager.instance.HealthPotion(1);
        Debug.Log("Health Potion: " + GameManager.instance.healthPotion);
        GameManager.instance.ManaPotion(1);
        Debug.Log("Health Potion: " + GameManager.instance.manaPotion);
    }
}
