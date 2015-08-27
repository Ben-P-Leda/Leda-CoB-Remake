namespace Shared.Scripts
{
    public enum AreaStage
    {
        One = 1,
        Two = 2,
        Boss = 3,
        Bonus = 4
    }

    public enum GameplayState
    {
        GetReady,
        InPlay,
        SequenceRunning,
        LevelComplete,
        GameOver
    }

    public enum PlayerDeathSequence
    {
        Generic = 1,
        Drowning = 2,
        Burning = 3
    }

    public enum ToolType
    {
        None = -1,
        Invincibility = 0,
        Jetpack = 1,
        SuperJump = 2,
        FirepowerUp = 3,
        Pickaxe = 4,
        FireExtinguisher = 5
    }

    public enum GateType
    {
        None = 0,
        Exit = 1,
        Warp = 2
    }
}
