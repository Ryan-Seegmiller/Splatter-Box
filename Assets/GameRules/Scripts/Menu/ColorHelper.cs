using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorHelper : MonoBehaviour
{
    public int index;
    [SerializeField, Audio] private string clickSound;
    //Searches through children to get the index
    private void Start()
    {
        GetComponent<Image>().sprite = GameManager.GetInstance().playerImages[index];
    }
    public void SetColor()
    {
        GameManager.GetInstance().audioSourceHelper.PlaySound(clickSound, transform);
    }
}
