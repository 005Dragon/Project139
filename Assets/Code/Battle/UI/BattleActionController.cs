using System;
using Code.Battle.Core;
using Code.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Battle.UI
{
    public class BattleActionController : MonoBehaviour, IManagedInitializable
    {
        public event EventHandler Click;
        
        public PlayerSide PlayerSide { get; set; }

        public IBattleActionCreator BattleActionCreator { get; set; }

        [SerializeField]
        private Image Image;
        
        public void OnClick() => Click?.Invoke(this, EventArgs.Empty);

        public void Initialize()
        {
            Image.sprite = Service.BattleActionImageService.GetSprite(BattleActionCreator.ActionId);
        }
    }
}
