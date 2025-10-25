using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform playerModel;

    [SerializeField] private TrailRenderer playerTrail;

    [SerializeField] private ParticleSystem breakParticles;

    public Transform PlayerModel => playerModel;

    public TrailRenderer PlayerTrail => playerTrail;

    void Start()
    {
        TransitionManager.Instance.SetPlayer(transform);
    }
}
