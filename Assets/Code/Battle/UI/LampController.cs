using System;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Battle.UI
{
    public class LampController : MonoBehaviour
    {
        public bool State { get; private set; } 
        
        public Color Color;
        
        [SerializeField] 
        private Sprite _onLampSprite;

        [SerializeField]
        private Sprite _offLampSprite;

        [SerializeField]
        private float _blinkOnTime;
        
        [SerializeField]
        private float _blinkOffTime;
        
        private Image _image;
        private bool _blink;
        private float _startBlinkTime;
        
        public void On()
        {
            ChangeState(true);

            _blink = false;
        }

        public void Off()
        {
            ChangeState(false);
            
            _blink = false;
        }

        public void Blink()
        {
            _blink = true;

            _startBlinkTime = Time.time;
        }
        
        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        private void Update()
        {
            if (_blink)
            {
                if (State)
                {
                    if (Time.time - _startBlinkTime > _blinkOnTime)
                    {
                        _startBlinkTime = Time.time;
                        
                        ChangeState(false);
                    }
                }
                else
                {
                    if (Time.time - _startBlinkTime > _blinkOffTime)
                    {
                        _startBlinkTime = Time.time;
                        
                        ChangeState(true);
                    }
                }
            }
        }

        private void ChangeState(bool value)
        {
            State = value;

            _image.sprite = State ? _onLampSprite : _offLampSprite;
        }
    }
}