using UnityEngine;
using UnityEngine.UI;
using System.IO;
using DistantLands.Cozy;
public class GameManager : MonoBehaviour
{
    public GameObject Player;
    public GameObject PlayerCharacter;
    public CozyWeather cozyWeather;
    public Animator Transition;

    public CUTSCENES currentCutscene = CUTSCENES.NONE;
    public bool isMovementTutorial = true;
    public bool isCameraTutorial = true;

    public static GameManager instance;

    //LECTURA DE IMAGENES
    string screenshotPath = "Photographs/";
    string screenshotNamePrefix = "photograph";

    public void Start()
    {
        screenshotPath = Application.dataPath + "/Photographs/";
        instance = this;
        //ChangeCutscene(CUTSCENES.INTRODUCTION);
    }

    public void ActivateCharAnimator(bool active)
    {
        Player.GetComponent<Animator>().enabled = active;
    }

    public void TurnNightlightsSwitches(bool active)
    {
        Nightlights[] lights = GameObject.FindObjectsOfType<Nightlights>(true);
        Debug.Log("HAY " + lights.Length + " Luces");
        foreach (Nightlights nl in lights) {
            nl.LightSwitch(active);
        }
    }
    public void FadeBlack(bool fade)
    {
        Transition.SetBool("isBlack", fade);
    }
    public void ChangeCutscene(CUTSCENES cuts)
    {
        currentCutscene = cuts;
        foreach(CutsceneAction cut in GameObject.FindObjectsOfType<CutsceneAction>(true))
        {
            cut.CheckActive();
        }
    }
    public void StopCutscenes()
    {
        if(currentCutscene == CUTSCENES.INTRODUCTION)
        {
            GameObject introSpawn = GameObject.FindGameObjectWithTag("IntroductionSpawn");
            Player.GetComponent<TPSController>().TransportCharacter(introSpawn.transform.position, introSpawn.transform.rotation);
            
        }
        if (currentCutscene == CUTSCENES.MEET_COUPLE)
        {
            GameObject coupleSpawn = GameObject.FindGameObjectWithTag("CoupleSpawn");
            Player.GetComponent<TPSController>().TransportCharacter(coupleSpawn.transform.position, coupleSpawn.transform.rotation);

        }
        if (currentCutscene == CUTSCENES.FRANK_MUSEUM)
        {
            GameObject frankSpawn = GameObject.FindGameObjectWithTag("FrankSpawn");
            Player.GetComponent<TPSController>().TransportCharacter(frankSpawn.transform.position, frankSpawn.transform.rotation);

        }
        currentCutscene = CUTSCENES.NONE;
    }

    public void ActivateMoveTutorial (bool active)
    {
        isMovementTutorial = active;
    }
    public void ActivateCameraTutorial(bool active)
    {
        isCameraTutorial = active;
    }
    public void GetPictures()
    {
        Debug.Log("RETRIEVING IMAGES");
        // Load all the screenshots in the folder
        string[] files = Directory.GetFiles(screenshotPath, "*.jpg");
        Texture2D[] textures = new Texture2D[files.Length];
        for (int i = 0; i < files.Length; i++)
        {
            byte[] bytes = File.ReadAllBytes(files[i]);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(bytes);
            textures[i] = texture;
        }

        // Convert the textures to sprites
        Sprite[] sprites = new Sprite[textures.Length];
        for (int i = 0; i < textures.Length; i++)
        {
            sprites[i] = Sprite.Create(textures[i], new Rect(0, 0, textures[i].width, textures[i].height), Vector2.zero);
        }

        MenuManager.instance.ShowImages(sprites);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}

public enum CUTSCENES
{
    NONE, INTRODUCTION, MEET_COUPLE, FRANK_MUSEUM, FIRST_EAGLE
}