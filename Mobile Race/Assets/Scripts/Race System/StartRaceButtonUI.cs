using System;
using UnityEngine;

public class StartRaceButtonUI : MonoBehaviour
{
    public event EventHandler OnRaceStart;

    public void OnStart()
    {
        OnRaceStart?.Invoke(this, EventArgs.Empty);
    }
}
