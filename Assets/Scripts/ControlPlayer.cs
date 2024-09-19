using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPlayer : MonoBehaviour
{

    private GameObject Player;
    private float DragSpeed;
    private Player PlayerScript;

    public void Start()
    {

        DragSpeed = 2.2f;
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerScript = Player.GetComponent<Player>();

    }

    float rotX;

    public void Update()
    {


    }

    #region Move Player X Pos

    public void Drag()
    {

        if (!PlayerScript.climbing)
        {

            rotX = Input.GetAxisRaw("Mouse X") * DragSpeed;
            Player.transform.eulerAngles += new Vector3(0f, rotX, 0f);

        }

    }

    #endregion

}
