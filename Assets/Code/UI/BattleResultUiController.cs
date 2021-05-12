using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Code.UI
{
    public class BattleResultUiController : BattleUi
    {
        protected override Canvas Canvas { get; set; }

        [SerializeField] 
        private Text _winText;

        [SerializeField]
        private Text _loseText;
        
        private void Awake()
        {
            Canvas = GetComponent<Canvas>();
        }

        public override void Enable()
        {
            base.Enable();

            PlayerId? managedPlayer = ReferenceItems.BattleSettings.ManagedPlayer;

            if (managedPlayer == null)
            {
                _winText.enabled = true;
            }

            PlayerId destroyedPlayer = ReferenceItems.ShipControllers.First(x => x.Destroyed).Player;

            if (managedPlayer == destroyedPlayer)
            {
                _loseText.enabled = true;
            }
            else
            {
                _winText.enabled = true;
            }
        }

        public void Complete()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}