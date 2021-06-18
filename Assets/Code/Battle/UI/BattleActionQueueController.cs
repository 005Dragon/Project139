using System;
using System.Collections.Generic;
using System.Linq;
using BattleCore;
using BattleCore.Actions;
using BattleCore.Utils;
using Code.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Battle.UI
{
    public class BattleActionQueueController : MonoBehaviour, IBattleActionQueue
    {
        public event EventHandler<EventArgs<BattleAction>> ActivateAction; 
        
        private class BattleActionQueueItemPair
        {
            public BattleAction Action;

            public Transform QueueItemTransform;

            public BattleActionQueueItemPair(BattleAction action, Transform queueItemTransform)
            {
                Action = action;
                QueueItemTransform = queueItemTransform;
            }
        }

        public PlayerSide PlayerSide => _playerSide;

        public bool IsEmpty => _queueItems.Count == 0;

        public float CellSize;
        public float Gap;

        public float MinDistanceToLerp;

        public IEnumerable<BattleAction> BattleActionModels => _queueItems.Select(x => x.Action);

        [SerializeField]
        private PlayerSide _playerSide;

        [SerializeField] 
        private Transform _queue; 
        
        [SerializeField]
        private Transform _startPositionQueue;

        [SerializeField]
        private Transform _endPositionQueue;

        [SerializeField]
        private GameObject _queueItemTemplate;
        
        private readonly Queue<BattleActionQueueItemPair> _queueItems = new Queue<BattleActionQueueItemPair>();
        
        private BattleActionQueueItemPair _activeQueueItem;
        
        private bool _allSleep = true;
        
        public void Enqueue(BattleAction action)
        {
            GameObject queueItemGameObject = Instantiate(_queueItemTemplate, _startPositionQueue.position, Quaternion.identity, _queue);

            queueItemGameObject.GetComponent<Image>().sprite = Service.BattleActionImageService.GetSprite(action.Id);
            
            _queueItems.Enqueue(new BattleActionQueueItemPair(action, queueItemGameObject.transform));
            
            _allSleep = false;
        }

        public BattleAction Dequeue()
        {
            if (_queueItems.Count == 0)
            {
                return null;
            }
            
            _activeQueueItem = _queueItems.Dequeue();
            _activeQueueItem.Action.Finished += OnBattleActionFinished;

            _allSleep = false;
            
            ActivateAction?.Invoke(this, new EventArgs<BattleAction>(_activeQueueItem.Action));
            
            return _activeQueueItem.Action;
        }
        
        public void Clear()
        {
            _queueItems.Clear();
        }

        private void OnBattleActionFinished(object sender, EventArgs eventArgs)
        {
            Destroy(_activeQueueItem.QueueItemTransform.gameObject);

            _activeQueueItem = null;
            
            _allSleep = false;
        }

        

        private void Update()
        {
            if (!_allSleep)
            {
                _allSleep = true;
                
                int index = 0;

                if (_activeQueueItem != null)
                {
                    UpdateQueueItem(_activeQueueItem, ref index);
                }
                else
                {
                    index++;
                }
                
                foreach (BattleActionQueueItemPair item in _queueItems)
                {
                    UpdateQueueItem(item, ref index);
                }
            }
        }

        private void UpdateQueueItem(BattleActionQueueItemPair item, ref int index)
        {
            bool needMove = GetNeedMoveQueueItem(item.QueueItemTransform, index++, out Vector2 targetPosition);

            if (needMove)
            {
                MoveQueueItem(item.QueueItemTransform, targetPosition);
                _allSleep = false;
            }
        }

        private bool GetNeedMoveQueueItem(Transform queueItem, int index, out Vector2 targetPosition)
        {
            targetPosition = new Vector2(GetQueueItemOffset(index), _endPositionQueue.position.y);
            
            float distanceToTarget = Mathf.Abs(queueItem.position.x - targetPosition.x);

            return distanceToTarget >= MinDistanceToLerp;
        }
        
        private void MoveQueueItem(Transform queueItem, Vector2 targetPosition)
        {
            queueItem.position = Vector2.Lerp(queueItem.position, targetPosition, Time.deltaTime * 5);

            // ReSharper disable once Unity.InefficientPropertyAccess
            float distanceToTarget = Mathf.Abs(queueItem.position.x - targetPosition.x);
            
            if (distanceToTarget < MinDistanceToLerp)
            {
                queueItem.position = targetPosition;
            }
        }

        private float GetQueueItemOffset(int queueItemIndex)
        {
            float startPositionX = _endPositionQueue.position.x;
            
            float directionCoefficient = _startPositionQueue.position.x - startPositionX > 0 ? 1 : -1;
            
            return  startPositionX + (queueItemIndex * (CellSize + Gap) * directionCoefficient);
        }
    }
}