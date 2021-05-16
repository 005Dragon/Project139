using System;

namespace Code.Battle
{
    public interface IBattleStep
    {
        event EventHandler Finished;
        
        void Play();
    }
}