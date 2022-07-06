using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarManager : MonoBehaviour
{
    [SerializeField]private GameObject[] carList;
    private int index;
    private GameObject childGO;

    void Awake()
    {
        index = CarToLoadValues.carIndex;

        childGO = Instantiate(carList[index], transform.position, transform.rotation, this.transform) as GameObject;
        transform.Find("Player Marker").parent = childGO.transform;
    }
}
