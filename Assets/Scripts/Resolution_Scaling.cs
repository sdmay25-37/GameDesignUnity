using UnityEngine;

// Code heavilly based off of Ido Asraff's FitWorkgraoundToCamera tutorial
// 

public class Resolution_Scaling : MonoBehaviour {
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, 0);
        Vector3 bottomL = mainCamera.ViewportToWorldPoint(Vector3.zero) * 100;
        Vector3 topR = mainCamera.ViewportToWorldPoint(new Vector3(mainCamera.rect.width, mainCamera.rect.height) * 100);
        Vector3 screenSize = topR - bottomL;
        float screenRatio = screenSize.x / screenSize.y;
        // float desiredRatio = transform.localScale.x/ transform.localScale.y;
        float desiredRatio = 16f/9f;

        Debug.Log("Ratio: " + screenRatio + "Desired: " + desiredRatio);
        if (screenRatio > desiredRatio){
            float width = screenSize.x;
            transform.localScale = new Vector3(width, width/desiredRatio);
        }
        else{
            float height = screenSize.y;
            transform.localScale = new Vector3(height * desiredRatio, height);
        }
    }

}
