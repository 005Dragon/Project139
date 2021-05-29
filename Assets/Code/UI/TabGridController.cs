using System.Collections.Generic;
using System.Linq;
using Code.Battle.UI;
using UnityEngine;

namespace Code.UI
{
    public class TabGridController : MonoBehaviour
    {
        private class CellModel
        {
            public int X { get; }
            public int Y { get; }
            
            public Transform Transform { get; }
            
            public BattleActionController BattleActionController { get; set; }

            public bool IsFree => BattleActionController == null;

            public CellModel(int x, int y, Transform transform)
            {
                X = x;
                Y = y;
                Transform = transform;
            }
        }
        
        private readonly List<CellModel> _cells = new List<CellModel>();
        
        [SerializeField]
        private int _rowsCount;
        
        [SerializeField]
        private int _columnCount;

        public void SetBattleActionInFirstFreeCell(BattleActionController battleActionController)
        {
            CellModel cell = _cells.First(x => x.IsFree);

            //TODO Добавить проверку есть ли свободная ячейка в сетке.

            cell.BattleActionController = battleActionController;
            battleActionController.transform.SetParent(cell.Transform, false);
        }
        
        public void Build()
        {
            float cellWidthPercent = 1.0f / _columnCount;
            float cellHeightPercent = 1.0f / _rowsCount;

            for (int y = 0; y < _rowsCount; y++)
            {
                for (int x = 0; x < _columnCount; x++)
                {
                    var cellGameObject = new GameObject($"Cell{y}{x}");
                    cellGameObject.transform.parent = transform;
                    
                    var rectTransform = cellGameObject.AddComponent<RectTransform>();
                    
                    rectTransform.anchorMin = new Vector2(x * cellWidthPercent, 1 - y * cellHeightPercent - cellHeightPercent);
                    rectTransform.anchorMax = new Vector2(x * cellWidthPercent + cellWidthPercent, 1 - y * cellHeightPercent);
                    rectTransform.anchoredPosition = Vector2.zero;
                    rectTransform.sizeDelta = Vector2.zero;
                    
                    _cells.Add(new CellModel(x, y, cellGameObject.transform));
                }
            }
        }
    }
}