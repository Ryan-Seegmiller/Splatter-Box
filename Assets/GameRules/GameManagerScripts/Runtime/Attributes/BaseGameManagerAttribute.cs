using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseGameManagerAttribute : PropertyAttribute
{
    public abstract object[] objectArray { get; }
}
