using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZEngine.Unity.Core;
using ZEngine.Unity.Core.Components;

public class Titlable : UIMonoBehaviour
{
    private Vector2 anchorPos;

    public float Speed;
    // Start is called before the first frame update
    void Start()
    {
        anchorPos = this.rectTransform.anchoredPosition;
    }

    public Vector2 viewport;
    // Update is called once per frame
    void Update()
    {
        viewport = ZCamera.main.ScreenToViewportPoint(
            Input.mousePosition);
        

        rectTransform.anchoredPosition = anchorPos + (viewport * Speed);
    }
}
