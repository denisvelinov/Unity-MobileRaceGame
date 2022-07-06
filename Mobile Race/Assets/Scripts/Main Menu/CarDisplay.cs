using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CarDisplay : MonoBehaviour
{
    // Car Data
    [SerializeField] private TMP_Text carName;
    [SerializeField] private GameObject lockIcon;
    [SerializeField] private Image lockImage;
    [SerializeField] private Button playButton;

    // Attribute Data
    [SerializeField] private Image topSpeed;
    [SerializeField] private Image acceleration;
    [SerializeField] private Image handling;

    // Car Model
    [SerializeField] private Transform carHolder;

    //Button Events
    [SerializeField] private BackToTrackSelect backButton;
    [SerializeField] private EnterCarSelect enterCarSelectButton;

    private void Start()
    {
        backButton.OnExitCarSelect += CarSelect_OnBack;
        enterCarSelectButton.OnEnterCarSelect += CarSelect_OnEnter;
    }

    private void CarSelect_OnBack(object sender, System.EventArgs e)
    {
        carHolder.gameObject.SetActive(false);
    }
    private void CarSelect_OnEnter(object sender, System.EventArgs e)
    {
        carHolder.gameObject.SetActive(true);
    }

    public void DisplayCar(Car car)
    {
        carName.text = car.carName;
        CarToLoadValues.carIndex = car.carIndex;

        if (carHolder.childCount > 0)
        {
            Destroy(carHolder.GetChild(0).gameObject);
        }

        Instantiate(car.carToLoad, carHolder.position, carHolder.rotation, carHolder);

        bool carUnlocked = BestRaceTimeSaveSystem.GetUnlockedCarsNumber() >= car.carIndex;
        lockIcon.SetActive(!carUnlocked);
        playButton.gameObject.SetActive(carUnlocked);

        if (carUnlocked)
        {
            lockImage.color = new Color(lockImage.color.r, lockImage.color.g, lockImage.color.b, 0f);
        }
        else
        {
            lockImage.color = new Color(lockImage.color.r, lockImage.color.g, lockImage.color.b, 0.5f);
        }
    }
    public void DisplayCarAttributes(Car car)
    {
        topSpeed.fillAmount = car.topSpeed / 100;
        acceleration.fillAmount = car.acceleration / 100;
        handling.fillAmount = car.handling / 100;
    }
}
