using System.Collections.Generic;

namespace Assets.Scripts.Interfaces
{
    public interface IGameDataReader
    {
        List<GameWaveDto> ReadGameData();
    }
}
