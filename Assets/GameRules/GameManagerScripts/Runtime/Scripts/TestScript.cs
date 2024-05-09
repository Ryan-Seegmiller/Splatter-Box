using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField, Audio] private string audio1;

   

    [SerializeField, Particle] private string particleToPlayOnSam;

    
    public void PlaySound()
    {
        GameManager.GetInstance().audioSourceHelper.PlaySound(audio1, transform);
    }
    public void PlayParticle()
    {
        GameManager.GetInstance().particleSystemHelper.Play(transform, particleToPlayOnSam, 5);
    }
}
