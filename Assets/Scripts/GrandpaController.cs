using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GrandpaController : NPCController
{
    private GRANDPASTATE state;
    [SerializeField] private TextMeshProUGUI dialog;

    void Start()
    {
        state = GRANDPASTATE.Idle;
    }

    override public void Interaction()
    {
        switch (state)
        {
            case GRANDPASTATE.Idle:
                popup.gameObject.SetActive(true);
                DialogIntro();
                state = GRANDPASTATE.Talking;
                break;
            case GRANDPASTATE.Talking:
                DialogMain();
                state = GRANDPASTATE.Goodbye;
                break;
            case GRANDPASTATE.Goodbye:
                popup.gameObject.SetActive(false);
                state = GRANDPASTATE.Idle;
                break;
            default:
                break;
        }
    }

    private void DialogIntro()
    {
        string message = "Grandson is that you? I didn't think you'd find me out here.";
        StartCoroutine(SwitchText(message));
    }

    private void DialogMain()
    {
        string message = "I don't have time to explain, but you are the only one that can defeat the shadows. To be continued....";
        StartCoroutine(SwitchText(message));
    }

    private IEnumerator SwitchText(string text)
    {
        for (int i = 1; i <= text.Length; i++)
        {
            dialog.SetText(text.Substring(0, i));
            yield return new WaitForSeconds(0.05f);
            if (i != text.Length && IsPunctuation(text.Substring(i - 1, 1)))
            {
                yield return new WaitForSeconds(0.35f);
            }
        }
    }

    private bool IsPunctuation(string character)
    {
        return character == "." || character == "," || character == "!" || character == "?";
    }

    enum GRANDPASTATE
    {
        Idle,
        Talking,
        Goodbye
    }
}
