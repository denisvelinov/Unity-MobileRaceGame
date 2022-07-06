using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPositionValues : MonoBehaviour
{
    public static Vector3 resetPosition;
    public static Quaternion resetRotation;

    public UnityStandardAssets.Vehicles.Car.CarController car;

    private void Start()
    {
        car = GetComponent<UnityStandardAssets.Vehicles.Car.CarController>();
        resetPosition = car.transform.position;
        resetRotation = car.transform.rotation;
    }
}
