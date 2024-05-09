using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAttribute : BaseGameManagerAttribute
{
    public override object[] objectArray { get => GameManager.GetInstance().particleSystemReferences; }

    public ParticleAttribute()
    {

    }
}
