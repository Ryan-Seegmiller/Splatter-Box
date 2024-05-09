using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddPlayer : MonoBehaviour
{
    [SerializeField] public GameObject[] playerobjects = new GameObject[4];
    [SerializeField, NonReorderable] private GameObject[] players = new GameObject[4];
    [SerializeField, Audio] string addPlayer;
    [SerializeField, Audio] string removePlayer;
    public int playerCount = 0;

    bool addedPlayer = false;
    bool deletedPlayer = false;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponentInChildren<Image>().sprite = GameManager.instance.playerImages[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        for (int i = 0; i < playerCount; i++) { 
        
            var im = players[i].GetComponentInChildren<Image>();
            var img = playerobjects[i].GetComponentInChildren<SpriteRenderer>();
            im.sprite = img.sprite;

        }

        /*
        if (Input.GetKey(KeyCode.Return) && !addedPlayer)
        {
            AddUser();
        }
        else if(!Input.GetKey(KeyCode.Return))
        {
            addedPlayer = false;
        }
        if (Input.GetKeyDown(KeyCode.Backspace) && !deletedPlayer)
        {
            DeleteUser();
        }
        else if (!Input.GetKeyDown(KeyCode.Backspace))
        {
            deletedPlayer = false;
        }
        */
    }
    public void AddUser(GameObject p)
    {
        if (playerCount == players.Length) { return; }
        playerobjects[playerCount] = p;
        Vector3 endPos = players[playerCount].transform.position;//Stores the ned position to tween to
        //if(playerCount>0) players[playerCount].transform.position = players[playerCount-1].transform.position;//Sets the objects position to the object before it
        //LeanTween.move(players[playerCount], endPos, .2f);
        players[playerCount].SetActive(true);
        playerCount++;
        addedPlayer = true;
        GameManager.instance.audioSourceHelper.PlaySound(addPlayer, transform);
    }
    public void DeleteUser()
    {
        if(playerCount == 1) { return; }
        Vector3 startPos = players[--playerCount].transform.position;//Sets the start position to be reste once tweening is finished
        LeanTween.move(players[playerCount], players[playerCount - 1].transform.position, .2f);
        StartCoroutine(SetActiveFalse(.2f, players[playerCount], startPos));
        deletedPlayer = true;
        GameManager.instance.audioSourceHelper.PlaySound(removePlayer, transform);

    }
    IEnumerator SetActiveFalse(float time, GameObject obj, Vector3 startPos)
    {
        yield return new WaitForSeconds(time);
        obj.SetActive(false);
        obj.transform.position = startPos;
    }
}
