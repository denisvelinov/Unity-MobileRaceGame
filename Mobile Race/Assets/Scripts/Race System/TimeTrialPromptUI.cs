using UnityEngine;

public class TimeTrialPromptUI : MonoBehaviour
{
    [SerializeField] private TimeTrialStart timeTrialStart;
    [SerializeField] private StartRaceButtonUI startRace;

    void Start()
    {
        timeTrialStart.OnPlayerEnterPromptBubble += TimeTrialStart_OnPlayerEnterPromptBubble;
        timeTrialStart.OnPlayerExitPromptBubble += TimeTrialStart_OnPlayerExitPromptBubble;

        startRace.OnRaceStart += StartRace_OnRaceStart;
        
        Hide();
    }

    private void StartRace_OnRaceStart(object sender, System.EventArgs e)
    {
        Hide();
    }
    
    private void TimeTrialStart_OnPlayerEnterPromptBubble(object sender, System.EventArgs e)
    {
        Show();
    }

    private void TimeTrialStart_OnPlayerExitPromptBubble(object sender, System.EventArgs e)
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
