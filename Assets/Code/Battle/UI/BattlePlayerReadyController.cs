using System;
using BattleCore.UI;
using UnityEngine;

namespace Code.Battle.UI
{
    public class BattlePlayerReadyController : MonoBehaviour, IUiPlayerReady
    {
        public event EventHandler Ready;

        public void SetReady()
        {
            Ready?.Invoke(this, EventArgs.Empty);
        }
    }
}