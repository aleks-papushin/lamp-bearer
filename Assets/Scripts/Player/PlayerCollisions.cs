﻿using Assets.Scripts.Resources;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    private PlayerController _playerController;

    public bool IsTouchingBottom => this.IsTouching(TagNames.BottomWallTag);
    public bool IsTouchingUpperWall => this.IsTouching(TagNames.UpperWallTag);
    public bool IsTouchingLeftWall => this.IsTouching(TagNames.LeftWallTag);
    public bool IsTouchingRightWall => this.IsTouching(TagNames.RightWallTag);

    void Start()
    {
        _playerController = GetComponent<PlayerController>();
    }

    private bool IsTouching(string tagName)
    {
        var otherCollider = GameObject.FindGameObjectWithTag(tagName).GetComponent<Collider2D>();
        return transform.GetChild(0).GetComponent<Collider2D>().IsTouching(otherCollider);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        this.HandleWallCollisions(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Contains(TagNames.OilBottle))
        {
            // destroy oil bottle
            Destroy(collision.gameObject);
        }
    }

    private void HandleWallCollisions(Collision2D collision)
    {
        if (collision.gameObject.tag.Contains(TagNames.WallTagSuffix))
        {
            _playerController.directionAlreadyChangedInJump = false;
        }
    }
}
