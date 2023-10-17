using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject loseUI;
    
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI loseTimeText;
    
    private float currentTime;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        else instance = this;

        Time.timeScale = 1;
        
        gameUI.SetActive(true);
        loseUI.SetActive(false);
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        timerText.text = ((int)currentTime).ToString();
    }

    public void LoadLoseUI()
    {
        Time.timeScale = 0;
        loseTimeText.text = currentTime.ToString(CultureInfo.InvariantCulture);
        
        gameUI.SetActive(false);
        loseUI.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
