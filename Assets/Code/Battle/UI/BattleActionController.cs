using System;
using System.Linq;
using Code.Battle.Core;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Code.Battle.UI
{
    public class BattleActionController : MonoBehaviour, IManagedInitializable
    {
        public PlayerSide PlayerSide { get; set; }

        public event EventHandler Click;

        public IBattleActionCreator BattleActionCreator
        {
            get => (IBattleActionCreator) _battleActionModelCreator;
            set => _battleActionModelCreator = (Object)value;
        }
        
        [SerializeField]
        private Object _battleActionModelCreator;
        
        public void OnClick() => Click?.Invoke(this, EventArgs.Empty);

        public void Initialize()
        {
            Sprite sprite = GetComponentsInChildren<Image>().Last().sprite;

            BattleActionCreator.PlayerSide = PlayerSide;
            BattleActionCreator.Sprite = sprite;
        }
    }
}
