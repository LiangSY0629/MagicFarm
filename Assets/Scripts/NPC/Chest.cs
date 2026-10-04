using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void CloseChest()
    {
        anim.SetBool("Open", false);
    }

}
