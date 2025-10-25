using System;
using Unity.Cinemachine;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private CinemachineCamera roomCam;

    public CinemachineCamera RoomCam => roomCam;
    [SerializeField] private Transform respawnPoint;

    public Transform RespawnPoint => respawnPoint;

    [SerializeField] private Transform transitionZone;

    public Transform TransitionZone => transitionZone;

}
