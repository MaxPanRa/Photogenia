using UnityEngine;
using DistantLands.Cozy;
using TMPro;

public class Clock : MonoBehaviour
{
    public CozyWeather cozy;

    private void FixedUpdate()
    {
        string hours = "";
        string minutes = "";
        if (cozy.calendar.meridiemTime.hours < 10)
        {
            hours = "0"+cozy.calendar.meridiemTime.hours;
        }
        else
        {
            hours = cozy.calendar.meridiemTime.hours+"";
        }
        if (cozy.calendar.meridiemTime.minutes < 10)
        {
            minutes = "0" + cozy.calendar.meridiemTime.minutes;
        }
        else
        {
            minutes = cozy.calendar.meridiemTime.minutes + "";
        }
        GetComponent<TextMeshProUGUI>().text = hours + ":"+ minutes;
    }
}
