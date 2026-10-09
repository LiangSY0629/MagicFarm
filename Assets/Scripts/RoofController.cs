using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoofController : MonoBehaviour
{
    Tilemap tilemap;
    Color outColor;
    Color inColor;

    private void Start()
    {
        tilemap = GetComponent<Tilemap>();
        outColor = new Color(1, 1, 1, 1);
        inColor = new Color(1, 1, 1, 0.1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(SetRoofFade(outColor,inColor));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(SetRoofFade(inColor, outColor));
        }
    }

    IEnumerator SetRoofFade(Color color1,Color color2)
    {
        for (float t = 0; t < 0.2f; t += Time.deltaTime)
        {

            tilemap.color = Color.Lerp(color1, color2, t / 0.2f);

            yield return null;
        }

    }


}
