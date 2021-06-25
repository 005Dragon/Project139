using UnityEngine;

namespace Code.UI.ProgressBar
{
    public class ProgressBarUiController : MonoBehaviour
    {
        public float Value { get; private set; }

        public float MaxValue { get; private set; } = 1.0f;

        [SerializeField]
        private Transform _valueTransform;
        
        public void SetValue(float value, float maxValue)
        {
            MaxValue = maxValue;
            
            SetValue(value);
        }

        public void SetValue(float value)
        {
            value = value;

            Vector3 currentScale = _valueTransform.localScale;
            
            _valueTransform.localScale = new Vector3(value / MaxValue, currentScale.y, currentScale.z);
        }
    }
}