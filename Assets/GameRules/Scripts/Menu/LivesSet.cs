using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LivesSet : MonoBehaviour
{
    [SerializeField] private TMP_InputField livesText;
    // Start is called before the first frame update
    void OnEnable()
    {
        livesText.text = $"{GameManager.instance.maxLives}";
    }
    public void SetMaxLives()
    {
        GameManager.instance.maxLives = int.Parse(livesText.text);
        print(GameManager.instance.maxLives);
    }
}
