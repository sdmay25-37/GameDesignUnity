using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScalingScript : MonoBehaviour
{
    public int PPU = 100;

    void Start()
    {
        AdjustCameraSize();
        
    }

    public void AdjustCameraSize(){
            float targetAspect = 16f / 9f;
            float windowAspect = (float)Screen.width / Screen.height;
            float scaleHeight = windowAspect / targetAspect;

            Camera camera = GetComponent<Camera>();

            float size = Screen.height / (2f * PPU);

            camera.orthographicSize = size;

            Debug.Log("Adjust Camera Size, size: " + size + "screen height: " + Screen.height + "PPU: " + PPU);

            if (scaleHeight < 1.0f)
            {
                // Add black bars on top & bottom
                Rect rect = camera.rect;
                rect.width = 1.0f;
                rect.height = scaleHeight;
                rect.x = 0;
                rect.y = (1.0f - scaleHeight) / 2.0f;
                camera.rect = rect;
            }
            else
            {
                // Add black bars on left & right
                float scaleWidth = 1.0f / scaleHeight;
                Rect rect = camera.rect;
                rect.width = scaleWidth;
                rect.height = 1.0f;
                rect.x = (1.0f - scaleWidth) / 2.0f;
                rect.y = 0;
                camera.rect = rect;
            }
        }

}
