using System;
using UnityEngine;
using System.IO;

public class BestRaceTimeSaveSystem : MonoBehaviour
{
    private static readonly string SAVE_FOLDER = Application.persistentDataPath + "\\saved files\\";
    private const int maxTime = 600000; 

    public static void Init()
    {
        if (!Directory.Exists(SAVE_FOLDER))
        {
            Directory.CreateDirectory(SAVE_FOLDER);
        }
    }

    public static void Save(string scene,int newBestTime)
    {
        File.WriteAllText(SAVE_FOLDER + $"{scene}.json", newBestTime.ToString());
    }

    public static int Load(string scene)
    {
        if (File.Exists(SAVE_FOLDER + $"{scene}.json"))
        {
            int retTime = Int32.Parse(File.ReadAllText(SAVE_FOLDER + $"{scene}.json"));
            return retTime;
        }
        else
        {
            return maxTime;
        }
    }

    public static void IncreaseUnlockedTracksNumber(int unlockedTracks)
    {
        string newNumber = (unlockedTracks + 1).ToString();
        File.WriteAllText(SAVE_FOLDER + "UnlockedTracks.json", newNumber);
    }

    public static int GetUnlockedTracksNumber()
    {
        if (File.Exists(SAVE_FOLDER + "UnlockedTracks.json"))
        {
            int retTime = Int32.Parse(File.ReadAllText(SAVE_FOLDER + "UnlockedTracks.json"));
            return retTime;
        }
        else
        {
            return 0;
        }
    }
    public static void IncreaseUnlockedCarssNumber(int unlockedCars)
    {
        string newNumber = (unlockedCars + 1).ToString();
        File.WriteAllText(SAVE_FOLDER + "UnlockedCars.json", newNumber);
    }
    public static int GetUnlockedCarsNumber()
    {
        if (File.Exists(SAVE_FOLDER + "UnlockedCars.json"))
        {
            int retTime = Int32.Parse(File.ReadAllText(SAVE_FOLDER + "UnlockedCars.json"));
            return retTime;
        }
        else
        {
            return 0;
        }
    }
}
