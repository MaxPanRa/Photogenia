using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneObject : MonoBehaviour
{
    public CUTSCENES cutscene;
    private bool hasStartedCutscene = false;

    private void FixedUpdate()
    {
        if(GameManager.instance.currentCutscene == cutscene)
        {
            hasStartedCutscene = true;
            this.gameObject.SetActive(true);
        }
        else
        {
            if (hasStartedCutscene)
            {
                Destroy(this);
            }
            this.gameObject.SetActive(false);
        }
    }
}