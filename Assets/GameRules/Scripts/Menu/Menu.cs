using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField, Audio] private string clickSound;
    [SerializeField, Audio] private string hoverSound;
    [SerializeField, Audio] private string hoverSound2;
    [SerializeField, Audio] private string closeWindowSound;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject helpPopup;
    [SerializeField] private GameObject gameRulesPopup;

    [SerializeField, NonReorderable] private PlayerGuideText[] descriptions;
    public void Start()
    {
        //Sets the text
        foreach(var desc in descriptions)
        {
            desc.SetText();
        }
        helpPopup.SetActive(false);
    }
    public void PlaySoundClick()
    {
        GameManager.GetInstance().audioSourceHelper.PlaySound(clickSound, transform);
    }
    public void PlayHoverSound()
    {
        GameManager.GetInstance().audioSourceHelper.PlaySound(hoverSound, transform);
    } public void PlayHoverSound2()
    {
        GameManager.GetInstance().audioSourceHelper.PlaySound(hoverSound2, transform);
    }
    public void PlayCloseSound()
    {
        GameManager.GetInstance().audioSourceHelper.PlaySound(closeWindowSound, transform);
    }
    public void HelpPopup()
    {
        menu.SetActive(!menu.activeSelf);

        TweenPopup(helpPopup);

    }
    public void GameRulesPopup()
    {
        menu.SetActive(!menu.activeSelf);

        TweenPopup(gameRulesPopup);

    }
    private void Update()
    {
        ExitPopup();
    }
    public void ExitPopup()
    {
        if (Input.GetKey(KeyCode.Escape) && !menu.activeSelf)
        {
            if (helpPopup.activeSelf)
            {
                HelpPopup();
            }
            else if(gameRulesPopup.activeSelf)
            {
                GameRulesPopup();
            }
        }
    }
    public void TweenPopup(GameObject obj)
    {
        //Animates the poup window
        if (obj.activeSelf)
        {
            obj.transform.localScale = Vector3.one;
            StartCoroutine(HelpPopupDisable(.2f, obj));
            LeanTween.scale(obj, Vector3.zero, .2f);
            PlayCloseSound();
        }
        else
        {
            obj.transform.localScale = Vector3.zero;
            obj.SetActive(true);
            LeanTween.scale(obj, Vector3.one, .2f);
            PlaySoundClick();
        }
    }
    IEnumerator HelpPopupDisable(float time, GameObject obj)
    {
        yield return new WaitForSeconds(time);
        obj.SetActive(false);
    }
    public void StartPlay()
    {
        if (GameManager.instance.playerManagerScript.playerCount == 0) { return; }
        PlaySoundClick();
        GameManager.instance.SetNewState(GameStates.Play);//Make sure this is called last
    }
}
[Serializable]
public struct PlayerGuideText
{
    public string title; 
    [TextArea]public string description;
    public GameObject textHolder;

    //Sets the text of the gameobjects
    public void SetText()
    {
        TextMeshProUGUI[] textBoxes = textHolder.GetComponentsInChildren<TextMeshProUGUI>();
        textBoxes[0].text = title;
        textBoxes[1].text = description;
    }
}
[Serializable]
public struct EditableTimers
{
    [HideInInspector]public float timeInMinutes;
    public Vector2 minMaxTimeInMinutes;
    public string displayText;
}