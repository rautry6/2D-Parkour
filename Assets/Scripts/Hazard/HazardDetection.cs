using UnityEngine;

public class HazardDetection : MonoBehaviour
{
    [SerializeField] private ParticleSystem breakParticle;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hazard"))
        {
            breakParticle.Play();
            RespawnManager.Instance.Respawn();
        }
    }
}
