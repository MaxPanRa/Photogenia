using System;
using UnityEngine;
using DistantLands.Cozy.Data;

public class CurrentTimeCycle : MonoBehaviour
{
    private DateTime currentDT;
    public DistantLands.Cozy.CozyWeather cozyWeather;
    void Start()
    {
        Debug.Log(cozyWeather.DayAndTime());
        int hora = currentDT.Hour;
        if (hora > 18 || hora < 4)
        {
            cozyWeather.currentTicks = 290;
        }
        else if(hora> 14)
        {
            cozyWeather.currentTicks = 260;
        }
        else if (hora > 10)
        {
            cozyWeather.currentTicks = 110;
        }
        else if (hora >=4)
        {
            cozyWeather.currentTicks = 60;
        }
    }

}
