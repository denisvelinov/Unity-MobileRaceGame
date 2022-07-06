using UnityEngine;

[CreateAssetMenu (fileName = "New Track", menuName = "Scriptable Objects/Maps")]
public class Track : ScriptableObject
{
    public int trackIndex;
    public string trackName;
    public string trackDescription;
    public Sprite trackImage;
    public Object sceneToLoad;
    public string bronzePrizeTime;
    public string silverPrizeTime;
    public string goldPrizeTime;
}
