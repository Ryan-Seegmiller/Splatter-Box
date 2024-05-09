using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SuddenDeath : MonoBehaviour
{
    Vector3 startScale;
    private void Start()
    {
        startScale = transform.localScale;
        SuddenDeathPopup();
    }
    public void SuddenDeathPopup()
    {
        transform.localScale = Vector3.zero;
        LeanTween.scale(gameObject, startScale, 1f);
        LeanTween.value(gameObject, GetComponent<TextMeshProUGUI>().color.a,0,1);
        LeanTween.alphaVertex(gameObject, 0, 1);
    }
}
