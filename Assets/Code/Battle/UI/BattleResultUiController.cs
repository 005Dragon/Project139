using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Code.Battle.UI
{
    public class BattleResultUiController : MonoBehaviour
    {
        private Canvas _canvas;

        [SerializeField] 
        private Text _winText;

        [SerializeField]
        private Text _loseText;
        
        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
        }

        public void Enable(PlayerSide winner)
        {
            // TODO Переделать.
            // _canvas.enabled = true;
            //
            // PlayerSide? managedPlayer = ReferenceItems.BattleSettings.ManagedPlayer;
            //
            // if (managedPlayer == null)
            // {
            //     _winText.enabled = true;
            // }
            //
            // PlayerSide destroyedPlayerSide = ReferenceItems.ShipControllers.First(x => x.Destroyed).PlayerSide;
            //
            // if (managedPlayer == destroyedPlayerSide)
            // {
            //     _loseText.enabled = true;
            // }
            // else
            // {
            //     _winText.enabled = true;
            // }
        }

        public void Complete()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}