using System;
using UnityEngine;

public class EndRaceButtonUI : MonoBehaviour
{
    public event EventHandler OnRaceEnd;

    [SerializeField] private LapsCounterUI completeRace;
    [SerializeField] private CountdownManager startRace;
    [SerializeField] private StartRaceButtonUI startRaceButton;

    private void Start()
    {
        completeRace.OnPlayerCompleteRace += CompleteRace_OnPlayerCompleteRace;
        startRace.OnCountdownEnd += StartRace_OnCountdownEnd;
        startRaceButton.OnRaceStart += StartRace_OnRaceStart;

        Hide();
    }

    public void OnEnd()
    {
        OnRaceEnd?.Invoke(this, EventArgs.Empty);
    }

    private void CompleteRace_OnPlayerCompleteRace(object sender, EventArgs e)
    {
        OnEnd();
    }

    private void StartRace_OnCountdownEnd(object sender, EventArgs e)
    {
        Show();
    }

    private void StartRace_OnRaceStart(object sender, System.EventArgs e)
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
