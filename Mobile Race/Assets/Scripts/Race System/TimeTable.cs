using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeTable : MonoBehaviour
{
    [SerializeField] private CloseTimeTableButtonUI closeTable;

    private Transform timeTemplate;

    private void Start()
    {
        closeTable.OnTimeTableClose += ClosseTable_OnTimeTableClose;
    }

    private void ClosseTable_OnTimeTableClose(object sender, System.EventArgs e)
    {
        EmptyScoreBoard();
    }

    public void UpdateScores(Queue raceTimesQue) 
    {
        timeTemplate = transform.Find("Times Container");
        
        int raceTimesCount = raceTimesQue.Count;
        for (int i = 0; i < raceTimesCount; i++)
        {
            Transform time = timeTemplate.GetChild(i);
            time.GetComponent<TMP_Text>().text = raceTimesQue.Peek().ToString();
            raceTimesQue.Dequeue();
        }

        Show();
    }
    private void EmptyScoreBoard()
    {
        for (int i = 0; i < timeTemplate.childCount; i++)
        {
            Transform time = timeTemplate.GetChild(i);
            time.GetComponent<TMP_Text>().text = "--:--:--";
        }

        Hide();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
