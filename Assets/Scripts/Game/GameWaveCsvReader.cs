﻿using Assets.Scripts.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace Assets.Scripts.Game
{
    public class GameWaveCsvReader : IGameDataReader
    {
        public List<GameWaveDto> ReadGameData()
        {
            using (StreamReader file = new StreamReader("GameData/GameWaves.csv"))
            {
                _ = file.ReadLine(); // feed line with column names
                string wave;
                var gameWaves = new List<GameWaveDto>();

                while ((wave = file.ReadLine()) != null)
                {
                    gameWaves.Add(this.ParseWave(wave));
                }

                return gameWaves;
            }
        }

        private GameWaveDto ParseWave(string waveString)
        {
            string[] wave = waveString.Split(',');

            return new GameWaveDto
            {
                number = int.Parse(wave[0]),
                wallWarningInterval = float.Parse(wave[1]),
                wallDangerousInterval = float.Parse(wave[2]),
                wallCoolDownInterval = float.Parse(wave[3]),
                enemyCount = int.Parse(wave[4]),
                enemySpeed = float.Parse(wave[5])
            };
        }
    }
}
