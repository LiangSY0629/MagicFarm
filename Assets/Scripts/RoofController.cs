using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoofController : MonoBehaviour
{
    Tilemap tilemap;

    private void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            tilemap.color = new(1, 1, 1, 0.2f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            tilemap.color = new(1, 1, 1, 1);
        }
    }

}
