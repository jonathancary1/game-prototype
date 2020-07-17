using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public TextMeshProUGUI Timer;

    public Slider HealthBar;

    public GameObject PlayButton;
    public GameObject QuitButton;

    public void SetTimer(float time)
    {
        Timer.text = TimeSpan.FromSeconds(time).ToString(@"mm\:ss\:fff");
    }

    public void SetHealthBar(float health)
    {
        HealthBar.value = health;
    }

    public void SetButtonsVisible(bool value)
    {
        PlayButton.SetActive(value);
        QuitButton.SetActive(value);
    }

    public void SetHealthBarVisible(bool value)
    {
        HealthBar.gameObject.SetActive(value);
    }
}
