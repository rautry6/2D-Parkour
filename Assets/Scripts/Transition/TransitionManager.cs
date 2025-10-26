using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance;

    private int currentRoom = 0;

    public int CurrentRoom => currentRoom;

    private Room[] rooms;
    private Room room;
    private CinemachineCamera currentCam;
    private Transform currentTransitionZone;

    public delegate void TransitioningAction();

    public event TransitioningAction OnTransitionStart;
    public event TransitioningAction OnTransitionEnd;

    private Player player;
    private Rigidbody2D playerRigidbody;
    private Movement playerMovement;

    private TransitionHelper transitionEffectHelper;

    [Header("Settings")]
    [SerializeField] private float playerShrinkEffectDuration;
    [SerializeField] private float playerGrowEffectDuration;
    [SerializeField] private float cameraZoomEffectDuration;
    [SerializeField] private float cameraZoomEffectValue;

    private bool transitioning = false;
    void Awake()
    {
        Instance = this;

        FetchAllRooms();
    }

    private void FetchAllRooms()
    {
        rooms = FindObjectsByType<Room>(FindObjectsSortMode.InstanceID);

        float[] distances = new float[rooms.Length];
        for (int i = 0; i < rooms.Length; i++)
            distances[i] = Vector2.Distance(transform.position, rooms[i].transform.position);

        Sorting.QuickSortRooms(rooms, distances, 0, rooms.Length - 1);

        room = rooms[0];
        currentCam = room.RoomCam;
        currentTransitionZone = room.TransitionZone;
    }

    void DebugLogRooms()
    {
        for (int i = 0; i < rooms.Length; i++)
            Debug.Log(rooms[i].gameObject.name);
    }

    public void SetPlayer(Transform playerTransform)
    {
        player = playerTransform.GetComponent<Player>();
        playerMovement = playerTransform.GetComponent<Movement>();
        playerRigidbody = player.GetComponent<Rigidbody2D>();

        transitionEffectHelper = new TransitionHelper(player.PlayerModel, playerShrinkEffectDuration, playerGrowEffectDuration, cameraZoomEffectDuration);
    }

    public void TransitionLevel()
    {
        if (!transitioning)
        {
            transitioning = true;
            OnTransitionStart?.Invoke();

            // fancy theatrics needed

            StartCoroutine(Transitioning());
        }
    }

    private IEnumerator Transitioning()
    {
        transitionEffectHelper.ShrinkPlayer(currentTransitionZone, playerMovement.Direction);
        StartCoroutine(transitionEffectHelper.ZoomInCam(currentCam, cameraZoomEffectValue));
        yield return new WaitUntil(() => !transitionEffectHelper.playerGrown);

        MoveToNextRoom();
        yield return new WaitForSeconds(2f);

        transitionEffectHelper.GrowPlayer();
        yield return new WaitUntil(() => transitionEffectHelper.playerGrown);
        playerRigidbody.gravityScale = 1f;

        yield return new WaitForSeconds(0.6f);

        player.PlayerTrail.enabled = true;
        transitioning = false;
        OnTransitionEnd?.Invoke();
    }

    private void MoveToNextRoom()
    {
        currentRoom++;

        if (currentRoom < rooms.Length)
        {
            player.PlayerTrail.enabled = false;

            room = rooms[currentRoom];

            // Camera tricks
            CinemachineCamera lastCam = currentCam;
            currentCam = room.RoomCam;
            currentTransitionZone = room.TransitionZone;

            currentCam.Priority = 1;
            lastCam.Priority = 0;

            // teleport player to new respawn point
            playerRigidbody.gravityScale = 0f;
            playerRigidbody.linearVelocity = Vector2.zero;

            RespawnManager.Instance?.RoomCleared(room.RespawnPoint);
            Vector3 respawnPosition = room.RespawnPoint.position + Vector3.up * 2;

            playerRigidbody.transform.position = respawnPosition;

        }
    }

}
