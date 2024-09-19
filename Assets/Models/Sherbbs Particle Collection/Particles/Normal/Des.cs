using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Des : MonoBehaviour
{


    public void Start()
    {

        Invoke("DesObj", 3f);

    }

    public void DesObj()
    {

        Destroy(gameObject);

    }

}
