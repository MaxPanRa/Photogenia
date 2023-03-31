using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiltersActivation : MonoBehaviour
{
    public Collider cameraCollider;
    public GameObject waterFilter;
    private string waterFilterTag = "WaterFilter";

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == waterFilterTag)
        {
            ActivateWaterFilter(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == waterFilterTag)
            ActivateWaterFilter(false);
    }

    public void ActivateWaterFilter(bool active)
    {
        waterFilter.SetActive(active);
    }
}
