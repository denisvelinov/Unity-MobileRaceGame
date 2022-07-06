using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectionNumberUI : MonoBehaviour
{
    [SerializeField] public TMP_Text selectionNumber;

    private void Start()
    {
        selectionNumber.text = 1.ToString();
    }

    public void ChangeNumber(int value)
    {
        int newNumber = Int32.Parse(selectionNumber.text) + value;
        
        if (newNumber < 1)
        {
            newNumber = 5;
        }
        else if (newNumber > 5)
        {
            newNumber = 1;
        }

        selectionNumber.text = newNumber.ToString();
    }
}
