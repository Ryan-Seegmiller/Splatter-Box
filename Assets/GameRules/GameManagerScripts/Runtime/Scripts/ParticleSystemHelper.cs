using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class ParticleSystemHelper : MonoBehaviour
{
    public static ParticleSystemHelper instance;

     public ParticleSystemReference[] psRefs => GameManager.GetInstance().particleSystemReferences;

    private ObjectPool<ParticleSystem>[] particlePools;

    private int currentRefIndex;//Stores refernce for which particle to use
    private int currentParticleIndex;//Stores the index for which particle to use
    private Transform currentParent;//Stores the current parent transform so the particle goes to the right location

    public void GetInstance()
    {
        //Singleton pattern
        if (instance == null)
        {
            instance = this;
            CreatePools();
        }
        else if (instance != this)
        {   
            Destroy(gameObject);
        }
    }
    //Creates the object pools that are needed for the particle systems
    private void CreatePools()
    {
        int particlePoolIndex = 0;
        int totalParticleCount = 0;
        if(psRefs == null) { return; }
        for(int i = 0; i < psRefs.Length; i++)
        {
            totalParticleCount += psRefs[i].particles.Length;
        }
        particlePools = new ObjectPool<ParticleSystem>[totalParticleCount];

        for (int i = 0; i < psRefs.Length; i++)
        {
            {
                //Creates a new object pool for each particle system
                for (int j = 0; j < psRefs[i].particles.Length; j++)
                {
                    particlePools[particlePoolIndex++] = new ObjectPool<ParticleSystem>(CreatePoolObject, OnTakeFromPool, OnReturnToPool, OnDestroyObject, false, psRefs[i].particles[j].amount, 100_000);
                }
            }
        }
       
    }
    //Creates object for the object pool
    private ParticleSystem CreatePoolObject()
    {
        ParticleSystem instance = Instantiate(psRefs[currentRefIndex].particles[currentParticleIndex].particle, Vector3.zero, Quaternion.identity);
        instance.gameObject.SetActive(false);

        return instance;
    }
    //Returns the object to the pool
    private void ReturnObjectToPool(ParticleSystem instance, int index)
    {
        particlePools[index].Release(instance);
    }

    //Whenever get is called
    private void OnTakeFromPool(ParticleSystem instance)
    {
        instance.gameObject.SetActive(true);
        instance.transform.SetParent(currentParent, true);
    }
    //Whenever the object returns to the object pool
    private void OnReturnToPool(ParticleSystem instance)
    {
        instance.transform.parent = GameManager.GetInstance().particleSystemHelper.gameObject.transform;
        instance.gameObject.SetActive(false);
    }
    //In case the object needs to be destroyed
    private void OnDestroyObject(ParticleSystem instance)
    {
        Destroy(instance.gameObject);
    }

    //To be called from the any script to have a particle effect play
    public void Play(Transform tr, string name, float duration)
    {
        int particlePoolIndex = 0;
        for (int i = 0; i < psRefs.Length; i++)
        {
            for(int j = 0;j < psRefs.Length; j++)
            {
                //Checks reference values
                if (psRefs[i].particles[j].name == name) 
                {
                    //Sets the index and parent values for the object to be created in the correct loctaion
                    currentParticleIndex = j;
                    currentRefIndex = i;
                    currentParent = tr;

                    

                    //Handles the particle system behaviours
                    ParticleSystem newPs = particlePools[particlePoolIndex].Get();
                    newPs.transform.position = tr.position;
                    var psMain = newPs.main;
                    psMain.duration = duration;
                    newPs.Play();

                    StartCoroutine(ReturnParticle(newPs, duration, i));
                    break;
                   
                } 
                particlePoolIndex++;
            }
            
        }
    }
    //Timer for the particles to return
    private IEnumerator ReturnParticle(ParticleSystem instance, float duration, int index)
    {
        yield return new WaitForSeconds(duration);
        ReturnObjectToPool (instance, index);
    }
}
