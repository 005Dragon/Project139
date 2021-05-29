using System;
using System.Collections.Generic;
using Code.UI;
using UnityEngine;

namespace Code.Battle.UI
{
    public class TabController : MonoBehaviour
    {
        public BattleActionType BattleActionType;

        public event EventHandler Click;
        
        public bool Opened { get; private set; }

        private Animator _animator;
        private TabGridController _gridController;

        private static readonly int OpenAnimatorId = Animator.StringToHash("Open");
        private static readonly int ShowAnimatorId = Animator.StringToHash("Show");

        public void OnClick() => Click?.Invoke(this, EventArgs.Empty);

        public void Build(IEnumerable<BattleActionController> battleActions)
        {
            _gridController.Build();

            foreach (BattleActionController battleAction in battleActions)
            {
                _gridController.SetBattleActionInFirstFreeCell(battleAction);
            }
        }
        
        public void Open()
        {
            Opened = true;
            
            _animator.SetBool(OpenAnimatorId, Opened);
        }

        public void Close()
        {
            Opened = false;
            
            _animator.SetBool(OpenAnimatorId, Opened);  
        }
        
        public void Show() => _animator.SetBool(ShowAnimatorId, true);
        public void Hide() => _animator.SetBool(ShowAnimatorId, false);
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _gridController = GetComponentInChildren<TabGridController>();
        }
    }
}