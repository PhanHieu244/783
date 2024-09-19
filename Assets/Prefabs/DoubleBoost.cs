using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleBoost : MonoBehaviour
{

    public GameObject Boost_1;
    public GameObject Boost_2;

    public Booster b_1;
    public Booster b_2;

    public void Update()
    {

        if (Boost_1 != null || Boost_2 != null)
        {

            if (Boost_1 == null)
            {

                b_2.DesObjMech();
                b_2.GetComponent<BoxCollider>().enabled = false;

            }
            else if (Boost_2 == null)
            {

                b_1.DesObjMech();
                b_1.GetComponent<BoxCollider>().enabled = false;

            }

        }
        else
            GetComponent<DoubleBoost>().enabled = false;

    }

}
