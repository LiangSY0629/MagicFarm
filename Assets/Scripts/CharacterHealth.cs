using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    public int current_health;
    public int max_health;


    public void change_health(int amount)
    {
        current_health += amount;

        if (current_health <= 0)
        {
            gameObject.SetActive(false);

        }
    }
}
