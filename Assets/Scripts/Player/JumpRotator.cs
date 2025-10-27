using UnityEngine;
using DG.Tweening;

public class JumpRotator : MonoBehaviour
{
    [SerializeField] private Transform playerModel;
    [SerializeField] private float rotationSpeed;

    private Jumping jumpingControls;
    private Movement movementControls;
    private Quaternion startingRotation;

    private Tween currentTween;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        jumpingControls = GetComponent<Jumping>();
        movementControls = GetComponent<Movement>();
        startingRotation = playerModel.rotation;
    }

    void OnEnable()
    {
        jumpingControls.OnJump += RotatePlayer;
    }

    void OnDisable()
    {
        jumpingControls.OnJump -= RotatePlayer;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void RotatePlayer()
    {
        currentTween?.Kill();

        playerModel.rotation = startingRotation;

        currentTween = playerModel.DORotate(new Vector3(0, 0, -180 * movementControls.Direction), rotationSpeed).OnComplete(() =>
        {
            playerModel.rotation = startingRotation;
        });
    }

    public void StopRotate()
    {
        currentTween?.Kill();

        playerModel.rotation = startingRotation;
    }
}
