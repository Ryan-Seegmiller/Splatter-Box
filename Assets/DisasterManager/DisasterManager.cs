using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Component that manages which and when disasters are active
/// </summary>
public class DisasterManager : MonoBehaviour
{
//-----------------------------------------------------------------------------
// public methods
//-----------------------------------------------------------------------------


    /// <summary>
    /// enables sudden death mode
    /// </summary>
    public void EnableSuddenDeath()
    {
        suddenDeathEnabled = true;
    }


//-----------------------------------------------------------------------------
// private members
//-----------------------------------------------------------------------------
    

    /// <summary>
    /// array of disaster prefabs that the DisasterManager can spawn
    /// </summary>
    [SerializeField] private DisasterInfo[] disasterPrefabs;

    /// <summary>
    /// the minimum amount of time that can elapse between disaster spawns
    /// </summary>
    [SerializeField] private float minTimeBetweenDisasters = 8.0f;
    
    /// <summary>
    /// the maximum amount of time that can elapse between disaster spawns
    /// </summary>
    [SerializeField] private float maxTimeBetweenDisasters = 12.0f;

    /// <summary>
    /// the length of time each disaster lasts
    /// </summary>
    [SerializeField] private float disasterLifetime = 10.0f;

    /// <summary>
    /// whether sudden death mode is enabled
    /// </summary>
    [SerializeField] private bool suddenDeathEnabled = false;


    /// <summary>
    /// how quickly things get worse in sudden death
    /// </summary>
    [SerializeField] private float suddenDeathSpeed = 0.99f;


    /// <summary>
    /// the size of the room the disasters are in
    /// </summary>
    [SerializeField] private Vector2 roomSize;

    /// <summary>
    /// the center of the active disasters UI bar
    /// </summary>
    [SerializeField] private Vector2 activeDisastersUiPosition;

    /// <summary>
    /// the horizontal distance between the center of each active disaster icon
    /// </summary>
    [SerializeField] private float activeDisastersUiDistance;

    /// <summary>
    /// the height at which an active disaster icon is offscreen
    /// </summary>
    [SerializeField] private float activeDisasterUiOffscreenHeight;


    /// <summary>
    /// how quickly the disaster icons move when a new icon is added 
    /// </summary>
    [SerializeField] private float activeDisasterIconsTransitionSpeed;

    /// <summary>
    /// how long before an icon is removed does the transition start
    /// </summary>
    [SerializeField] private float activeDisasterIconsTransitionTime;


    /// <summary>
    /// prefab for active disaster icons
    /// </summary>
    [SerializeField] private GameObject activeDisasterIconPrefab;

    /// <summary>
    /// icon to display what the next disaster is
    /// </summary>
    [SerializeField] private Image nextDisasterIconUi;

    /// <summary>
    /// the canvas to manipulate UI on
    /// </summary>
    [SerializeField] private Transform uiCanvas;

    /// <summary>
    /// the sound to play when a new disaster spawns
    /// </summary>
    [SerializeField, Audio] private string disasterSpawnSound;


    /// <summary>
    /// the amount of time between the previous disaster and the next
    /// </summary>
    private float timeBetweenDisasters = 0.0f;


    /// <summary>
    /// the amount of time until the next disaster spawns
    /// </summary>
    private float timeUntilNextDisaster = 0.0f;

    /// <summary>
    /// the index of the next disaster to spawn
    /// TODO: replace with voting system
    /// </summary>
    private int nextDisasterIndex = -1;


    /// <summary>
    /// all of the currently active disasters
    /// </summary>
    private List< Disaster > activeDisasters = new List< Disaster >();

    /// <summary>
    /// UI icons for all currently active disasters
    /// </summary>
    private List< Image > activeDisasterIcons = new List< Image >();


//-----------------------------------------------------------------------------
// engine methods
//-----------------------------------------------------------------------------

    /// <summary>
    /// called when this behavior enters the scene
    /// </summary>
    void Start()
    {
        queueNextDisaster();
    }

    /// <summary>
    /// called each simulation frame
    /// </summary>
    void FixedUpdate()
    {
        updateActiveDisasters();

        if ( GameManager.instance.currentState == GameStates.Menu )
        {
            gameObject.SetActive( false );
            return;
        }

        timeUntilNextDisaster -= Time.deltaTime;
        if ( timeUntilNextDisaster <= 0.0f )
        {
            startNextDisaster();
            queueNextDisaster();
        }

        if ( suddenDeathEnabled )
        {
            accelerateDisasterSpawns();
        }

        updateUi();
    }


//-----------------------------------------------------------------------------
// private methods
//-----------------------------------------------------------------------------

