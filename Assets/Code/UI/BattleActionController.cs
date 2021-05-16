using System;
using System.Linq;
using Code.Battle.ActionCreators;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Code.UI
{
    public class BattleActionController : MonoBehaviour, IManagedInitializable
    {
        public PlayerSide Player { get; set; }

        public event EventHandler Click;

        public IBattleActionCreator BattleActionCreator => (IBattleActionCreator) _battleActionModelCreator;
        
        [SerializeField]
        private Object _battleActionModelCreator;
        
        public void OnClick() => Click?.Invoke(this, EventArgs.Empty);

        public void Initialize()
        {
            Sprite sprite = GetComponentsInChildren<Image>().Last().sprite;

            BattleActionCreator.PlayerSide = Player;
            BattleActionCreator.Sprite = sprite;
        }
    }
}
