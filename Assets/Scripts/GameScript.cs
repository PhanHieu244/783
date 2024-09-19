using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using PathCreation;

public class GameScript : MonoBehaviour
{

    public static GameScript instance;
    public GameObject TimerObj;
    public Text Timer_txt;
    private int timer;
    [HideInInspector]
    public bool playGame;
    public Player Player;
    public GameObject[] Triggers_obj;
    public PathCreator[] Pathes;
    public Transform[] TriggerSpawners;
    [HideInInspector]
    public List<GameObject> Triggers;
    public GameObject WinImg;
    public GameObject LostImg;
    public Material LogMaterial;
    public GameObject EffectOnFinish;
    public GameObject PlayButton;
    private Transform FinishPos;
    public Image ProgressToFinish;
    public Text ProgressToFinish_txt;
    public Text PlayerRate_txt;
    public GameObject GameParams;
    List<float> TriggerDistance = new List<float>();

    float PlayerDisToFinish;
    float TotalPosToFinish;
    float perOnePer;

    int spawnCounter;
    int playerRate;

    public void Awake()
    {

        Application.targetFrameRate = 65;

    }

    public void Start()
    {

        LogMaterial.color = RandomColor();

        instance = this;
        playGame = false;
        timer = 4;
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        FinishPos = GameObject.FindGameObjectWithTag("Finish").transform;
        PlayerDisToFinish = Vector3.Distance(Player.transform.position, FinishPos.position);
        TotalPosToFinish = PlayerDisToFinish;
        perOnePer = 1f / TotalPosToFinish;
        spawnCounter = TriggerSpawners.Length - 1;
        InvokeRepeating("TriggerSpawner", 1f, 0.5f);

        if (Player.startLog <= 0)
            Player.Anim.SetBool("Idle", true);
        else if (Player.startLog > 0)
            Player.Anim.SetBool("CarryIdle", true);

        //Triggers = GameObject.FindGameObjectsWithTag("Trigger");

    }

    public void Update()
    {

        if (playGame)
        {

            PlayerDisToFinish = Vector3.Distance(Player.transform.position, FinishPos.position);
            ProgressToFinish.fillAmount = perOnePer * (TotalPosToFinish - PlayerDisToFinish);
            ProgressToFinish_txt.text = ((int)(ProgressToFinish.fillAmount * 100f)).ToString() + "%";

        }

        CheckPlayerRate();

    }

    public void CheckPlayerRate()
    {

        if (playGame)
        {

            for (int i = 0; i < TriggerDistance.Count; i++)
            {

                if (Triggers[i] == null)
                    continue;
                else
                    TriggerDistance[i] = Vector3.Distance(Triggers[i].transform.position, FinishPos.position);

            }

            TriggerDistance.Add(PlayerDisToFinish);
            TriggerDistance.Sort();
            playerRate = TriggerDistance.IndexOf(PlayerDisToFinish);
            TriggerDistance.RemoveAt(playerRate);
            PlayerRate_txt.text = (playerRate + 1).ToString();

        }

    }

    public void TriggerSpawner()
    {

        GameObject obj;
        int randNum = UnityEngine.Random.Range(0, Triggers_obj.Length);
        obj = Instantiate(Triggers_obj[randNum], TriggerSpawners[spawnCounter].position, Quaternion.identity);
        obj.GetComponent<Trigger>().pathCreator = Pathes[spawnCounter];
        obj.GetComponent<Trigger>().CreateAppearEffect();
        TriggerSpawners[spawnCounter] = null;
        Triggers.Add(obj);
        TriggerDistance.Add(Vector3.Distance(obj.transform.position, FinishPos.position));
        spawnCounter--;
        if (spawnCounter < 0)
            CancelInvoke("TriggerSpawner");

    }

    public void StartTimer()
    {

        timer--;
        Timer_txt.text = timer.ToString();
        if(timer == 0)
        {

            Timer_txt.text = "Go!";

        }else if(timer <= -1)
        {

            TimerObj.SetActive(false);
            playGame = true;
            if (Player.AllLogs.Count <= 0)
                Player.Anim.SetBool("Idle", false);
            else if (Player.AllLogs.Count > 0)
                Player.Anim.SetBool("CarryIdle", false);
            for(int i = 0; i < Triggers.Count; i++)
            {

                if (Triggers[i].GetComponent<Trigger>().AllLogs.Count <= 0)
                    Triggers[i].GetComponent<Trigger>().Anim.SetBool("Idle", false);
                else if (Triggers[i].GetComponent<Trigger>().AllLogs.Count > 0)
                    Triggers[i].GetComponent<Trigger>().Anim.SetBool("CarryIdle", false);

            }
            CancelInvoke("StartTimer");

        }

    }

    public void FinishGame()
    {

        WinImg.SetActive(true);
        EffectOnFinish.SetActive(true);
        Player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        GameParams.SetActive(false);

    }

    public void GameOver()
    {

        LostImg.SetActive(true);
        GameParams.SetActive(false);


    }

    public void RestartGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    public Color32 RandomColor()
    {

        Color32 color;
        int r = UnityEngine.Random.Range(1, 255);
        int g = UnityEngine.Random.Range(1, 255);
        int b = UnityEngine.Random.Range(1, 255);
        color = new Color32(Convert.ToByte(r), Convert.ToByte(g), Convert.ToByte(b), 255);
        return color;

    }

    public void Play()
    {

        PlayButton.SetActive(false);
        TimerObj.SetActive(true);
        GameParams.SetActive(true);
        InvokeRepeating("StartTimer", 0f, 1f);

        GAManager.instance.OnStartGame();
        FacebookManager.instance.OnStartGame();

    }

}
