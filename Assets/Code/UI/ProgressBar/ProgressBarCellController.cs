using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.ProgressBar
{
    public class ProgressBarCellController : MonoBehaviour
    {
        public bool IsActive { get; private set; }
        
        public Color BackgroundColor;
        
        public Color ActiveColor;
        
        [SerializeField]
        private Image _image;

        public void SetActive(bool value)
        {
            IsActive = value;

            _image.color = IsActive ? ActiveColor : BackgroundColor;
        }
    }
}