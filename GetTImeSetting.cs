using DistantLands.Cozy;
using UnityEngine;
using UnityEngine.UI;
public class GetTImeSetting : MonoBehaviour
{
    public CozyWeather cozyWeather;
    void FixedUpdate()
    {
        GetComponent<Slider>().value = cozyWeather.currentTicks;
    }
}
