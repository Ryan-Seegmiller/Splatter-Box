using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PlayerManagerScript : MonoBehaviour
{

    [SerializeField]
    public AddPlayer addplayerscript;

    public GameObject[] players = new GameObject[4];
    public int playerCount = 0;

    public void OnPlayerJoined(GameObject p)
    {
        players[playerCount] = p;
        addplayerscript.AddUser(p);
        playerCount++;
    }
    public void OnPlayerLeft()
    {
        addplayerscript.DeleteUser();
        playerCount--;
    }
   
}
