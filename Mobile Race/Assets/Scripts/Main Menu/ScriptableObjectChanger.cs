using UnityEngine;

public class ScriptableObjectChanger : MonoBehaviour
{
    [SerializeField] private ScriptableObject[] scriptableObjects;
    [SerializeField] private TrackDisplay trackDisplay;
    [SerializeField] private CarDisplay carDisplay;
    private int currentIndex;

    private void Awake()
    {
        ChangeScriptableObject(0);
    }

    private void Start()
    {
        BestRaceTimeSaveSystem.Init();
    }

    public void ChangeScriptableObject(int change)
    {
        currentIndex += change;

        if (currentIndex < 0)
        {
            currentIndex = scriptableObjects.Length - 1;
        }
        else if (currentIndex > scriptableObjects.Length - 1)
        {
            currentIndex = 0;
        }

        if (trackDisplay != null)
        {
            trackDisplay.DisplayTrack((Track)scriptableObjects[currentIndex]);
            trackDisplay.DisplayPrizeTimes((Track)scriptableObjects[currentIndex]);
        }

        if (carDisplay != null)
        {
            carDisplay.DisplayCar((Car)scriptableObjects[currentIndex]);
            carDisplay.DisplayCarAttributes((Car)scriptableObjects[currentIndex]);
        }
    }
}
