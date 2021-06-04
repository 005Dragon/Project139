using System;
using System.Collections.Generic;
using System.Linq;
using Code.Battle.Core;
using Code.Battle.Core.Log;
using Code.Utils;
using UnityEngine;

namespace Code.Battle.UI
{
    public class BarController : MonoBehaviour
    {
        public event EventHandler<EventArgs<IBattleActionCreator>> CreateBattleAction;
        
        public PlayerSide playerSide;
        
        private TabController[] _tabControllers;

        public void Build(GameObject battleActionTemplate, IEnumerable<IBattleActionCreator> battleActionCreators)
        {
            List<BattleActionController> battleActionControllers =
                CreateBattleActions(battleActionTemplate, battleActionCreators).ToList();
            
            foreach (TabController tabController in _tabControllers)
            {
                tabController.Build(battleActionControllers.Where(x => x.BattleActionCreator.ActionType == tabController.BattleActionType));
            }
        }

        public void ResetTabs()
        {
            foreach (TabController tabController in _tabControllers)
            {
                tabController.Close();
                tabController.Show();
            }
        }
        
        private void Awake()
        {
            _tabControllers = GetComponentsInChildren<TabController>();

            foreach (TabController tabController in _tabControllers)
            {
                tabController.Click += OnTabClick;
            }
        }

        private void OnTabClick(object sender, EventArgs e)
        {
            var activeTabController = (TabController) sender;

            if (activeTabController.Opened)
            {
                ResetTabs();
            }
            else
            {
                OpenTab(activeTabController);
            }
        } 

        private void OpenTab(TabController tabControllerToOpen)
        {
            foreach (TabController tabController in _tabControllers)
            {
                if (tabController == tabControllerToOpen)
                {
                    tabController.Show();
                    tabController.Open();
                }
                else
                {
                    tabController.Close();
                    tabController.Hide();
                }
            }
        }

        private IEnumerable<BattleActionController> CreateBattleActions(
            GameObject battleActionTemplate,
            IEnumerable<IBattleActionCreator> battleActionCreators)
        {
            foreach (IBattleActionCreator battleActionCreator in battleActionCreators)
            {
                GameObject battleActionGameObject = Instantiate(battleActionTemplate);

                var battleActionController = battleActionGameObject.GetComponent<BattleActionController>();

                battleActionController.PlayerSide = playerSide;
                battleActionController.BattleActionCreator = battleActionCreator;

                battleActionController.Click += OnClickBattleActionController;

                battleActionController.Initialize();

                yield return battleActionController;
            }
        }

        private void OnClickBattleActionController(object sender, EventArgs e)
        {
            var battleActionController = (BattleActionController) sender;

            var eventArgs = new EventArgs<IBattleActionCreator>(battleActionController.BattleActionCreator);
            
            CreateBattleAction?.Invoke(this, eventArgs);
        }
    }
}