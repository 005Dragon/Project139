using System.Linq;
using Code.Battle.Core;
using Code.UI;
using UnityEngine;

namespace Code.Battle.UI
{
    public class BattleActionLineUiController : MonoBehaviour
    {
        public IBattleActionQueue[] BattleActionQueues { get; private set; }
        
        private void Awake()
        {
            BattleActionQueues = GetComponentsInChildren<BattleActionQueueController>().Select(x => (IBattleActionQueue) x).ToArray();
        }
    }
}