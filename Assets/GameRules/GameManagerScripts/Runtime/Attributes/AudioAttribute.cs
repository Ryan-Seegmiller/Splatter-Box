using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System;
using System.Linq;

public class AudioAttribute : BaseGameManagerAttribute
{
    public override object[] objectArray { get => GameManager.GetInstance().audioReferneces; }

    public AudioAttribute() {
        
        
    }
    

}
