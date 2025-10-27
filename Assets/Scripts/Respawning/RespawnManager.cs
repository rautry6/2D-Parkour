using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform respawnPoint;

    [SerializeField] private GameObject player;

    public static RespawnManager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private float respawnHeightOffset = 1.0f;

    public delegate void PlayerRespawnDelegate();

    public event PlayerRespawnDelegate OnPlayerRespawn;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Respawn()
    {
        OnPlayerRespawn?.Invoke();
        player.transform.position = respawnPoint.position + new Vector3(0, respawnHeightOffset, 0);
    }

    public void RoomCleared(Transform newRespawnPoint)
    {
        respawnPoint = newRespawnPoint;
    }
}
