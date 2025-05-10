using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public Player player;

    private void Awake()
    {
        // Ensure singleton instance
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        // Try to auto-assign if not done in Inspector
        if (player == null)
        {
            player = FindObjectOfType<Player>();
            if (player == null)
                Debug.LogError("[PlayerManager] Player reference is missing and could not be found in the scene!");
        }
    }
}
