using System;
using System.Linq;
using BattleCore.Actions;
using UnityEngine;

namespace Code.Services.Controllers
{
    public class BattleActionImageServiceController : MonoBehaviour, IBattleActionImageService
    {
        [Serializable]
        private class BattleActionImage
        {
            public BattleActionId BattleActionId;

            public Sprite Sprite;
        }
        
        [SerializeField]
        private BattleActionImage[] _images;

        public Sprite GetSprite(BattleActionId battleActionId)
        {
            return _images.First(x => x.BattleActionId == battleActionId).Sprite;
        }
    }
}