using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    //Menues
    [SerializeField] private Transform mainMenu;
    [SerializeField] private Transform trackSelect;
    [SerializeField] private Transform carSelect;

    //Button Events
    [SerializeField] private BackToTrackSelect backToTrackSelectButton;
    [SerializeField] private BackToMainMenu backToMainMenuButton;
    [SerializeField] private EnterTrackSelect enterTrackSelectButton;
    [SerializeField] private EnterCarSelect enterCarSelectButton;

    private void Start()
    {
        backToTrackSelectButton.OnExitCarSelect += CarSelect_OnBack;
        backToMainMenuButton.OnExitTrackSelect += TrackSelect_OnBack;
        enterTrackSelectButton.OnEnterTrackSelect += TrackSelect_OnEnter;
        enterCarSelectButton.OnEnterCarSelect += CarSelect_OnEnter;
    }

    private void CarSelect_OnBack(object sender, System.EventArgs e)
    {
        carSelect.gameObject.SetActive(false);
        trackSelect.gameObject.SetActive(true);
    }
    private void TrackSelect_OnBack(object sender, System.EventArgs e)
    {
        trackSelect.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);
    }
    private void TrackSelect_OnEnter(object sender, System.EventArgs e)
    {
        mainMenu.gameObject.SetActive(false);
        trackSelect.gameObject.SetActive(true);
    }
    private void CarSelect_OnEnter(object sender, System.EventArgs e)
    {
        trackSelect.gameObject.SetActive(false);
        carSelect.gameObject.SetActive(true);
    }
}
