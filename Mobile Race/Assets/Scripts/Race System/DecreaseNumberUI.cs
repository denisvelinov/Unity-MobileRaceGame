using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecreaseNumberUI : MonoBehaviour
{
    [SerializeField] private SelectionNumberUI selectionNumber;

    public void DecreaseNumber() 
    {
        selectionNumber.ChangeNumber(-1);
    }
}
