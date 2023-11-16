using System;
using UnityEngine;

public class Session : MonoBehaviour
{
    public DateTime SessionStartDate { get; private set; }


    public Session(DateTime startDate)
    {
        SessionStartDate = startDate;
    }
}
