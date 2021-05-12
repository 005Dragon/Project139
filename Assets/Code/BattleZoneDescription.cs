using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code
{
    public class BattleZoneDescription : MonoBehaviour
    {
        [Serializable]
        private class PlayerBattleZones
        {
            public PlayerId PlayerId;

            public Transform[] BattleZones;
        }
        
        [SerializeField]
        private PlayerBattleZones[] _playerBattleZones;

        public IEnumerable<PlayerBattleZoneDescription> GetPlayerBattleZoneDescriptions()
        {
            foreach (PlayerBattleZones playerBattleZones in _playerBattleZones)
            {
                PlayerId playerId = playerBattleZones.PlayerId;

                foreach (Transform battleZone in playerBattleZones.BattleZones)
                {
                    yield return new PlayerBattleZoneDescription {PlayerId = playerId, BattleZone = battleZone};
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

        public bool TryGetBattleZoneByDirection(Transform currentBattleZone, Direction4 direction, out Transform resultBattleZone)
        {
            resultBattleZone = default;
            
            foreach (PlayerBattleZones playerBattleZone in _playerBattleZones)
            {
                for (int i = 0; i < playerBattleZone.BattleZones.Length; i++)
                {
                    if (currentBattleZone == playerBattleZone.BattleZones[i])
                    {
                        if (direction == Direction4.Up || direction == Direction4.Down)
                        {
                            int targetZoneIndex = direction == Direction4.Up ? i - 1 : i + 1;

                            if (targetZoneIndex < 0 || targetZoneIndex >= playerBattleZone.BattleZones.Length)
                            {
                                return false;
                            }

                            resultBattleZone = playerBattleZone.BattleZones[targetZoneIndex];

                            return true;
                        }
                        else
                        {
                            if (direction == Direction4.Right && playerBattleZone.PlayerId == PlayerId.Right)
                            {
                                return false;
                            }

                            if (direction == Direction4.Left && playerBattleZone.PlayerId == PlayerId.Left)
                            {
                                return false;
                            }

                            PlayerBattleZones otherPlayerBattleZones =
                                _playerBattleZones.First(x => x.PlayerId != playerBattleZone.PlayerId);
                            
                            if (i >= playerBattleZone.BattleZones.Length)
                            {
                                return false;
                            }

                            resultBattleZone = otherPlayerBattleZones.BattleZones[i];

                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public Transform GetShipBattleZone(PlayerId player)
        {
            PlayerBattleZones playerBattleZones = _playerBattleZones.First(x => x.PlayerId == player);

            foreach (Transform battleZone in playerBattleZones.BattleZones)
            {
                if (battleZone.childCount > 0)
                {
                    return battleZone;
                }
            }

            throw new Exception("Ship battle zone not found.");
        }
    }
}
