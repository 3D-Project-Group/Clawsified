public class GameInfo
{
    public static bool Fighting_Boss = false;

    //Load Scene Control
    public static string SceneToLoad;
    public static string SceneToUnload;

    //Video Control
    public static int currentResolutionIndex = -1;
    public static int currentFpsLimitIndex = 3;
    public static bool fullScreen = true;

    //Volume Control
    public static float currentMasterVolume = 1;
    public static float currentMusicVolume = 1;
    public static float currentSoundFxVolume = 1;

    public static bool showingPopup = false;
    //Tutorial popups
    public static bool bossButtonsTutorial = false;
}
