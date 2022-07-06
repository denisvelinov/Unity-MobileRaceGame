using System;
using UnityEngine;

public class EnterCarSelect : MonoBehaviour
{
    public EventHandler OnEnterCarSelect;

    public void ToCarSelect()
    {
        OnEnterCarSelect?.Invoke(this, EventArgs.Empty);
    }
}
