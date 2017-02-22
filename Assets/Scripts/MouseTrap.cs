using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTrap : MonoBehaviour
{
    private Animator anim;

    void Start ()

    {
        anim = gameObject.GetComponentInChildren<Animator>();
        anim.enabled = false;
    }

    void OnTriggerEnter ()
    {
        anim.enabled = true;
    }
}
