using System;
using UnityEngine;

public class EnterTrackSelect : MonoBehaviour
{
    public EventHandler OnEnterTrackSelect;

    public void ToTrackSelect()
    {
        OnEnterTrackSelect?.Invoke(this, EventArgs.Empty);
    }
}
