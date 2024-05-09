using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{

    bool isDead = false;

    float invulnTimer = 0;
    float respawnTimer = 0;

    float invulnDuration = 3;
    float respawnDuration = 5;

    [SerializeField]
    private SpriteRenderer playerRenderer;

    [SerializeField]
    private ParticleSystem deathParticles;

    [SerializeField]
    private GameObject body;

    [SerializeField]
    private GameObject[] spawnPoints;
    private int spawnPointCount = 0;

    [SerializeField]
    private PlayerMovement movementscript;

    int playernum;
    public int lives;

    [SerializeField, Audio] private string playerDeathSound;
    [SerializeField, Audio] private string ReviveSound;



    // Start is called before the first frame update
    void Start()
    {

        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        spawnPointCount = spawnPoints.Length;

        lives = (int)GameManager.instance.maxLives;
        RevivePlayer();
        GameManager.instance.resetPlayers += ResetPlayerStats;

        PlayerManagerScript manager = GameObject.Find("PlayerManager").GetComponent<PlayerManagerScript>();
        manager.OnPlayerJoined(gameObject);
        playernum = manager.playerCount-1;

    }

    // Update is called once per frame
    void Update()
    {
        invulnTimer -= Time.deltaTime;

        if (isDead) {
            respawnTimer -= Time.deltaTime;
            if (respawnTimer < 0) RevivePlayer();
        }

    }
    public void ResetPlayerStats()
    {
        lives = (int)GameManager.instance.maxLives;
        RevivePlayer();
    }
    public void RevivePlayer() {
        GameModes currentMode = GameManager.instance.currentMode;
        GameStates currentState = GameManager.instance.currentState;

        if(((currentMode == GameModes.Survival || currentMode == GameModes.SuddenDeath) && lives <= 0) || GameStates.SuddenDeath == currentState) { return; }
        
        isDead = false;
        movementscript.canmove = true;
        invulnTimer = invulnDuration;
        movementscript.Teleport(spawnPoints[Random.Range(0, spawnPointCount)].transform.position);
        playerRenderer.enabled = true;

    }


    public void KillPlayer() {

        //already dead
        if (isDead) return;

        //invincible
        if (invulnTimer > 0) return;

        isDead = true;
        movementscript.canmove = false;
        respawnTimer = respawnDuration;
        playerRenderer.enabled = false;
        deathParticles.Play();
        movementscript.Teleport(new Vector3(0,1000,0));
        GameManager.instance.playerStatsUI.UpdateStats(playernum, (GameManager.instance.currentMode == GameModes.DeathMatch) ? lives+1 : lives-1);
        lives = (int)GameManager.instance.playerStatsUI._UIValues[playernum].lives;
        GameManager.instance.audioSourceHelper.PlaySound(playerDeathSound, transform);

    }

}
