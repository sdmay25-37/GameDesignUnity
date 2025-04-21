using System;
using System.Collections;
using TMPro;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private string targetScene; // Scene to transition to
    [SerializeField] private Vector3 spawnPosition; // Where the player spawns in the target scene
    [SerializeField] private GameObject blackout;
    private static GameObject blackoutBox;
    private Boolean transitioning = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the trigger. Transitioning to the target scene...");
            PlayerPrefs.SetFloat("SpawnX", spawnPosition.x);
            PlayerPrefs.SetFloat("SpawnY", spawnPosition.y);
            PlayerPrefs.SetFloat("SpawnZ", spawnPosition.z);
            PlayerPrefs.Save();
            StartCoroutine(FadeOutForest());
        }
    }

    private void Start()
    {
        blackoutBox = blackout;
        StartCoroutine(FadeIn());
    }

    private void Update()
    {
        if(MainManager.Instance.died && !transitioning){
            StartCoroutine(Death());
            //MainManager.Instance.died = false; Moved to the end of the death coroutine
        }
    }

    public void PlayGame (){
        StartCoroutine(StartGame());
    }

    public void QuitGame(){
        Debug.Log("Quit");
        Application.Quit();
    }

    private IEnumerator FadeOutStart()
    {
        GameObject blackbox = Instantiate(blackout);
        Image fadebox = blackbox.GetComponentInChildren<Image>();
        Color color = new Color(0, 0, 0, 0);
        while (color.a < 1f)
        {
            fadebox.color = color;
            color.a += 0.01f;
            yield return null;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private IEnumerator FadeOutForest()
    {
        GameObject blackbox = Instantiate(blackout);
        Image fadebox = blackbox.GetComponentInChildren<Image>();
        Color color = new Color(0, 0, 0, 0);
        while (color.a < 1f)
        {
            fadebox.color = color;
            color.a += 0.01f;
            yield return null;
        }
        SceneManager.LoadScene(targetScene);
    }


    private IEnumerator StartGame()
    {
        GameObject blackbox = Instantiate(blackout);
        Image fadebox = blackbox.GetComponentInChildren<Image>();
        Color color = new Color(0, 0, 0, 0);
        while (color.a < 1f)
        {
            fadebox.color = color;
            color.a += 0.01f;
            yield return null;
        }
        SceneManager.LoadScene(1);
    }

    private IEnumerator FadeIn()
    {
        GameObject blackbox = Instantiate(blackout);
        Image fadebox = blackbox.GetComponentInChildren<Image>();
        Color color = new Color(0, 0, 0, 1.0f);
        while (color.a > 0f)
        {
            fadebox.color = color;
            color.a -= 0.01f;
            yield return null;
        }
        Destroy(blackbox);
    }

    private IEnumerator Death(){
        transitioning = true;
        SoundManager.Instance.PlaySFX(SoundManager.Instance.sounds.deathSound);
        GameObject blackbox = Instantiate(blackout);
        Image fadebox = blackbox.GetComponentInChildren<Image>();
        Animator cutscene = blackbox.GetComponentInChildren<Animator>();
        Color color = new Color(0, 0, 0, 0);
        while (color.a < 1f)
        {
            fadebox.color = color;
            color.a += 0.01f;
            yield return null;
        }
        yield return new WaitForSeconds(1);
        cutscene.SetTrigger("Animate");
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => cutscene.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99);

        MainManager.Instance.died = false;
        transitioning = false;
        SceneManager.LoadScene(1);
    }

    public static IEnumerator EndGame()
    {
        GameObject blackbox = Instantiate(blackoutBox);
        Image fadebox = blackbox.GetComponentInChildren<Image>();
        TextMeshProUGUI text = blackbox.GetComponentInChildren<TextMeshProUGUI>();
        Color colorText = new Color(1.0f, 1.0f, 1.0f, 0);
        Color color = new Color(0, 0, 0, 0);
        while (color.a < 1f)
        {
            fadebox.color = color;
            color.a += 0.005f;
            yield return null;
        }
        while (colorText.a < 1f)
        {
            text.color = colorText;
            colorText.a += 0.002f;
            yield return null;
        }
        yield return new WaitForSeconds(5);
        MainFarmer.StartMovement();
        SceneManager.LoadScene("startMenu");
    }
}
