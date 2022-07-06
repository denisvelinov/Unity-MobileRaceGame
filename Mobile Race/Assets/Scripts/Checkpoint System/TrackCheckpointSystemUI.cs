using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCheckpointSystemUI : MonoBehaviour
{
    [SerializeField] private TrackTimeTrialSystem trackCheckpoints;
    [SerializeField] private EndRaceButtonUI endRace;

    private void Start()
    {
        trackCheckpoints.OnPlayerCorrectCheckpoint += TrackCheckpointSystem_OnPlayerCorrectCheckpoint;
        trackCheckpoints.OnPlayerWrongCheckpoint += TrackCheckpointSystem_OnPlayerWrongCheckpoint;
        endRace.OnRaceEnd += EndRace_OnRaceEnd;

        Hide();
    }

    private void TrackCheckpointSystem_OnPlayerCorrectCheckpoint(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void TrackCheckpointSystem_OnPlayerWrongCheckpoint(object sender, System.EventArgs e)
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
