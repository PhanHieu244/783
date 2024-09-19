using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{

    Transform Player;

    public void Start()
    {

        Player = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0);

    }

    public void Update()
    {

        //  + new Vector3(0f, 9.30f, -13f)
        transform.position = Vector3.MoveTowards(transform.position, Player.position, 20f * Time.deltaTime);
        transform.LookAt(Player.parent.position + new Vector3(0f, 4f, 0f));
        //transform.eulerAngles = new Vector3(20f, Player.rotation.eulerAngles.y, 0f);

    }

}
