using BattleCore;

namespace Code.Services
{
    public static class Service
    {
        public static IBattleSettingsService BattleSettingsService { get; private set; }
        
        public static IBattleShipsService BattleShipsService { get; private set; }
        
        public static IBattleActionImageService BattleActionImageService { get; private set; }

        public static IBattleActionCreatorService BattleActionCreatorService { get; private set; }
        
        public static IBattleZone BattleZone { get; private set; }

        public static void Initialize(
            IBattleSettingsService battleSettingsService,
            IBattleShipsService battleShipsService,
            IBattleActionImageService battleActionImageService,
            IBattleActionCreatorService battleActionCreatorService,
            IBattleZone battleZone)
        {
            BattleSettingsService = battleSettingsService;
            BattleShipsService = battleShipsService;
            BattleActionImageService = battleActionImageService;
            BattleActionCreatorService = battleActionCreatorService;
            BattleZone = battleZone;
        }
    }
}