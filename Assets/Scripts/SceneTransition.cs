using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private string targetScene; // Scene to transition to
    [SerializeField] private Vector3 spawnPosition; // Where the player spawns in the target scene
    [SerializeField] private GameObject blackout;

    [SerializeField] public AudioClip jumpSound; //Audio
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the trigger. Transitioning to the target scene...");
            PlayerPrefs.SetFloat("SpawnX", spawnPosition.x);
            PlayerPrefs.SetFloat("SpawnY", spawnPosition.y);
            PlayerPrefs.SetFloat("SpawnZ", spawnPosition.z);
            PlayerPrefs.Save();
            SoundManager.Instance.PlaySFX(jumpSound);
            StartCoroutine(FadeOutForest());
        }
    }

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    private void Update()
    {
        if(MainManager.Instance.died){
            StartCoroutine(Death());
            MainManager.Instance.died = false;
        }
    }

    public void PlayGame (){
        StartCoroutine(FadeOutForest());
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

}
