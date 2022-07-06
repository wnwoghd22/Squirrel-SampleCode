using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageUI : MonoBehaviour
{
    [SerializeField] private GameObject ingameUI;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject menuButton;

    [SerializeField] private GameOverUI clearScrollUI;
    [SerializeField] private GameOverUI gameOverScrollUI;

    [SerializeField] private Slider progressBar;
    public Slider ProgressBar => progressBar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowInGameUI()
    {
        clearScrollUI.gameObject.SetActive(false);
        gameOverScrollUI.gameObject.SetActive(false);
        CloseMenu();
        ingameUI.SetActive(true);
    }
    public void CloseInGameUI() => ingameUI.SetActive(false);

    public void OpenMenu()
    {
        menuButton.SetActive(false);
        menu.SetActive(true);
    }
    public void CloseMenu()
    {
        menuButton.SetActive(true);
        menu.SetActive(false);
    }

    public void ShowClearUI(int acorns, int chapter, int endingItems, bool seasonUnLock = false)
    {
        clearScrollUI.SetValues(acorns, chapter, endingItems, seasonUnLock);
        clearScrollUI.gameObject.SetActive(true);
    }
    public void ShowGameOverUI(int acorns, int chapter, int endingItems, bool seasonUnLock = false)
    {
        gameOverScrollUI.SetValues(acorns, chapter, endingItems, seasonUnLock);
        gameOverScrollUI.gameObject.SetActive(true);
    }
}
