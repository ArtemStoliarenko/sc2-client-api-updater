using System;

namespace Sc2ApiUpdater
{
    [Flags]
    public enum ApiCalls
    {
        None = 0,
        Ui = 0b01,
        Game = 0b10,
        All = Ui | Game
    }
}
