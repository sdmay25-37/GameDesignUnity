using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Slider volumeSlider; // Assign this in the Inspector

    void Start()
    {
        // Optionally, load a saved value
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 0.5f);

        // Add a listener to update the value dynamically
        volumeSlider.onValueChanged.AddListener(UpdateVolume);
    }

    void UpdateVolume(float value)
    {
        Debug.Log("Slider Value: " + value);
        // Example: Set audio volume

        // Save the setting
        PlayerPrefs.SetFloat("Volume", value);
        PlayerPrefs.Save();
    }
}
