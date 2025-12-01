using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    void Update()
    {
        
    }

    public void Interact()
    {
        GameManager.instance.AddHealthPotion();
        Debug.Log("Health Potion: " + GameManager.instance.healthPotion);
        GameManager.instance.AddManaPotion();
        Debug.Log("Health Potion: " + GameManager.instance.manaPotion);
    }
}
