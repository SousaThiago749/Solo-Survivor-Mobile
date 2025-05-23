using UnityEngine;

public class PowerManager : MonoBehaviour
{
    public FireBallManager fireBallManager;
    public SunStrikeManager sunStrikeManager;
    public LightningManager lightningStrikeManager;
    public WaterBallManager waterManager;
    public BlackHoleManager blackHoleManager;
    public GlacialManager glacialManager;


    public void ChoosePower(string powerName)
    {
        switch (powerName)
        {
            case "FireBall":
                if (fireBallManager != null) fireBallManager.LevelUp();
                break;

            case "SunStrike":
                if (sunStrikeManager != null) sunStrikeManager.LevelUp();
                break;

            case "LightningStrike":
                if (lightningStrikeManager != null) lightningStrikeManager.LevelUp();
                break;

            case "WaterBall":
                if (waterManager != null) waterManager.LevelUp();
                break;
            
            case "BlackHole":
                if (blackHoleManager != null) blackHoleManager.LevelUp();
                break;

            case "Glacial":
                if (glacialManager != null) glacialManager.LevelUp();
                break;

        }
    }
}
