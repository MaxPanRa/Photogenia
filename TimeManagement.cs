using UnityEngine;
using DistantLands.Cozy;

[ExecuteInEditMode]
public class TimeManagement : MonoBehaviour
{
    public CozyWeather cozyWeather;
    public void StopCozy(bool stop)
    {
        cozyWeather.transitioningTime = !stop;
    }
    public void TransitionTimeCozy(int time)
    {
        cozyWeather.TransitionTime(time, cozyWeather.currentDay);
    }
    public void TransitionDayCozy(int day)
    {
        cozyWeather.TransitionTime(cozyWeather.currentTicks, day);
    }

    public void ChangeTimeScale(float scale)
    {
        Time.timeScale = scale;
    }

}