    private void accelerateDisasterSpawns()
    {
        minTimeBetweenDisasters -= Time.deltaTime * suddenDeathSpeed * minTimeBetweenDisasters;
        maxTimeBetweenDisasters -= Time.deltaTime * suddenDeathSpeed * maxTimeBetweenDisasters;
    }


    /// <summary>
    /// queues up a disaster to be spawned, and resets the timer for the next disaster
    /// </summary>
    private void queueNextDisaster()
    {
        timeBetweenDisasters = Random.Range( minTimeBetweenDisasters, maxTimeBetweenDisasters );
        timeUntilNextDisaster += timeBetweenDisasters;

        nextDisasterIndex = Random.Range( 0, disasterPrefabs.Length );

        nextDisasterIconUi.material.mainTexture = disasterPrefabs[ nextDisasterIndex ].disasterIcon;
    }

    /// <summary>
    /// spawns the next disaster
    /// </summary>
    private void startNextDisaster()
    {
        GameObject disasterObject = (GameObject)Instantiate( disasterPrefabs[ nextDisasterIndex ].disasterPrefab );
        Disaster disaster = disasterObject.GetComponent< Disaster >();
        disaster.lifeTime = disasterLifetime;
        disaster.lifeTimer = disasterLifetime;
        disaster.roomSize = roomSize;
        activeDisasters.Add( disaster );

        Image icon = Instantiate( activeDisasterIconPrefab ).GetComponent< Image >();
        icon.rectTransform.anchoredPosition = nextDisasterIconUi.rectTransform.anchoredPosition;
        icon.material = Instantiate( icon.material ); // copy the material to prevent global changes
        icon.material.mainTexture = disasterPrefabs[ nextDisasterIndex ].disasterIcon;
        icon.transform.SetParent( uiCanvas, false );
        activeDisasterIcons.Add( icon );

        GameManager.instance.audioSourceHelper.PlaySound( disasterSpawnSound, transform );
    }

    /// <summary>
    /// updates the ui displaying how long until the next disaster
    /// </summary>
    private void updateUi()
    {
        nextDisasterIconUi.material.SetFloat( "_Proportion", 1.0f - timeUntilNextDisaster / timeBetweenDisasters );

        adjustActiveDisasterIconPositions();
    }

    /// <summary>
    /// adds an active disaster icon to the UI
    /// </summary>
    private void adjustActiveDisasterIconPositions()
    {
        if ( activeDisasters.Count == 0 )
        {
            return;
        }

        float timeUntilNextDeparture = Mathf.Clamp( 1.0f - activeDisasters[ 0 ].lifeTimer / activeDisasterIconsTransitionTime, 0.0f, 1.0f );

        for ( int i = 0; i < activeDisasters.Count; ++i )
        {

            float moveOutAnimation = Mathf.Clamp( 1.0f - activeDisasters[ i ].lifeTimer / activeDisasterIconsTransitionTime, 0.0f, 1.0f );

            Vector2 targetPos;
            Vector2 currentPos = activeDisasterIcons[ i ].rectTransform.anchoredPosition;

            targetPos.y = activeDisastersUiPosition.y;
            targetPos.y += moveOutAnimation * activeDisasterUiOffscreenHeight;

            if ( moveOutAnimation > 0 )
            {
                targetPos.x = currentPos.x;
            }
            else
            {
                targetPos.x = activeDisastersUiPosition.x + i * activeDisastersUiDistance;
                targetPos.x -= (activeDisasters.Count - 1.0f) * activeDisastersUiDistance * 0.5f;
                targetPos.x -= timeUntilNextDeparture * activeDisastersUiDistance * 0.5f;
            }

            currentPos += (targetPos - currentPos) * activeDisasterIconsTransitionSpeed * Time.deltaTime;

            activeDisasterIcons[ i ].rectTransform.anchoredPosition = currentPos;
        }
    }


    /// <summary>
    /// updates all active disasters and their UI
    /// </summary>
    private void updateActiveDisasters()
    {
        if ( activeDisasters.Count > 0 && activeDisasters[ 0 ] == null )
        {
            Destroy( activeDisasterIcons[ 0 ].gameObject );

            activeDisasters.RemoveAt( 0 );
            activeDisasterIcons.RemoveAt( 0 );
        }

        for ( int i = 0; i < activeDisasters.Count; ++i )
        {
            float proportion = activeDisasters[ i ].lifeTimer / activeDisasters[ i ].lifeTime;
            activeDisasterIcons[ i ].material.SetFloat( "_Proportion", proportion );
        }
    }


//-----------------------------------------------------------------------------
}
