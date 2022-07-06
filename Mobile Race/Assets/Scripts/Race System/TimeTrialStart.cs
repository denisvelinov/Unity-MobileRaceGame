using System;
using UnityEngine;

public class TimeTrialStart : MonoBehaviour
{
    public event EventHandler OnPlayerEnterPromptBubble;
    public event EventHandler OnPlayerExitPromptBubble;

    [SerializeField] private StartRaceButtonUI startRace;
    [SerializeField] private CloseTimeTableButtonUI closeTable;

    private void Start()
    {
        startRace.OnRaceStart += StartRace_OnRaceStart;
        closeTable.OnTimeTableClose += ClosseTable_OnTimeTableClose;

        Show();
    }

    private void StartRace_OnRaceStart(object sender, System.EventArgs e)
    {
        Hide();
    }
    
    private void ClosseTable_OnTimeTableClose(object sender, System.EventArgs e)
    {
        Show();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "ColliderBottom")
        {
            OnPlayerEnterPromptBubble?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "ColliderBottom")
        {
            OnPlayerExitPromptBubble?.Invoke(this, EventArgs.Empty);
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
