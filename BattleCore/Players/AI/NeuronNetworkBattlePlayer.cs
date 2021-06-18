using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using BattleCore.Utils;
using Platform.NeuronsNetworks;
using Platform.NeuronsNetworks.ActivateFunctions;
using Platform.NeuronsNetworks.NeuronLayers;
using Platform.NeuronsNetworks.NeuronLayers.Neurons;

namespace BattleCore.Players.AI
{
    public class NeuronNetworkBattlePlayer : IBattlePlayer
    {
        public event EventHandler<EventArgs<IBattleActionCreator>> CreateBattleAction;
        public event EventHandler Ready;
        
        public PlayerSide PlayerSide { get; }
        public bool IsReady { get; private set; }

        private static string DefaultNeuronNetworkNameFormat = "{0}NeuronNetwork.dat";
        
        private MutationNeuronNetwork _neuronNetwork;
        
        private readonly IBattleZone _battleZone;
        private readonly IBattleShip _selfShip;
        
        private Dictionary<int, IBattleActionCreator> _idToBattleActionCreatorIndex;

        public NeuronNetworkBattlePlayer(
            PlayerSide playerSide,
            IBattleZone battleZone,
            IBattleShip selfShip,
            MutationNeuronNetwork neuronNetwork = null)
        {
            _battleZone = battleZone;
            PlayerSide = playerSide;

            _selfShip = selfShip;

            _neuronNetwork = neuronNetwork;
        }

        public void AddEnableBattleActionCreators(IBattleActionCreator[] battleActionCreators)
        {
            _idToBattleActionCreatorIndex = CreateBattleActionCreatorIndex(battleActionCreators);

            if (_neuronNetwork == null)
            {
                _neuronNetwork = LoadNeuronNetwork() ?? CreateNeuronNetwork();
            }
        }

        public void Sleep()
        {
            IsReady = false;
        }

        public void Wake()
        {
            int? lastAction = null;
            
            while (_selfShip.Energy > 0)
            {
                GenerateBattleAction(ref lastAction);
            }

            IsReady = true;
            
            Ready?.Invoke(this, EventArgs.Empty);
        }
        
        private MutationNeuronNetwork CreateNeuronNetwork()
        {
            var neuronNetwork = new MutationNeuronNetwork(
                GetInputs(null).Count(),
                true,
                new NeuronLayerSettings(8, new LogisticFunction(), true),
                new NeuronLayerSettings(_idToBattleActionCreatorIndex.Count, new LogisticFunction(), false)
            );

            string fileName = string.Format(DefaultNeuronNetworkNameFormat, PlayerSide);
            
            if (!File.Exists(fileName))
            {
                BinaryFormatter formatter = new BinaryFormatter();

                using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
                {
                    formatter.Serialize(fileStream, neuronNetwork);
                }
            }

            return neuronNetwork;
        }

        private MutationNeuronNetwork LoadNeuronNetwork()
        {
            string name = string.Format(DefaultNeuronNetworkNameFormat, PlayerSide);
            
            if (File.Exists(name))
            {
                var formatter = new BinaryFormatter();

                using (FileStream fileStream = new FileStream(name, FileMode.Open))
                {
                    return (MutationNeuronNetwork) formatter.Deserialize(fileStream);
                }
            }

            return null;
        }

        private void GenerateBattleAction(ref int? lastActionIndex)
        {
            _neuronNetwork.SetInputs(GetInputs(lastActionIndex).ToArray());
            _neuronNetwork.Activate();

            Neuron[] result = _neuronNetwork.Result.ToArray();

            float maxResult = float.MinValue;
            int maxResultIndex = -1;
            
            for (int i = 0; i < result.Length; i++)
            {
                if (result[i].Signal > maxResult)
                {
                    maxResult = result[i].Signal;
                    maxResultIndex = i;
                }
            }

            lastActionIndex = maxResultIndex;

            CreateBattleAction?.Invoke(this, new EventArgs<IBattleActionCreator>(_idToBattleActionCreatorIndex[maxResultIndex]));
        }

        private Dictionary<int, IBattleActionCreator> CreateBattleActionCreatorIndex(IBattleActionCreator[] source)
        {
            Dictionary<int, IBattleActionCreator> result = new Dictionary<int, IBattleActionCreator>();
            
            int id = 0;

            foreach (IBattleActionCreator battleActionCreator in source)
            {
                if (battleActionCreator is ITargetBattleActionCreator targetBattleActionCreator)
                {
                    IEnumerable<IBattleZoneField> enableTargets = targetBattleActionCreator.GetEnableTargets(PlayerSide, _battleZone);

                    foreach (IBattleZoneField battleZoneField in enableTargets)
                    {
                        var battleActionCreatorClone = (ITargetBattleActionCreator) targetBattleActionCreator.Clone();
                        
                        battleActionCreatorClone.SetTargets(battleZoneField.ToSingleElementEnumerable());
                        
                        result.Add(id, battleActionCreatorClone);
                        
                        id++;
                    }
                }
                else
                {
                    var battleActionCreatorClone = (IBattleActionCreator)battleActionCreator.Clone();
                    
                    result.Add(id, battleActionCreatorClone);
                    
                    id++;
                }
            }

            return result;
        }

        private IEnumerable<float> GetInputs(int? lastActionIndex)
        {
            IBattleZoneField selfShipBattleZoneField = _battleZone.GetShipBattleZoneField(PlayerSide);

            selfShipBattleZoneField.TryGetShip(out IBattleShip selfShip);

            IBattleZoneField[] battleZoneFields = _battleZone.GetBattleZoneFields().ToArray();
            
            yield return selfShip.Health;
            yield return selfShip.Energy;

            IEnumerable<float> selfBattleZoneFieldIndexInputs =
                Enumerable.Range(0, battleZoneFields.Count(x => x.PlayerSide == PlayerSide))
                    .Select(x => x == selfShipBattleZoneField.Index ? 1.0f : 0.0f);

            foreach (float input in selfBattleZoneFieldIndexInputs)
            {
                yield return input;
            }

            IBattleZoneField enemyShipBattleZoneField = _battleZone.GetShipBattleZoneField(PlayerSide.GetAnother());

            enemyShipBattleZoneField.TryGetShip(out IBattleShip enemyShip);

            yield return enemyShip.Health;
            yield return enemyShip.Energy;
            
            IEnumerable<float> enemyBattleZoneFieldIndexInputs =
                Enumerable.Range(0, battleZoneFields.Count(x => x.PlayerSide == PlayerSide.GetAnother()))
                    .Select(x => x == enemyShipBattleZoneField.Index ? 1.0f : 0.0f);

            foreach (float input in enemyBattleZoneFieldIndexInputs)
            {
                yield return input;
            }

            yield return lastActionIndex + 1.0f ?? 0.0f;
        }
    }
}