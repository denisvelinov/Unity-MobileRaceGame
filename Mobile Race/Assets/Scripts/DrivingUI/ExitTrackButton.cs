using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitTrackButton : MonoBehaviour
{
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

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
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
