using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    //Pociones
    [Header("Pociones")]
    public int healthPotion = 0;
    public int manaPotion = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ManaPotion(int used)
    {
        manaPotion += used;
    }
    public void HealthPotion(int used)
    {
        healthPotion += used;
    }
}
