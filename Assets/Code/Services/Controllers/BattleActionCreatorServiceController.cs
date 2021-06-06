using System;
using System.Collections.Generic;
using System.Linq;
using BattleCore;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Services.Controllers
{
    public class BattleActionCreatorServiceController : MonoBehaviour, IBattleActionCreatorService
    {
        [SerializeField]
        private MonoBehaviour _source;

        private IBattleActionCreator[] _battleActionCreators;

        public void Awake()
        {
            _battleActionCreators = _source.GetComponents<MonoBehaviour>().Select(x => (IBattleActionCreator) x).ToArray();
        }

        public IEnumerable<IBattleActionCreator> GetBattleActionCreators() => _battleActionCreators;
    }
}