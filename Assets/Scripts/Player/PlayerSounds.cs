using System.Collections.Generic;
using Interfaces;
using UnityEngine;

namespace Player
{
    public class PlayerSounds : MonoBehaviour, ICornerJumpSoundSource
    {
        [SerializeField] private AudioClip _jump;
        [SerializeField] private AudioClip[] _jumpToSide;
        [SerializeField] private AudioClip[] _landing;
        [SerializeField] private AudioClip[] _oilTaken;

        [SerializeField] private AudioSource _playerAudio;

        public void CornerJump()
        {
            _playerAudio.PlayOneShot(PickRandom(_jumpToSide));
        }

        public void Jump()
        {
            _playerAudio.PlayOneShot(_jump);
        }

        public void Landing()
        {
            _playerAudio.PlayOneShot(PickRandom(_landing));
        }

        public void OilTaken()
        {
            _playerAudio.PlayOneShot(PickRandom(_oilTaken));
        }

        private static AudioClip PickRandom(IReadOnlyList<AudioClip> clip)
        {
            return clip[Random.Range(0, clip.Count)];
        }
    }
}
