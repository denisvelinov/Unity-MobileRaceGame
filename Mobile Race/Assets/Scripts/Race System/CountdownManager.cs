using System;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using TMPro;

public class CountdownManager : MonoBehaviour
{
    public event EventHandler OnCountdownEnd;

    [SerializeField] private StartRaceButtonUI startRace;
    [SerializeField] private SteeringWheelScript steeringWheel;
    [SerializeField] private TMP_Text Countdown;

    private void Start()
    {
        startRace.OnRaceStart += StartRace_OnRaceStart;
    }
    private void StartRace_OnRaceStart(object sender, System.EventArgs e)
    {
        steeringWheel.canMove = false;

        StartCoroutine(CountdownRoutine());
    }

    IEnumerator CountdownRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        Countdown.text = "3";
        Countdown.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);
        Countdown.gameObject.SetActive(false);
        Countdown.text = "2";
        Countdown.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);
        Countdown.gameObject.SetActive(false);
        Countdown.text = "1";
        Countdown.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);
        Countdown.gameObject.SetActive(false);
        Countdown.text = "GO!";
        Countdown.gameObject.SetActive(true);

        steeringWheel.canMove = true;
        OnCountdownEnd?.Invoke(this, EventArgs.Empty);

        yield return new WaitForSeconds(1f);
        Countdown.gameObject.SetActive(false);
    }
}
