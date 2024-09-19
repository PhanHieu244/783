using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Booster : MonoBehaviour
{

    public TextMeshProUGUI Text;
    public int Boost;
    private Animator Anim;

    public Material[] Materials;
    // 0 - Blue
    // 1 - Grey
    // 2 - Red
    // 3 - DesMaterial
    MeshRenderer MR;

    public void Start()
    {

        Anim = GetComponent<Animator>();
        MR = GetComponent<MeshRenderer>();
        Boost = Random.Range(-10, 20);
        if (Boost < 0)
        {

            MR.material = Materials[2];
            Text.text = Boost.ToString();

        }
        else if (Boost == 0)
        {

            MR.material = Materials[1];
            Text.text = Boost.ToString();

        }
        else if (Boost > 0)
        {

            MR.material = Materials[0];
            Text.text = "+" + Boost;

        }

    }

    public void DesObjMech()
    {

        Anim.SetTrigger("Des");
        MR.material = Materials[3];
        Text.gameObject.SetActive(false);

    }

}
