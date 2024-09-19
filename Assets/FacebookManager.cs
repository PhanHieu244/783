using UnityEngine;

public class FacebookManager : MonoBehaviour
{

    // Include Facebook namespace

    public static FacebookManager instance;

// Awake function from Unity's MonoBehavior
void Awake()
{
    instance = this; 
    DontDestroyOnLoad(this);

}

private void InitCallback()
{
  
}

private void OnHideUnity(bool isGameShown)
{
    if (!isGameShown)
    {
        // Pause the game - we will need to hide
        Time.timeScale = 0;
    }
    else
    {
        // Resume the game - we're getting focus again
        Time.timeScale = 1;
    }
}

    public void OnStartGame()
    {
        print("Game Start");

    }

}
