using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicPanelZoom : MonoBehaviour
{
    public RectTransform content;
    public float maxScale;
    public float minScale;
    public float zoomSpeed;

    private void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01)
        {
            float newScale = content.localScale.x + scroll * zoomSpeed;

            newScale = Mathf.Clamp(newScale, minScale, maxScale);
            content.localScale = Vector3.one * newScale;

        }
    }


}
