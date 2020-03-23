using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : Enemy
{
    public enum PatrollerState
    {
        MovingRight,
        MovingLeft
    }

    public PatrollerState CurrentState;

    [SerializeField] private LedgeCheck _ledgeCheckRight;
    [SerializeField] private LedgeCheck _ledgeCheckLeft;
    [SerializeField] private WallCheck _wallCheckRight;
    [SerializeField] private WallCheck _wallCheckLeft;

    void Start()
    {
        int rand = Random.Range(0, 2);
        if (rand == 1)
        {
            CurrentState = PatrollerState.MovingLeft;
        }
        else
        {
            CurrentState = PatrollerState.MovingRight;
        }
    }

    void Update()
    {
        switch (CurrentState)
        {
            case PatrollerState.MovingRight:
                if (_ledgeCheckRight.isApproachingLedge || _wallCheckRight.isApproachingWall)
                {
                    CurrentState = PatrollerState.MovingLeft;
                }

                spriteRenderer.transform.localScale = new Vector3(Mathf.Abs(spriteSize), spriteSize);
                //TODO Make Moving better
                transform.position = new Vector3(transform.position.x + .01f, transform.position.y, 0);
                break;

            case PatrollerState.MovingLeft:
                if (_ledgeCheckLeft.isApproachingLedge || _wallCheckLeft.isApproachingWall)
                {
                    CurrentState = PatrollerState.MovingRight;
                }

                spriteRenderer.transform.localScale = new Vector3(-Mathf.Abs(spriteSize), spriteSize);
                //TODO Make Moving better
                transform.position = new Vector3(transform.position.x - .01f, transform.position.y, 0);
                break;
        }
    }
}
