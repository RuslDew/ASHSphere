using UnityEngine;
using System;

public class SaveButton : MonoBehaviour
{
    public Session TargetSession { get; private set; }

    public Action OnClick; 
    public Action OnClickRemove; 

    public void Init()
    {

    }
}
