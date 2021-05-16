using System.Collections.Generic;
using System.Linq;
using Code.Battle;
using Code.Battle.Actions;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class BattleActionQueueController : MonoBehaviour, IBattleActionQueue
    {
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

        public float CellSize;
        public float Gap;

        public float MinDistanceToLerp;

        public IEnumerable<BattleAction> BattleActionModels => QueueItems.Select(x => x.Action);

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
        
        private readonly Queue<BattleActionQueueItemPair> QueueItems = new Queue<BattleActionQueueItemPair>();

        private bool _allSleep = true;

        public void Enqueue(BattleAction action)
        {
            GameObject queueItemGameObject = Instantiate(_queueItemTemplate, _startPositionQueue.position, Quaternion.identity, _queue);

            queueItemGameObject.GetComponent<Image>().sprite = action.Sprite;
            
            QueueItems.Enqueue(new BattleActionQueueItemPair(action, queueItemGameObject.transform));
            
            _allSleep = false;
        }

        public BattleAction Dequeue()
        {
            if (QueueItems.Count == 0)
            {
                return null;
            }
            
            BattleActionQueueItemPair pair = QueueItems.Dequeue();

            Destroy(pair.QueueItemTransform.gameObject);
            
            _allSleep = false;

            return pair.Action;
        }
        
        private void Update()
        {
            if (!_allSleep)
            {
                _allSleep = true;
                
                int index = 0;
                foreach (BattleActionQueueItemPair item in QueueItems)
                {
                    bool needMove = GetNeedMoveQueueItem(item.QueueItemTransform, index++, out Vector2 targetPosition);

                    if (needMove)
                    {
                        MoveQueueItem(item.QueueItemTransform, targetPosition);
                        _allSleep = false;
                    }
                }
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