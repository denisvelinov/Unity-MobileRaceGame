using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TrackDisplay : MonoBehaviour
{
    // Track Data
    [SerializeField] private TMP_Text trackName;
    [SerializeField] private TMP_Text trackDescription;
    [SerializeField] private Image trackImage;
    [SerializeField] private GameObject lockIcon;
    [SerializeField] private Button nextButton;

    // Prize Data
    [SerializeField] private TMP_Text bronzePrizeTime;
    [SerializeField] private GameObject bronzeCheckMarck;

    [SerializeField] private TMP_Text silverPrizeTime;
    [SerializeField] private GameObject silverCheckMarck;

    [SerializeField] private TMP_Text goldPrizeTime;
    [SerializeField] private GameObject goldCheckMarck;

    // Scene Data
    private string scene = null;

    public void DisplayTrack(Track track)
    {
        trackName.text = track.trackName;
        trackDescription.text = track.trackDescription;
        trackImage.sprite = track.trackImage;

        TrackToLoadValues.trackIndex = track.trackIndex;
        bool mapUnlocked = BestRaceTimeSaveSystem.GetUnlockedTracksNumber() >= track.trackIndex;
        lockIcon.SetActive(!mapUnlocked);
        nextButton.gameObject.SetActive(mapUnlocked);

        if (mapUnlocked)
        {
            trackImage.color = Color.white;
        }
        else
        {
            trackImage.color = Color.gray;
        }
    }

    public void DisplayPrizeTimes(Track track)
    {
        scene = track.sceneToLoad.name;
        int playerBestTime = GetTrackBestTime(scene);

        bronzePrizeTime.text = GetTimeInFormat(track.bronzePrizeTime);
        bronzeCheckMarck.SetActive(SetCheckMark(track.bronzePrizeTime, playerBestTime));
        TrackToLoadValues.trackUnlockTime = int.Parse(track.bronzePrizeTime);

        silverPrizeTime.text = GetTimeInFormat(track.silverPrizeTime);
        silverCheckMarck.SetActive(SetCheckMark(track.silverPrizeTime, playerBestTime));

        goldPrizeTime.text = GetTimeInFormat(track.goldPrizeTime);
        goldCheckMarck.SetActive(SetCheckMark(track.goldPrizeTime, playerBestTime));
        TrackToLoadValues.carUnlockTime = int.Parse(track.goldPrizeTime);
    }

    private string GetTimeInFormat(string time)
    {
        int totalTime = int.Parse(time);

        int mins = totalTime / 10000;
        int secs = Mathf.Abs((totalTime / 100) - (mins * 100));
        int decsecs = Mathf.Abs(totalTime - ((secs * 100) + (mins * 10000)));

        string formatedTime = $"{mins:D2}:{secs:D2}:{decsecs:D2}";
        return formatedTime;
    }

    private int GetTrackBestTime(string sceneName) 
    {
        int bestTime = BestRaceTimeSaveSystem.Load(sceneName);
        return bestTime;
    }

    private bool SetCheckMark(string requiredTime, int playerBestTime) 
    {
        int prizeRequiredTime = int.Parse(requiredTime);
        
        if (prizeRequiredTime >= playerBestTime)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
