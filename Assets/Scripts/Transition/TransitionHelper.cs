using UnityEngine;
using DG.Tweening;
using System.Collections;

public class TransitionHelper
{
    private Transform player;
    private Vector3 startingScale;
    private float playerShrinkTime;
    private Quaternion startingRotation;

    private float playerGrowTime;

    public bool playerGrown = true;

    public TransitionHelper(Transform player, float shrinkTime, float growTime)
    {
        this.player = player;
        startingScale = player.localScale;
        startingRotation = player.rotation;

        playerShrinkTime = shrinkTime;
        playerGrowTime = growTime;
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
        player.rotation = startingRotation;
        player.DOScale(startingScale, playerGrowTime).OnComplete(() =>
        {
            playerGrown = true;
        });
    }
}
