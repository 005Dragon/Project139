using System;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Battle.UI
{
    public class BattleReactorUiController : MonoBehaviour
    {
        public int State { get; private set; }

        [SerializeField] 
        private float _changeStateDelay;
        
        [SerializeField] 
        private Sprite[] _sprites;

        private Image _image;
        private Text _text;

        private int _realState = 0;
        private float _lastChangeStateTime;

        private int _diagnosticState = -1;

        public void SetState(int value)
        {
            if (value >= _sprites.Length)
            {
                State = _sprites.Length - 1;
            }
            else
            {
                State = value;
            }
        }
        
        private void Awake()
        {
            _lastChangeStateTime = Time.time;
            _image = GetComponent<Image>();
            _text = GetComponentInChildren<Text>();
        }

        private void Start()
        {
            _diagnosticState = 0;

            State = 0;
        }

        private void Update()
        {
            if (_realState != State)
            {
                float changeStateDeltaTime = Time.time - _lastChangeStateTime;

                if (changeStateDeltaTime > _changeStateDelay)
                {
                    _lastChangeStateTime = Time.time;

                    if (_realState > State)
                    {
                        ChangeState(_realState - 1);
                    }
                    else
                    {
                        ChangeState(_realState + 1);
                    }
                }
            }

            if (_diagnosticState >= 0)
            {
                if (_diagnosticState == 0)
                {
                    if (_realState == 0)
                    {
                        _diagnosticState = 1;

                        State = 10;
                    }
                }
                else if (_diagnosticState == 1)
                {
                    if (_realState == 10)
                    {
                        _diagnosticState = 0;

                        State = 0;
                    }
                }
            }
        }

        private void ChangeState(int value)
        {
            _realState = value;

            _image.sprite = _sprites[value];
            _text.text = "+" + value;
        }
    }
}