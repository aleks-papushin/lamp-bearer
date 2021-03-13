﻿using System;
using Game;
using Resources;
using UnityEngine;
using Wall;

namespace Player
{
    public class PlayerCollisions : MonoBehaviour
    {
        [SerializeField] private PlayerSounds _playerSounds;
        private GameManager _gameManager;
        private PlayerController _playerController;
        private LightIntensityController _light;

        public static event Action OnOilBottleTaken;
        public static event Action OnPlayerDied;

        private void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();
            _playerController = GetComponent<PlayerController>();
            _light = GetComponent<LightIntensityController>();
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

        private void HandleWalls(Collision2D collision)
        {
            if (!collision.gameObject.tag.Contains(Tags.WallSuffix)) return;
            HandleDangerousWall(collision);
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
            _playerController.DirectionWasChangedInJump = false;
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
