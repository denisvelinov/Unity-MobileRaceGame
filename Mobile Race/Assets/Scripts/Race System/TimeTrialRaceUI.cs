using System;
using UnityEngine;

public class TimeTrialRaceUI : MonoBehaviour
{
    [SerializeField] private StartRaceButtonUI startRace;
    [SerializeField] private EndRaceButtonUI endRace;

    void Start()
    {
        startRace.OnRaceStart += StartRace_OnRaceStart;
        endRace.OnRaceEnd += EndRace_OnRaceEnd;

        Hide();
    }

    private void StartRace_OnRaceStart(object sender, EventArgs e)
    {
        Show();
    }

    private void EndRace_OnRaceEnd(object sender, System.EventArgs e)
    {
        Hide();
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
