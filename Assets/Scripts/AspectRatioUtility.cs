using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Code taken from Max O'Didily on Youtube
public class AspectRatioUtility : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    /*[SerializeField] private Canvas[] canvases;*/
    private float lastWidth, lastHeight;

    void Start()
    {
        Adjust();
    }

    private void Update()
    {
        if(Screen.width !=  lastWidth || Screen.height != lastHeight)
        {
            Adjust();
        }
    }

    public void Adjust()
    {
        lastHeight = (float)Screen.height;
        lastWidth = (float)Screen.width;
        float targetAspect = 16.0f / 9.0f;
        float windowApect = (float)Screen.width/(float)Screen.height;
        float scaleHeight = windowApect / targetAspect;

        if(scaleHeight < 1.0f)
        {
            Rect rect = _camera.rect;

            rect.width = 1;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;

            _camera.rect = rect;
            /*foreach (Canvas canvas in canvases)
            {
                canvas.scaleFactor = 0.5f;
            }*/
        }
        else
        {
            float scaleWidth = 1.0f / scaleHeight;

            Rect rect = _camera.rect;

            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;

            _camera.rect = rect;
            /*foreach (Canvas canvas in canvases)
            {
                canvas.scaleFactor = 0.5f;
            }*/
        }
    }
}
