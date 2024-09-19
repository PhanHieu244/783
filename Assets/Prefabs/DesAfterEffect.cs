using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesAfterEffect : MonoBehaviour
{

    public ParticleSystem PS;

    public void Update()
    {

        if (PS.isStopped)
            Destroy(gameObject);

    }

}
