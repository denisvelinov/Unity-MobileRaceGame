using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleCheckpoint : MonoBehaviour
{
    private TrackTimeTrialSystem trackCheckpoints;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        Hide();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "ColliderBottom")
        {
            //Debug.Log("Checkpoint!");
            trackCheckpoints.PlayerTroughCheckpoint(this);
        }
        //Debug.Log(other.name.ToString());
    }

    public void SetTrackCheckpoints(TrackTimeTrialSystem trackCheckpoints)
    {
        this.trackCheckpoints = trackCheckpoints;
    }

    public void Show()
    {
        meshRenderer.enabled = true;
    }
    public void Hide()
    {
        meshRenderer.enabled = false;
    }
}
