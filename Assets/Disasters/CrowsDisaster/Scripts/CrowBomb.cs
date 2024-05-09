using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowBomb : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float explosionLength;


    void OnCollisionEnter2D( Collision2D collision )
    {
        GameObject explosion = Instantiate( explosionPrefab, transform.position, Quaternion.identity );
        Destroy( explosion, explosionLength );
        Destroy( gameObject );
    }
}
