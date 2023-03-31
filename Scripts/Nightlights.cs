using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nightlights : MonoBehaviour
{
    public void LightSwitch(bool active)
    {
        if (active)
        {
            transform.gameObject.SetActive(true);
        }
        else
        {
            transform.gameObject.SetActive(false);
        }
    }
}
