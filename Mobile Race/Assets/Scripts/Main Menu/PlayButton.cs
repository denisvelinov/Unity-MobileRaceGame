using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public void LoadGame() 
    {
        SceneManager.LoadScene("Track" + (TrackToLoadValues.trackIndex + 1));
    }
}
