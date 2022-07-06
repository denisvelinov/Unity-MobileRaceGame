using UnityEngine;

[CreateAssetMenu(fileName = "New Car", menuName = "Scriptable Objects/Car")]
public class Car : ScriptableObject
{
    public int carIndex;
    public string carName;
    public GameObject carToLoad;
    public float topSpeed;
    public float acceleration;
    public float handling;
}
