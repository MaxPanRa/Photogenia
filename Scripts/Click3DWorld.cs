using UnityEngine;
using UnityEngine.SceneManagement;
public class Click3DWorld : MonoBehaviour
{
    private Ray mouseRay;
    private RaycastHit mouseHit;
    public GameObject startButton;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouseRay, out mouseHit, 1000f))
            {
                if(mouseHit.collider.gameObject == startButton)
                {
                    Debug.Log("CAMBIO DE ESCENA");
                    startButton.GetComponent<Animator>().SetTrigger("start");
                    
                    SceneManager.LoadSceneAsync("Photocity");
                }
            }
        }
    }
}