using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerSounds : MonoBehaviour
    {
        [SerializeField] private AudioClip _jump;
        [SerializeField] private AudioClip _airTurning;
        [SerializeField] private AudioClip[] _jumpToSide;
        [SerializeField] private AudioClip[] _landing;
        [SerializeField] private AudioSource _playerAudio;

        private bool _gameStarted;

        public void CornerJump()
        {
            _playerAudio.PlayOneShot(PickRandom(_jumpToSide));
        }

        public void Jump(bool isGrounded)
        {
            if (isGrounded)
            {
                _playerAudio.PlayOneShot(_jump);
            }
            else
            {
                _playerAudio.PlayOneShot(_airTurning, 0.2f);
            }
        }

        public void Landing()
        {
            if (!_gameStarted)
            {
                _gameStarted = true;
                return;
            }

            _playerAudio.PlayOneShot(PickRandom(_landing));
        }

        private AudioClip PickRandom(IReadOnlyList<AudioClip> clip) => clip[Random.Range(0, clip.Count)];
    }
}
