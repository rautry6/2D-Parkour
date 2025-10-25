using UnityEngine;

public class TransitionZone : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TransitionManager.Instance.TransitionLevel();
        }
    }
}
