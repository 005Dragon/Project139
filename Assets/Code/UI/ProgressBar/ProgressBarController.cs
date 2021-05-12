using System.Collections.Generic;
using UnityEngine;

namespace Code.UI.ProgressBar
{
    public class ProgressBarController : MonoBehaviour
    {
        public Color BackgroundColor;
        
        public Color ActiveColor;

        public float Value { get; private set; }
        
        [SerializeField]
        private GameObject _startCellTemplate;
        
        [SerializeField]
        private GameObject _cellTemlate;
        
        [SerializeField]
        private GameObject _endCellTemplate;
        
        private readonly List<ProgressBarCellController> _barCells = new List<ProgressBarCellController>();

        public void SetValue(float value)
        {
            Value = value;

            for (int i = 0; i < _barCells.Count; i++)
            {
                _barCells[i].SetActive(i < value);
            }
        }
        
        public void Build(float maxValue)
        {
            _barCells.Add(Instantiate(_startCellTemplate, transform).GetComponent<ProgressBarCellController>());

            for (int i = 0; i < maxValue - 2; i++)
            {
                _barCells.Add(Instantiate(_cellTemlate,transform).GetComponent<ProgressBarCellController>());
            }
            
            _barCells.Add(Instantiate(_endCellTemplate, transform).GetComponent<ProgressBarCellController>());

            float offset = 0;

            foreach (ProgressBarCellController cellController in _barCells)
            {
                cellController.BackgroundColor = BackgroundColor;
                cellController.ActiveColor = ActiveColor;
                
                var rectTransform = cellController.GetComponent<RectTransform>();

                float currentSizeX = rectTransform.sizeDelta.x;
                
                rectTransform.offsetMin = new Vector2(offset, rectTransform.offsetMin.y);
                rectTransform.sizeDelta = new Vector2(currentSizeX, rectTransform.sizeDelta.y);
                
                offset += currentSizeX;
            }
            
            SetValue(maxValue);
        }
    }
}
