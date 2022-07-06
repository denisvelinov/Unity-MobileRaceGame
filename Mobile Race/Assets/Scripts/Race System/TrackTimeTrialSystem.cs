using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackTimeTrialSystem : MonoBehaviour
{
    public event EventHandler OnPlayerCorrectCheckpoint;
    public event EventHandler OnPlayerWrongCheckpoint;
    public event EventHandler OnPlayerCompleteLap;

    [SerializeField] private StartRaceButtonUI startRace;
    [SerializeField] private EndRaceButtonUI endRace;
    [SerializeField] private ResetButton resetButton;

    private List<SingleCheckpoint> singleCheckpointList;
    private int nextSingleCheckpointIndex;
    [SerializeField]private int checkpointCounter;

    private void Awake()
    {
        ResetCheckpoints();
    }

    private void Start()
    {
        startRace.OnRaceStart += StartRace_OnRaceStart;
        endRace.OnRaceEnd += EndRace_OnRaceEnd;

        Hide();
    }
    private void StartRace_OnRaceStart(object sender, System.EventArgs e)
    {
        ResetPositionValues.resetPosition = GameObject.Find("Checkpoint (5)").transform.position;
        ResetPositionValues.resetRotation = GameObject.Find("Checkpoint (5)").transform.rotation;

        resetButton.ResetPosition();
        checkpointCounter = 0;

        Show();
    }
    private void EndRace_OnRaceEnd(object sender, System.EventArgs e)
    {
        ResetPositionValues.resetPosition = GameObject.Find("Checkpoint (5)").transform.position;
        ResetPositionValues.resetRotation = GameObject.Find("Checkpoint (5)").transform.rotation;

        resetButton.ResetPosition();

        HighlightCorrectCheckpoint("hide");

        nextSingleCheckpointIndex = 0;
        checkpointCounter = 0;
        
        Hide();
    }

    public void PlayerTroughCheckpoint(SingleCheckpoint singleCheckpoint)
    {
        if (singleCheckpointList.IndexOf(singleCheckpoint) == nextSingleCheckpointIndex)
        {
            //correct

            SingleCheckpoint correctSingleCheckpoint = singleCheckpointList[nextSingleCheckpointIndex];
            correctSingleCheckpoint.Hide();

            nextSingleCheckpointIndex = (nextSingleCheckpointIndex + 1) % singleCheckpointList.Count;
            OnPlayerCorrectCheckpoint?.Invoke(this, EventArgs.Empty);

            if (nextSingleCheckpointIndex == 1 && checkpointCounter == singleCheckpointList.Count)
            {
                checkpointCounter = 1;
                OnPlayerCompleteLap?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                checkpointCounter++;
            }
        }
        else
        {
            //wrong

            OnPlayerWrongCheckpoint?.Invoke(this, EventArgs.Empty);

            HighlightCorrectCheckpoint("show");
        }
    }

    private void HighlightCorrectCheckpoint(string action) 
    {
        SingleCheckpoint correctSingleCheckpoint = singleCheckpointList[nextSingleCheckpointIndex];
        if (action == "show")
        {
            correctSingleCheckpoint.Show();
        }
        if (action == "hide")
        {
            correctSingleCheckpoint.Hide();
        }
    }

    private void ResetCheckpoints() 
    {
        Transform checkpointsTransform = this.transform;

        singleCheckpointList = new List<SingleCheckpoint>();
        foreach (Transform singleCheckpointTransform in checkpointsTransform)
        {
            SingleCheckpoint singleCheckpoint = singleCheckpointTransform.GetComponent<SingleCheckpoint>();

            singleCheckpoint.SetTrackCheckpoints(this);

            singleCheckpointList.Add(singleCheckpoint);
        }

        nextSingleCheckpointIndex = 0;
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
