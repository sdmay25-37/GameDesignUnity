using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickHandler : MonoBehaviour, IPointerClickHandler
{

    private Button button;
    public System.Action LeftClickFunc;
    public System.Action RightClickFunc;

    private void Awake() {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() =>
            {
                LeftClickFunc?.Invoke();
            });
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            RightClickFunc?.Invoke();
        }
    }
}
