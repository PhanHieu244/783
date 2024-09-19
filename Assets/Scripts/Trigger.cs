﻿using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{

    private Transform LogContiner;
    public GameObject Log;
    public GameObject LogPlatform;
    public List<GameObject> AllLogs = new List<GameObject>();
    public List<GameObject> AllLogPlatforms = new List<GameObject>();
    float PosLogY;
    [HideInInspector]
    public bool onGround;
    public float Speed;
    private float in_speed;
    bool canSpawn;
    public float SpawnSpeed;
    [HideInInspector]
    public Animator Anim;
    private Rigidbody rb;
    bool doJump;
    bool overPlatform;
    [HideInInspector]
    public bool climbing;
    public GameObject RunEffect;
    private bool onLogPlatform;
    bool onFinish;

    [HideInInspector]
    public PathCreator pathCreator;
    float TravelledDistance;

    public GameObject AppearEffect;
    int startLog;

    public void Awake()
    {

        onFinish = false;
        onGround = true;
        canSpawn = true;
        overPlatform = false;
        doJump = true;
        climbing = false;
        onLogPlatform = false;
        Speed = 10f;
        SpawnSpeed = 0.075f;
        in_speed = Speed;
        rb = GetComponent<Rigidbody>();
        Anim = GetComponent<Animator>();
        //pathCreator = GameObject.FindGameObjectWithTag("Path").GetComponent<PathCreator>();
        LogContiner = transform.GetChild(0);
        PosLogY = 0;
        startLog = 5;
        CreatLog(startLog);

        if (startLog <= 0)
            Anim.SetBool("Idle", true);
        else if (startLog > 0)
            Anim.SetBool("CarryIdle", true);

        TravelledDistance = 0f; // 15

    }

    public void CreateAppearEffect()
    {

        Instantiate(AppearEffect, transform.position + new Vector3(0f, 1.5f, 0f), Quaternion.Euler(90f, 0f, 0f));

    }

    int amountLog;

    public void CreatLog(int amount)
    {

        if (amount > 0)
        {

            amountLog = amount;
            InvokeRepeating("SpawnLog", 0.05f, 0.05f);

        }
        else if (amount < 0)
        {

            amountLog = Mathf.Abs(amount);
            InvokeRepeating("DesLog", 0.03f, 0.03f);

        }

    }

    void SpawnLog()
    {

        PosLogY = AllLogs.Count * 0.17f;
        GameObject obj = Instantiate(Log, new Vector3(0f, PosLogY, 0f), Quaternion.identity);
        AllLogs.Add(obj);
        obj.transform.SetParent(LogContiner.transform, false);
        amountLog--;
        if (amountLog <= 0)
            CancelInvoke("SpawnLog");


    }

    void DesLog()
    {

        if (AllLogs.Count > 0)
        {

            AllLogs[AllLogs.Count - 1].GetComponent<Log>().DesObj();
            AllLogs.RemoveAt(AllLogs.Count - 1);
            amountLog--;

        }

        if (amountLog <= 0 || AllLogs.Count <= 0)
            CancelInvoke("DesLog");

    }

    public void CreateLogUnderPlayer()
    {

        if (AllLogs.Count > 0)
        {

            GameObject obj = Instantiate(LogPlatform, new Vector3(transform.position.x, -0.08f, transform.position.z), transform.rotation);
            AllLogPlatforms.Add(obj);
            canSpawn = true;
            Destroy(AllLogs[AllLogs.Count - 1]);
            AllLogs.RemoveAt(AllLogs.Count - 1);

        }

    }

    public void Update()
    {

        if (GameScript.instance.playGame)
            Speed = in_speed;
        else
            Speed = 0f;

        if (GameScript.instance.playGame)
        {

            if (climbing)
                Speed = 0f;
            else
                Speed = in_speed;

        }

        TravelledDistance += Speed * Time.deltaTime;
        if (!onFinish)
        {

            if ((onGround && AllLogs.Count > 0) || (onGround && AllLogs.Count <= 0) || (!onGround && AllLogs.Count > 0))
            {

                transform.position = pathCreator.path.GetPointAtDistance(TravelledDistance);
                transform.rotation = pathCreator.path.GetRotationAtDistance(TravelledDistance);

            }
            else if (!onGround && AllLogs.Count <= 0)
            {

                transform.position += transform.forward * Speed * Time.deltaTime;


            }

        }

        if (!onGround && canSpawn)
        {

            Invoke("CreateLogUnderPlayer", SpawnSpeed);
            canSpawn = false;

        }

        if (AllLogs.Count <= 0 && onGround && !overPlatform && !climbing)
            Anim.SetBool("Run", true);
        else if (AllLogs.Count > 0 && onGround && !overPlatform && !climbing)
            Anim.SetBool("Run", false);

        CheckAmountLogs();

        if (onGround && !overPlatform && !climbing)
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, 0f, transform.position.z), 2f * Time.deltaTime);

        if ((AllLogs.Count <= 0 && onLogPlatform && !onGround) || (onGround && onLogPlatform))
        {

            SpawnSpeed = 0.075f;
            in_speed = 10f;
            RunEffect.SetActive(false);
            onLogPlatform = false;
            Anim.SetFloat("SpeedCarryRun", 1.1f);
            CancelInvoke("BoostSpeed");

        }

    }

    public void CheckAmountLogs()
    {

        if (!climbing)
        {

            if (AllLogs.Count > 0)
            {

                rb.useGravity = false;

            }
            else if (AllLogs.Count <= 0)
            {

                rb.useGravity = true;

                if (doJump && !onGround)
                {

                    Jump();
                    doJump = false;

                }
                else if (!doJump && !onGround && overPlatform)
                {

                    Anim.SetBool("FreeFall", true);
                    Anim.SetBool("Jump", false);

                }

            }

        }
        else if (climbing)
            rb.useGravity = false;

    }

    public void Jump()
    {

        if (AllLogs.Count <= 0)
        {

            rb.AddForce(Vector2.up * 320f);
            Anim.SetBool("Jump", true);

        }

    }

    public void OnTriggerEnter(Collider other)
    {

        if (other.transform.tag == "DeadZone")
            Destroy(gameObject);

        if (other.transform.tag == "OverPlatform")
        {

            overPlatform = true;

        }

        if(other.transform.tag == "Finish")
        {

            onFinish = true;
            foreach (GameObject i in AllLogs)
                Destroy(i);
            AllLogs.Clear();
            Anim.SetBool("Run", false);
            Anim.SetTrigger("Dance");

        }

    }

    public void OnTriggerExit(Collider other)
    {

        if (other.transform.tag == "OverPlatform")
        {

            overPlatform = false;
            Anim.SetBool("Climbing", false);

        }


    }

    public void OnCollisionEnter(Collision collision)
    {

        if (collision.transform.tag == "Booster")
        {

            CreatLog(collision.transform.GetComponent<Booster>().Boost);
            Destroy(collision.transform.gameObject);

        }

        if (collision.transform.tag == "Ground" && overPlatform)
        {

            Anim.SetBool("Climbing", true);
            Anim.SetBool("Jump", false);
            Anim.SetBool("FreeFall", false);
            Anim.SetBool("Run", false);
            Vector3 hitPoint = Vector3.zero;
            foreach (ContactPoint contact in collision.contacts)
            {

                hitPoint = contact.point;

            }
            transform.position = new Vector3(hitPoint.x, -1.67f, hitPoint.z);
            climbing = true;

        }

    }

    public void OnCollisionStay(Collision collision)
    {

        if (collision.transform.tag == "Ground")
        {

            Anim.SetBool("Jump", false);
            if (!climbing)
                doJump = true;
            onGround = true;
            canSpawn = false;

        }

    }

    public void OnCollisionExit(Collision collision)
    {

        if (collision.transform.tag == "Ground")
        {

            onGround = false;
            canSpawn = true;
            if (AllLogs.Count > 0)
            {

                onLogPlatform = true;
                Invoke("BoostSpeed", 0.9f);

            }

        }

    }

    public void BoostSpeed()
    {

        in_speed = 15f;
        SpawnSpeed /= 1.20f;
        RunEffect.SetActive(true);
        Anim.SetFloat("SpeedCarryRun", 2f);

    }

    public void OffClimbing()
    {

        Anim.SetBool("Jump", false);
        Anim.SetBool("Run", false);
        Anim.SetBool("Climbing", false);
        transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        climbing = false;

    }

}
