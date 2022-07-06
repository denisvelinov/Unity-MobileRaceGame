using System;
using UnityEngine;

public class BackToMainMenu : MonoBehaviour
{
    public EventHandler OnExitTrackSelect;

    public void ToMainMenu()
    {
        OnExitTrackSelect?.Invoke(this, EventArgs.Empty);
    }
}
