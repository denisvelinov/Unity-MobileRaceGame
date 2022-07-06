using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

[RequireComponent(typeof(CarController))]
public class ResetButton : MonoBehaviour
{
    public CarController car;

    private void Start()
    {
        car = PlayerCarManager.FindObjectOfType<CarController>();
    }

    public void ResetPosition()
    {
        car.ResetPosition(ResetPositionValues.resetPosition, ResetPositionValues.resetRotation);
    }
}
