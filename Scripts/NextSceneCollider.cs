using UnityEngine;

public class NextSceneCollider : MonoBehaviour
{
    public CUTSCENES nextCutscene = CUTSCENES.NONE;
    public GameObject cutsceneToDeactivate;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Invoke("ChangeCut", 3f);
            if(cutsceneToDeactivate != null)
            {
                cutsceneToDeactivate.SetActive(false);
            }
            GameManager.instance.FadeBlack(true);
        }
    }

    private void ChangeCut()
    {
        GameManager.instance.ChangeCutscene(nextCutscene);
    }
}
