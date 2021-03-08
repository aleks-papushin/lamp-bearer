﻿using Assets.Scripts.Resources;
using System;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerCollisions : MonoBehaviour
    {
        [SerializeField] private PlayerSounds _playerSounds;
        private GameManager _gameManager;
        private PlayerController _playerController;
        private LightIntensityController _light;
        private Animator _animator;
        private static readonly int Jumping = Animator.StringToHash("IsJumping");

        public static event Action OnOilBottleTaken;
        public static event Action OnPlayerDied;

        private void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();
            _playerController = GetComponent<PlayerController>();
            _light = GetComponent<LightIntensityController>();
        }

        public void SetAnimator(Animator animator)
        {
            _animator = animator;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            HandleWalls(collision);        
            HandlePlayerGrounding(collision);
            HandleEnemy(collision);
        }

        private void HandleEnemy(Collision2D collision)
        {
            if (collision.gameObject.tag.Contains(Tags.Enemy))
            {
                HandlePlayerDead();
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.tag.Contains(Tags.WallSuffix))
            {
                HandleBeingOnWall(collision, false);
            }
        }

        private void HandleWalls(Collision2D collision)
        {
            if (!collision.gameObject.tag.Contains(Tags.WallSuffix)) return;
            HandleDangerousWall(collision);
            _animator.SetBool(Jumping, false);
            HandleBeingOnWall(collision, true);
        }

        private static void HandleBeingOnWall(Collision2D collision, bool isOnWall)
        {
            collision.transform.GetComponent<WallPlayerCollisions>().IsPlayerStandsOnMe = isOnWall;
        }

        private void OnCollisionStay2D(Collision2D collision)
        {        
            HandleDangerousWall(collision);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(Tags.OilBottle))
            {
                OilBottleTaken(collision);
            }
        }

        private void OilBottleTaken(Component oilBottle)
        {
            Destroy(oilBottle.gameObject);
            _gameManager.UpdateScore(1);
            _playerSounds.OilTaken();
            _light.OilTaken();
            OnOilBottleTaken?.Invoke();
        }

        private void HandlePlayerGrounding(Collision2D collision)
        {
            if (!collision.gameObject.tag.Contains(Tags.WallSuffix)) return;
            _playerController._isChangedDirectionInJump = false;
            _playerController.UnfreezeRig();
            _playerSounds.Landing();
        }

        private void HandleDangerousWall(Collision2D collision)
        {
            var variableWall = collision.gameObject.GetComponent<WallDanger>();
            if (variableWall != null && variableWall.IsDangerous)
            {
                HandlePlayerDead();
            }
        }

        private void HandlePlayerDead()
        {
            Destroy(gameObject);            
        }

        private void OnDestroy()
        {
            OnPlayerDied?.Invoke();
        }
    }
}
