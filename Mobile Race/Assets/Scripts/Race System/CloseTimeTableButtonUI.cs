using System;
using UnityEngine;

public class CloseTimeTableButtonUI : MonoBehaviour
{
    public event EventHandler OnTimeTableClose;

    public void OnTableClose()
    {
        OnTimeTableClose?.Invoke(this, EventArgs.Empty);
    }
}
