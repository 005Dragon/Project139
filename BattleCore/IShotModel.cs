namespace BattleCore
{
    public interface IShotModel
    {
        IBattleZoneField Target { get; }
        
        float Damage { get; }
    }
}