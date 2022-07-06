using System;
using UnityEngine;

public class BackToTrackSelect : MonoBehaviour
{
    public EventHandler OnExitCarSelect;

    public void ToTrackSelect() 
    {
        OnExitCarSelect?.Invoke(this, EventArgs.Empty);
    }
    
}
