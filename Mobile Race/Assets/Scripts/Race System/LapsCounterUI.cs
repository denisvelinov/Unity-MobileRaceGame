using System;
using UnityEngine;
using TMPro;

public class LapsCounterUI : MonoBehaviour
{
    [SerializeField] private TMP_Text lapsCounter;

    [SerializeField] private StartRaceButtonUI startRace;
    [SerializeField] private LapTimerUI completeLap;

    [SerializeField] private SelectionNumberUI selection;

    public EventHandler OnPlayerCompleteRace;

    private int curLap;
    private int lapsLenght;

    private void Start()
    {
        startRace.OnRaceStart += StartRace_OnRaceStart;
        completeLap.OnLapTimeRecorded += CompleteLap_OnLapTimeRecorded;

        SetLaps();
    }

    private void CompleteLap_OnLapTimeRecorded(object sender, EventArgs e)
    {
        curLap++;

        if (curLap > lapsLenght)
        {
            // end race
            curLap = 1;
            OnPlayerCompleteRace?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            lapsCounter.text = $"{curLap}/{lapsLenght}";
        }
    }

    private void StartRace_OnRaceStart(object sender, System.EventArgs e)
    {
        SetLaps();
    }

    private void SetLaps() 
    {
        lapsLenght = Int32.Parse(selection.selectionNumber.text);
        curLap = 1;

        lapsCounter.text = $"{curLap}/{lapsLenght}";
    }
}
