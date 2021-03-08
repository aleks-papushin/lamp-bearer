using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Interfaces;
using UnityEngine;

public class PlayerSounds : SoundSource, ICornerJumpSoundSource
{
    [SerializeField] private AudioClip _jump;
    [SerializeField] private AudioClip[] _jumpToSide;
    [SerializeField] private AudioClip[] _landing;
    [SerializeField] private AudioClip[] _oilTaken;

    [SerializeField] private AudioSource _playerAudio;

    public void CornerJump()
    {
        PlayOnce(_playerAudio, PickRandom(_jumpToSide));
    }

    public void Jump()
    {
        PlayOnce(_playerAudio, _jump);
    }

    public void Landing()
    {
        PlayOnce(_playerAudio, PickRandom(_landing));
    }

    public void OilTaken()
    {
        PlayOnce(_playerAudio, PickRandom(_oilTaken));
    }

    private static AudioClip PickRandom(IReadOnlyList<AudioClip> clip)
    {
        return clip[Random.Range(0, clip.Count)];
    }
}
