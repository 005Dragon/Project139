using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.UI
{
    public class BarController : MonoBehaviour
    {
        public event EventHandler<CreateBattleActionEventArgs> CreateBattleAction;
        
        public PlayerId Player;
        
        private TabController[] _tabControllers;

        public void Build(IEnumerable<GameObject> battleActionTemplates)
        {
            List<BattleActionController> battleActionControllers = CreateBattleActions(battleActionTemplates).ToList();

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

        private IEnumerable<BattleActionController> CreateBattleActions(IEnumerable<GameObject> battleActionTemplates)
        {
            foreach (GameObject template in battleActionTemplates)
            {
                GameObject battleActionGameObject = Instantiate(template);

                var battleActionController = battleActionGameObject.GetComponent<BattleActionController>();

                battleActionController.Player = Player;

                battleActionController.Click += OnClickBattleActionController;
                
                battleActionController.Initialize();

                yield return battleActionController;
            }
        }

        private void OnClickBattleActionController(object sender, EventArgs e)
        {
            var battleActionController = (BattleActionController) sender;

            var eventArgs = new CreateBattleActionEventArgs(Player, battleActionController.BattleActionCreator);
            
            CreateBattleAction?.Invoke(this, eventArgs);
        }
    }
}