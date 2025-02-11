using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Hover : MonoBehaviour
{

    private RectTransform transf;
    private Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        transf = GetComponent<RectTransform>();
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transf.position = pos + new Vector3(0, Mathf.Sin(Time.time), 0);
    }
}
