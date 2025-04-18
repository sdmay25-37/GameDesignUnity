using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GrandpaController : NPCController
{
    private bool talking, interacted;
    [SerializeField] private TextMeshProUGUI dialog;
    [SerializeField] private float timeBetweenLetters = 0.04f;
    [SerializeField] private float timeBetweenPunctuation = 0.25f;

    public override void Interaction()
    {
        if(interacted)
        {
            return;
        }
        interacted = true;

        MainFarmer.StopMovement();
        popup.gameObject.SetActive(true);
        StartCoroutine(SwitchText("You found me Grandson! We have to get out of here, there are worse things than the shadows in this forest"));
    }

    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(2);
        popup.gameObject.SetActive(false);
        SoundManager.Instance.PlaySFX(SoundManager.Instance.sounds.deathSound, 0.1f);
        yield return new WaitForSeconds(1);
        SoundManager.Instance.PlaySFX(SoundManager.Instance.sounds.deathSound, 0.4f);
        yield return new WaitForSeconds(1);
        SoundManager.Instance.PlaySFX(SoundManager.Instance.sounds.deathSound, 1f);
        StartCoroutine(SceneTransition.EndGame());
    }

    private IEnumerator SwitchText(string text)
    {
        talking = true;
        for (int i = 1; i <= text.Length; i++)
        {
            dialog.SetText(text.Substring(0, i));
            yield return new WaitForSeconds(0.05f);
            if (i != text.Length && IsPunctuation(text.Substring(i - 1, 1)))
            {
                yield return new WaitForSeconds(0.35f);
            }
        }
        talking = false;
        StartCoroutine(EndGame());
    }

    private bool IsPunctuation(string character)
    {
        if (character == "." || character == "," || character == "!" || character == "?")
        {
            return true;
        }
        return false;
    }
}
