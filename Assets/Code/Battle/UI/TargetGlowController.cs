using System;
using Code.Battle.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Battle.UI
{
    public class TargetGlowController : MonoBehaviour, IManagedInitializable
    {
        public BattleZoneField BattleZoneField { get; set; }

        public event EventHandler Selected;
        
        private Image _image;
        private Button _button;

        public void Select() => Selected?.Invoke(this, EventArgs.Empty);
        
        public void Show()
        {
            _image.enabled = true;
            _button.enabled = true;
        }

        public void Hide()
        {
            _image.enabled = false;
            _button.enabled = false;
        }

        public void Initialize()
        {
            _image = GetComponent<Image>();
            _button = GetComponent<Button>();
        }
    }
}