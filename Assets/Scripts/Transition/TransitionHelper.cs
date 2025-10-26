using UnityEngine;
using DG.Tweening;
using System.Collections;
using Unity.Cinemachine;

public class TransitionHelper
{
    private Transform player;
    private Vector3 startingScale;
    private float playerShrinkTime;
    private Quaternion startingRotation;

    private float playerGrowTime;

    private float zoomTime;

    public bool playerGrown = true;

    public TransitionHelper(Transform player, float shrinkTime, float growTime, float zoomTime)
    {
        this.player = player;
        startingScale = player.localScale;
        startingRotation = player.rotation;

        playerShrinkTime = shrinkTime;
        playerGrowTime = growTime;

        this.zoomTime = zoomTime;
    }

    public void ShrinkPlayer(Transform transitionZone, float direction)
    {
        // move player holder
        player.parent.DOMove(transitionZone.position + Vector3.up, playerShrinkTime);

        // rotate player
        player.DORotate(new Vector3(0, 0, -180 * direction), playerShrinkTime * 1.1f);

        // shrink player
        player.DOScale(Vector3.zero, playerShrinkTime).OnComplete(() =>
        {
            playerGrown = false;
        });
    }

    public void GrowPlayer()
    {
        // rotate player
        player.DORotate(new Vector3(0, 0, -180), playerGrowTime * 0.9f);

        player.rotation = startingRotation;
        player.DOScale(startingScale, playerGrowTime).OnComplete(() =>
        {
            playerGrown = true;
        });
    }

    public IEnumerator ZoomInCam(CinemachineCamera currentCam, float zoomValue)
    {
        while (currentCam.Lens.OrthographicSize > zoomValue + 0.01f)
        {
            currentCam.Lens.OrthographicSize = Mathf.Lerp(
                currentCam.Lens.OrthographicSize,
                zoomValue,
                Time.deltaTime * zoomTime
            );

            yield return null; // Wait one frame before continuing
        }

        // Snap to exact value at the end to prevent floating point drift
        currentCam.Lens.OrthographicSize = zoomValue;
    }
}
