using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using TMPro;
using UnityEngine;

public class MessagePopup : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject textMeshPrefab;
    private Queue<TextMeshProUGUI> messages = new Queue<TextMeshProUGUI>();

    public void SendPopupMessage(string message)
    {
        GameObject textObj = Instantiate(textMeshPrefab, canvas.transform);
        TextMeshProUGUI text = textObj.GetComponent<TextMeshProUGUI>();
        text.text = message;
        messages.Enqueue(text);
        StartCoroutine(FadeOut(text));
        ManageQueue();
    }

    public void SendPopupMessage(string message, Color color)
    {
        GameObject textObj = Instantiate(textMeshPrefab, canvas.transform);
        TextMeshProUGUI text = textObj.GetComponent<TextMeshProUGUI>();
        text.text = message;
        text.color = color;
        messages.Enqueue(text);
        StartCoroutine(FadeOut(text));
        ManageQueue();
    }

    private IEnumerator FadeOut(TextMeshProUGUI obj)
    {
        yield return new WaitForSeconds(2f);
        if(obj != null && obj.color.a > 0)
        {
            obj.CrossFadeAlpha(0, 0.8f * obj.color.a, false);
        }
        yield return new WaitUntil(() => obj == null ||  obj.color.a == 0);
        if(obj != null && messages.Contains(obj))
        {
            messages.Dequeue();
            Destroy(obj.gameObject);
        }
    }

    private void ManageQueue()
    {
        if(messages.Count > 4)
        {
            Destroy(messages.Dequeue().gameObject);
        }

        TextMeshProUGUI[] mess = messages.ToArray();
        for (int i = messages.Count - 2; i >= 0; i--)
        {
            mess[i].transform.Translate(0f, 0.3f, 0f);
            mess[i].fontSize = 0.2f;
        }
    }
}
