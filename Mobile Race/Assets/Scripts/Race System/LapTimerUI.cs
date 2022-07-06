using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LapTimerUI : MonoBehaviour
{
    [SerializeField] private CountdownManager startRace;
    [SerializeField] private EndRaceButtonUI endRace;
    [SerializeField] private TrackTimeTrialSystem completeLap;
    [SerializeField] private TimeTable timeTable;

    [SerializeField] private TMP_Text lapTime;
    [SerializeField] private TMP_Text bestLapTime;
    [SerializeField] private TMP_Text newBest;

    public EventHandler OnLapTimeRecorded;

    private string scene;

    [SerializeField] private bool startTimer;

    [SerializeField] private int minutes = 0;
    [SerializeField] private int seconds = 0;
    [SerializeField] private int deciseconds = 0;
    private int lastLap = 0;
    private Queue lapTimesQue;

    private void Start()
    {
        startRace.OnCountdownEnd += StartRace_OnCountdownEnd;
        completeLap.OnPlayerCompleteLap += CompleteLap_OnPlayerCompleteLap;
        endRace.OnRaceEnd += EndRace_OnRaceEnd;

        scene = SceneManager.GetActiveScene().name;
        lapTimesQue = new Queue();

        BestRaceTimeSaveSystem.Init();

        bestLapTime.text = "";
        newBest.text = "";
    }

    private void FixedUpdate()
    {
        if (startTimer)
        {
            deciseconds++;

            if (deciseconds > 59)
            {
                deciseconds = 0;
                seconds++;

                if (seconds > 59)
                {
                    seconds = 0;
                    minutes++;

                    if (minutes > 59)
                    {
                        minutes = 60;
                        seconds = 0;
                        deciseconds = 0;
                        startTimer = false;
                    }
                }
            }
        }

        DisplayTimer(minutes, seconds, deciseconds);
    }

    private void CompleteLap_OnPlayerCompleteLap(object sender, EventArgs e)
    {
        if (deciseconds != 0 || seconds != 0 || minutes != 0)
        {
            int totalLapTime = (minutes * 10000) + (seconds * 100) + deciseconds;
            
            int bestTime = BestRaceTimeSaveSystem.Load(scene);
            int unlockedTracks = BestRaceTimeSaveSystem.GetUnlockedTracksNumber();
            int unlockedCars = BestRaceTimeSaveSystem.GetUnlockedCarsNumber();
            int trackIndex = TrackToLoadValues.trackIndex;

            if (bestTime > totalLapTime)
            {
                BestRaceTimeSaveSystem.Save(scene, totalLapTime);

                StartCoroutine(DisplayBestTimeRoutine(GetTimeInformat(totalLapTime), "New Best"));

                if (trackIndex == unlockedTracks && totalLapTime < TrackToLoadValues.trackUnlockTime)
                {
                    BestRaceTimeSaveSystem.IncreaseUnlockedTracksNumber(unlockedTracks);
                }
                if (trackIndex == unlockedCars && totalLapTime < TrackToLoadValues.carUnlockTime)
                {
                    BestRaceTimeSaveSystem.IncreaseUnlockedCarssNumber(unlockedCars);
                }
            }   
            else
            {
                int timeDifference = Mathf.Abs(lastLap - totalLapTime);
                if (totalLapTime > lastLap)
                {
                    if (lastLap == 0)
                    {
                        StartCoroutine(DisplayBestTimeRoutine(GetTimeInformat(timeDifference), "Lap"));
                    }
                    else
                    {
                        StartCoroutine(DisplayBestTimeRoutine(GetTimeInformat(timeDifference), "+"));
                    }
                }
                else
                {
                    StartCoroutine(DisplayBestTimeRoutine(GetTimeInformat(timeDifference), "-"));
                }
            }

            lastLap = totalLapTime;
            lapTimesQue.Enqueue(GetTimeInformat(lastLap));
        }

        deciseconds = 0;
        seconds = 0;
        minutes = 0;

        OnLapTimeRecorded?.Invoke(this, EventArgs.Empty);
    }
    private void StartRace_OnCountdownEnd(object sender, EventArgs e)
    {
        bestLapTime.text = "";
        newBest.text = "";

        startTimer = true;
    }

    private void EndRace_OnRaceEnd(object sender, System.EventArgs e)
    {
        deciseconds = 0;
        seconds = 0;
        minutes = 0;

        lastLap = 0;

        startTimer = false;

        bestLapTime.text = "";
        newBest.text = "";

        timeTable.UpdateScores(lapTimesQue);
        lapTimesQue.Clear();
    }

    private void DisplayTimer(int mins, int secs, int decsecs)
    {
        lapTime.text = $"{minutes:D2}:{seconds:D2}:{deciseconds:D2}";
    }

    public string GetTimeInformat(int totalTime)
    {
        int mins = totalTime / 10000;
        int secs = Mathf.Abs((totalTime / 100) - (mins * 100));
        int decsecs = Mathf.Abs(totalTime - ((secs * 100) + (mins * 10000)));

        string formatedTime = $"{mins:D2}:{secs:D2}:{decsecs:D2}";
        return formatedTime;
    }

    IEnumerator DisplayBestTimeRoutine(string timeInFormat, string operatorSign)
    {
        bestLapTime.text = timeInFormat;
        newBest.text = operatorSign;
        
        yield return new WaitForSeconds(3f);
        
        bestLapTime.text = "";
        newBest.text = "";
    }
}
