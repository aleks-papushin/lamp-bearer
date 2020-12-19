using Assets.Scripts;
using UnityEngine;

public class PlayerSounds : SoundSource
{
    [SerializeField] private AudioClip _jump;
    [SerializeField] private AudioClip[] _jumpToSide;
    [SerializeField] private AudioClip[] _landing;
    [SerializeField] private AudioClip[] _oilTaken;

    [SerializeField] private AudioSource _playerAudio;

    public void CornerJump()
    {
        this.PlayOnce(_playerAudio, this.PickRandom(_jumpToSide));
    }

    public void Jump()
    {
        this.PlayOnce(_playerAudio, _jump);
    }

    public void Landing()
    {
        this.PlayOnce(_playerAudio, this.PickRandom(_landing));
    }

    public void OilTaken()
    {
        this.PlayOnce(_playerAudio, this.PickRandom(_oilTaken));
    }

    private AudioClip PickRandom(AudioClip[] clip)
    {
        return clip[Random.Range(0, clip.Length)];
    }
}
