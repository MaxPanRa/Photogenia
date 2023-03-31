using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneDeactivateAnimator : MonoBehaviour
{
    public void StopAnimator(bool stop)
    {
        GetComponent<Animator>().enabled = !stop;
    }
}
