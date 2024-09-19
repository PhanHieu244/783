using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log : MonoBehaviour
{

    [HideInInspector]
    public float PosY;
    [SerializeField]
    Animator Anim;

    public GameObject DesEffect;

    public void Start()
    {

        //transform.localPosition = new Vector3(0f, PosY, 0f);

    }

    public void Update()
    {

        //transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(0f, PosY, 0f), 10f * Time.deltaTime);

    }

    public void DesObj() => Anim.SetTrigger("Des");

    public void EndAnim()
    {

        //Instantiate(DesEffect, transform.position, Quaternion.Euler(90f, 0f, 0f));
        Destroy(gameObject);

    }

}
