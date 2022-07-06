using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private GameObject TrackPath;

    public GameObject LocalPlayer;
    public GameObject MiniMapCam;
    public GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        TrackPath = this.gameObject;

        int numOfPath = TrackPath.transform.childCount;
        lineRenderer.positionCount = numOfPath + 1;

        for (int i = 0; i < numOfPath; i++)
        {
            lineRenderer.SetPosition(i, new Vector3(TrackPath.transform.GetChild(i).transform.position.x, TrackPath.transform.GetChild(i).transform.position.y + 10, TrackPath.transform.GetChild(i).transform.position.z));
        }

        lineRenderer.SetPosition(numOfPath, lineRenderer.GetPosition(11));

        lineRenderer.startWidth = 22f;
        lineRenderer.endWidth = 22f;
    }

    // Update is called once per frame
    void Update()
    {
        MiniMapCam.transform.position = (new Vector3(LocalPlayer.transform.position.x, MiniMapCam.transform.position.y, LocalPlayer.transform.position.z));
        //MiniMapCam.transform.rotation = LocalPlayer.transform.rotation;

        Player.transform.position = (new Vector3(LocalPlayer.transform.position.x, Player.transform.position.y, LocalPlayer.transform.position.z));
        Player.transform.rotation = (new Quaternion(0, LocalPlayer.transform.rotation.y, 0, LocalPlayer.transform.rotation.w));
    }
}
