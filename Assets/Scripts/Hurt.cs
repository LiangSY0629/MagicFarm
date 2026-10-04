using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurt : MonoBehaviour
{
    public int damage;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.GetComponent<CharacterHealth>().change_health(-damage);

    }

}
