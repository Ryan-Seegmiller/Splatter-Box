using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRules : MonoBehaviour
{
    public GameObject rules;
    public GameModes gameMode;
    public static GameObject currentActive;
    private void OnEnable()
    {
        
        if(gameMode == GameManager.instance.currentMode)
        {
            rules.SetActive(true);
            currentActive = rules;
        }
    }
    public void SetGameMode()
    {
        GameManager.instance.SetGameMode(gameMode);
        currentActive.SetActive(false);
        rules.SetActive(true);
        currentActive = rules;
    }
    
}
