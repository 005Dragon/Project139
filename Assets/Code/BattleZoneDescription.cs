using System;
using System.Collections.Generic;
using System.Linq;
using Code.Battle;
using Code.Battle.Core;
using Code.Utils;
using UnityEngine;

namespace Code
{
    public class BattleZoneDescription : MonoBehaviour, IBattleZone
    {
        [Serializable]
        private class PlayerBattleZones
        {
            public PlayerSide playerSide;

            public Transform[] BattleZones;
        }
        
        [SerializeField]
        private PlayerBattleZones[] _playerBattleZones;

        public IEnumerable<IBattleZoneField> GetBattleZoneFields()
        {
            foreach (PlayerBattleZones playerBattleZone in _playerBattleZones)
            {
                for (int i = 0; i < playerBattleZone.BattleZones.Length; i++)
                {
                    yield return new BattleZoneField(playerBattleZone.playerSide, i, playerBattleZone.BattleZones[i]);
                }
            }
        }
        
        public Transform GetCurrentBattleZone(Vector2 position)
        {
            int minRangeZoneId = 0;
            float minRange = float.MaxValue;
            
            List<Transform> battleZones = _playerBattleZones.SelectMany(x => x.BattleZones).ToList();

            for (int i = 0; i < battleZones.Count; i++)
            {
                float range = ((Vector2) battleZones[i].position - position).magnitude;

                if (range < minRange)
                {
                    minRange = range;
                    minRangeZoneId = i;
                }
            }

            return battleZones[minRangeZoneId];
        }

        public bool TryGetRelativeBattleZoneFieldByDirection(PlayerSide playerSide, Direction4 direction, out IBattleZoneField resultField)
        {
            IBattleZoneField currentField = GetShipBattleZoneField(playerSide);
            
            resultField = default;

            if (direction == Direction4.Left || direction == Direction4.Right)
            {
                if (direction == Direction4.Left && currentField.PlayerSide == PlayerSide.Left)
                {
                    return false;    
                }

                if (direction == Direction4.Right && currentField.PlayerSide == PlayerSide.Right)
                {
                    return false;
                }

                resultField = GetBattleZoneFields()
                    .First(x => x.Index == currentField.Index && x.PlayerSide == currentField.PlayerSide.GetAnother());

                return true;
            }

            if (direction == Direction4.Up || direction == Direction4.Down)
            {
                if (direction == Direction4.Up && currentField.Index == 0)
                {
                    return false;
                }

                IBattleZoneField[] fields = GetBattleZoneFields().Where(x => x.PlayerSide == currentField.PlayerSide).ToArray();

                if (direction == Direction4.Down && currentField.Index == fields.Length - 1)
                {
                    return false;
                }

                if (direction == Direction4.Up)
                {
                    resultField = fields.First(x => x.Index == currentField.Index - 1);

                    return true;
                }

                if (direction == Direction4.Down)
                {
                    resultField = fields.First(x => x.Index == currentField.Index + 1);

                    return true;
                }
            }

            return false;
        }

        public IBattleZoneField GetShipBattleZoneField(PlayerSide playerSide)
        {
            return GetBattleZoneFields().Where(x => x.PlayerSide == playerSide).First(x => x.TryGetShip(out _));
        }
    }
}
