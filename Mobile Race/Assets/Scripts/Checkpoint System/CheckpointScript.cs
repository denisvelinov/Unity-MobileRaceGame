using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    private void OnTriggerEnter()
    {
        ResetPositionValues.resetPosition = this.transform.position;
        ResetPositionValues.resetRotation = this.transform.rotation;
    }
}
