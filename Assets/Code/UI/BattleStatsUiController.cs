﻿using System.Linq;
using UnityEngine;

namespace Code.UI
{
    public class BattleStatsUiController : BattleUi
    {
        protected override Canvas Canvas { get; set; }

        private BattleStatsBarController[] _statsBarControllers;

        public BattleStatsBarController GetStats(PlayerId player)
        {
            return _statsBarControllers.First(x => x.Player == player);
        }
        
        public override void Enable()
        {
        }

        public override void Disable()
        {
        }

        public override void Build()
        {
            base.Build();

            foreach (BattleStatsBarController statsBarController in _statsBarControllers)
            {
                 ShipController shipController = ReferenceItems.ShipControllers.First(x => x.Player == statsBarController.Player);
                
                statsBarController.Build(shipController);
            }
        }

        private void Awake()
        {
            Canvas = GetComponent<Canvas>();
            _statsBarControllers = GetComponentsInChildren<BattleStatsBarController>();
        }
    }
}