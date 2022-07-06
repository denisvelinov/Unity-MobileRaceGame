using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseNumberUI : MonoBehaviour
{
    [SerializeField] private SelectionNumberUI selectionNumber;

    public void IncreaseNumber()
    {
        selectionNumber.ChangeNumber(1);
    }
}
