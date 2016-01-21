using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static int currentScore;
    public static int highScore;

    public static int currentLevel = 0;
    public static int unlockedLevel;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public static void CompleteLevel()
    {
        currentLevel += 1;
        Application.LoadLevel(currentLevel);
    }
}
