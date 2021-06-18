using System.Linq;
using UnityEngine;

namespace Code.Battle.UI
{
    public class BattleActionLineUiController : MonoBehaviour
    {
        public BattleBottomBarController[] BottomBarControllers { get; private set; }
        
        private void Awake()
        {
            BottomBarControllers = GetComponentsInChildren<BattleBottomBarController>().ToArray();
        }
    }
}