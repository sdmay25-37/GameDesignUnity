using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WitchController : NPCController
{
    private WITCHSTATE state;
    [SerializeField] private TextMeshProUGUI dialog; 

    // Start is called before the first frame update
    void Start()
    {
        state = WITCHSTATE.Idle;
    }

    override public void Interaction()
    {
        switch (state)
        { 
            case WITCHSTATE.Idle:
                popup.gameObject.SetActive(true);
                DialogIntro();
                state = WITCHSTATE.Talking;
                break;
            case WITCHSTATE.Talking:
                DialogMain();
                state = WITCHSTATE.Goodbye;
                break;
            case WITCHSTATE.Goodbye:
                popup.gameObject.SetActive(false);
                state = WITCHSTATE.Idle;
                break;
            default: 
                break;
        }
    }

    private void DialogIntro()
    {
        string message = "Hello there farmer.";
        StartCoroutine(SwitchText(message));
    }

    private void DialogMain()
    {
        string message = "Heading into the forest? I'd be careful if I were you, awful things lurk in the shadows.";
        StartCoroutine(SwitchText(message));
    }

    private IEnumerator SwitchText(string text)
    {
        for(int i = 1; i <= text.Length; i++)
        {
            dialog.SetText(text.Substring(0, i));
            yield return new WaitForSeconds(0.05f);
            if (i != text.Length && IsPunctuation(text.Substring(i-1, 1)))
            {
                yield return new WaitForSeconds(0.35f);
            }
        }
    }

    private bool IsPunctuation(string character)
    {
        if(character == "." || character == "," || character == "!" || character == "?")
        {
            return true;
        }
        return false;
    }

    enum WITCHSTATE
    {
        Idle,
        Talking,
        Goodbye
    }

}
