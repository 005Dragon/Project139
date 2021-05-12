using System;
using System.Linq;
using Code.BattleActionCreators;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Code.UI
{
    public class BattleActionController : MonoBehaviour, IManagedInitializable
    {
        public PlayerId Player { get; set; }

        public event EventHandler Click;

        public IBattleActionCreator BattleActionCreator => (IBattleActionCreator) _battleActionModelCreator;
        
        [SerializeField]
        private Object _battleActionModelCreator;
        
        public void OnClick() => Click?.Invoke(this, EventArgs.Empty);

        public void Initialize()
        {
            Sprite sprite = GetComponentsInChildren<Image>().Last().sprite;

            BattleActionCreator.Player = Player;
            BattleActionCreator.Sprite = sprite;
        }
    }
}
