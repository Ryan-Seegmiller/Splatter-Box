
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUIRefernce : MonoBehaviour
{
    [SerializeField, NonReorderable] public PlayerStatsUIValues[] _UIValues = new PlayerStatsUIValues[4];
    [SerializeField, Audio]private string endAudio;

    public void OnEnable()
    {
        //Depending on the game mode th ui will display differnet values
        GameModes currentMode = GameManager.instance.currentMode;
        int i = 0;
        foreach (PlayerStatsUIValues refValues in _UIValues)
        {
            switch (currentMode)
            {
                case GameModes.SuddenDeath:
                    refValues.deathsText.text = "1 : Life";
                    GameManager.instance.playUI.timer.gameObject.transform.parent.gameObject.SetActive(false);
                    break;
                case GameModes.Survival:
                    refValues.deathsText.text = GameManager.instance.maxLives + " : Lives";
                    _UIValues[i].lives = GameManager.instance.maxLives;
                    break;
                case GameModes.DeathMatch:
                    refValues.deathsText.text =  "0 : Deaths";
                    break;
            }
            _UIValues[i].dead = false;
            if(i > GameManager.instance.playerManagerScript.playerCount-1)
            {
                refValues.display.SetActive(false);
            }
            refValues.imageToTint.color = Color.green;
            _UIValues[i].playerIcon.sprite = GameManager.instance.playerImages[i++];
        }
    }
    void Update() {
        for (int index = 0; index < 4; index++) {

            PlayerStatsUIValues refValues = _UIValues[index];
            GameObject currentPlayer = GameManager.instance.playerManagerScript.players[index];

            if (GameManager.instance.playerManagerScript.playerCount > index)
            {
                SpriteRenderer img = currentPlayer.GetComponentInChildren<SpriteRenderer>();
                refValues.playerIcon.sprite = img.sprite;
            }

            _UIValues[index] = refValues;
        }
    }

   public void UpdateStats(int index, int lives)
   {

        GameModes currentMode = GameManager.instance.currentMode;
        PlayerStatsUIValues refValues = _UIValues[index];
        GameObject currentPlayer = GameManager.instance.playerManagerScript.players[index];


        switch (currentMode)
        {
            //Displays UI based on the different modes
            case GameModes.SuddenDeath:
                refValues.lives = lives;
                if(lives<=0) refValues.dead = true;
                break;
            case GameModes.Survival:
                refValues.lives = lives;
                refValues.deathsText.text = (int)refValues.lives + " : Lives";
                if (refValues.lives <= 0 || GameManager.instance.currentState == GameStates.SuddenDeath)
                {
                    print(GameManager.instance.currentState);
                    refValues.dead = true;
                }
                break;
            case GameModes.DeathMatch:
                refValues.lives += lives;
                refValues.deathsText.text = (int)refValues.lives + " : Deaths";
                break;
        }
        if (refValues.dead && currentMode != GameModes.DeathMatch && !(lives > 0))
        {
            refValues.imageToTint.color = Color.red;
            refValues.deathsText.text = "DEAD";

        }
        else
        {
            refValues.imageToTint.color = Color.green;
        }
        _UIValues[index] = refValues;
        if (GameOverCheck() && currentMode != GameModes.DeathMatch)
        {
            GameManager.instance.audioSourceHelper.PlaySound(endAudio, transform);
            GameManager.instance.SetNewState(GameStates.Menu);
        }
        
    }
    private bool GameOverCheck()
    {
        for(int i = 0; i < _UIValues.Length; i++)
        {
            if (_UIValues[i].display.activeSelf)
            {
                if (_UIValues[i].dead) { continue; }
                return false;
            }
        }
        return true;
    }
}

[Serializable]
public struct PlayerStatsUIValues
{
    [HideInInspector]public float lives;
    public TextMeshProUGUI deathsText;
    public Image playerIcon;
    public Image imageToTint;
    [HideInInspector]public bool dead;
    public GameObject display;
    
}
