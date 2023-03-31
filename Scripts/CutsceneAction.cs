using UnityEngine;
public class CutsceneAction : MonoBehaviour
{
    public CUTSCENES thisCutscene;
    public void CheckActive()
    {
        bool active = thisCutscene == GameManager.instance.currentCutscene;
        this.gameObject.SetActive(active);
    }
}
