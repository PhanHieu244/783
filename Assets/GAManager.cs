using UnityEngine;

public class GAManager : MonoBehaviour
{

    public static GAManager instance;

    public void Awake()
    {

        instance = this;
        DontDestroyOnLoad(this);

    }

    public void Start()
    {

    }

    public void OnStartGame()
    {
        print("Start Game");
    }

}
