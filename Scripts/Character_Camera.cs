using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Camera : MonoBehaviour
{

    public Transform cameraObject;
    public CameraObject camara;
    public float distance = 100f;
    public bool camDebug = true;
    //Otros
    int layerMask;

    //GUARDADO DE IMAGENES
    string screenshotPath = "Photographs/";
    string screenshotName = "photograph";

    //DEBUGGING CAMERA READING ATTRIBUTES
    public float xO = 6.8f;
    public float yO = 4.6f;
    int raysCount = 25;
    public bool repeatRays = true;
    bool isPhotoing;
    void Start()
    {
        screenshotPath = Application.dataPath + "/Photographs/";
        layerMask = ~LayerMask.GetMask("Ignore_Photo");
        isPhotoing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (((Input.GetKey(KeyCode.Mouse0) || Input.GetKeyDown("joystick button 5")) && !isPhotoing) || repeatRays)
        {
            CameraReader();
            TakeScreenshot();
            isPhotoing = true;
        }
    }

    void CameraReader()
    {
        Vector3 pf = cameraObject.forward;
        Vector3[,] rays = new Vector3[raysCount, raysCount];
        RaycastHit[,] hits = new RaycastHit[raysCount, raysCount];
        int middle = Mathf.FloorToInt(raysCount / 2);
        int hitNo = 0;

        for (int i = 0; i < raysCount; i++)
        {

            for (int j = 0; j < raysCount; j++)
            {
                //GIRAMOS VECTORES RELATIVAMENTE SIN IMPORTAR DIRECCION:
                rays[i, j] = Quaternion.AngleAxis((i - middle)* xO, cameraObject.up) * pf;
                rays[i, j] = Quaternion.AngleAxis((j - middle) * yO, cameraObject.right) * rays[i,j] ;
                //GUARDAMOS REGISTRO DE RAYOS EN LA VARIABLE HITS:
                Physics.Raycast(cameraObject.position, rays[i, j], out hits[i, j], distance, layerMask);
                if (camDebug)
                {
                    Color rayColor = Color.clear;
                    if (i == middle && j == middle)
                    {
                        rayColor = Color.green;
                    }
                    if (hits[i,j].collider){
                        rayColor = Functions.GetObjectFraming(raysCount, i, j) == ObjectFraming.General ? Color.yellow :
                                    Functions.GetObjectFraming(raysCount, i, j) == ObjectFraming.Left ? Color.red :
                                    Functions.GetObjectFraming(raysCount, i, j) == ObjectFraming.LeftGeneral ? Color.magenta :
                                    Functions.GetObjectFraming(raysCount, i, j) == ObjectFraming.Right ? Color.blue :
                                    Functions.GetObjectFraming(raysCount, i, j) == ObjectFraming.RightGeneral ? Color.cyan :
                                    Functions.GetObjectFraming(raysCount, i, j) == ObjectFraming.Center ? Color.white : Color.gray;
                    }
                    
                    Debug.DrawRay(cameraObject.position, rays[i, j] * distance, rayColor);
                }
                if (hits[i,j].collider)
                {
                    hitNo++;
                }


            }
        }
        Debug.Log("NUMBER OF HITS: " + hitNo);

        RaycastsReader(hits);
    }

    void RaycastsReader(RaycastHit[,] hits)
    {
        //MANEJAMOS LA METADATA DE LAS COLISIONES:
        Dictionary<int, Subject> cleanHits = new Dictionary<int, Subject>();
        //bool hasNewFrame = false;
        for (int i = 0; i < raysCount; i++)
        {
            for (int j = 0; j < raysCount; j++)
            {
                
                if (hits[i, j].collider && hits[i, j].collider.GetComponent<Subject>() != null)
                {
                    Subject object_metadata = hits[i, j].collider.GetComponent<Subject>();
                    int id = hits[i, j].transform.gameObject.GetInstanceID();
                    ObjectFraming newFraming = Functions.GetObjectFraming(raysCount, i, j);
                    int points = object_metadata.points;
                    if (cleanHits.ContainsKey(id))
                    {
                        ObjectFraming currentFraming = cleanHits[id].objectFraming;
                        if (currentFraming == ObjectFraming.Center)
                            continue;
                        else if ((newFraming == ObjectFraming.Left || newFraming == ObjectFraming.Right) && (currentFraming != ObjectFraming.Left && currentFraming != ObjectFraming.Right))
                        {
                            cleanHits[id].objectFraming = newFraming;
                        } else if (newFraming == ObjectFraming.Middle && currentFraming != ObjectFraming.Middle)
                        {
                            cleanHits[id].objectFraming = newFraming;
                        }
                    }
                    object_metadata.objectFraming = newFraming;
                    object_metadata.totalPoints = points * (newFraming == ObjectFraming.Center ? 3 : newFraming == ObjectFraming.Left || newFraming == ObjectFraming.Right ? 3 : newFraming == ObjectFraming.Middle ? 2 : 1);
                    object_metadata.distance = Mathf.Round(hits[i, j].distance * 100f) / 100f;
                    cleanHits.Add(id, object_metadata);
                }
            }
        }
        Debug.Log(cleanHits.ToString());
        //if (camDebug) Debug.Log(id + " - HIT: " + hits[i, j].collider.name + " AT: " + Mathf.Round(hits[i, j].distance * 100f) / 100f + " MTS. ON [" + Functions.GetObjectFraming(raysCount, i, j) + "]");
        StartCoroutine(waitForProcessing());
    }

    IEnumerator waitForProcessing()
    {
        yield return new WaitForSeconds(1);
        isPhotoing = false;
    }

    void TakeScreenshot()
    {
        if (!Directory.Exists(screenshotPath))
        {
            Directory.CreateDirectory(screenshotPath);
        }
        // Disable the canvas to avoid it being captured in the screenshot
        Canvas[] canvas = FindObjectsOfType<Canvas>();
        foreach(Canvas canva in canvas)
        {
            canva.enabled = false;
        }


        // Take the screenshot
        long milliseconds = System.DateTime.Now.Ticks / System.TimeSpan.TicksPerMillisecond;
        string date = milliseconds.ToString();
        string fileName = screenshotPath + date+"_"+ screenshotName + ".jpg";
        StartCoroutine(TakeScreenShot(fileName,canvas));
        


        
    }
    IEnumerator TakeScreenShot(string fileN, Canvas[] canvas)
    {
        yield return new WaitForEndOfFrame();
        ScreenCapture.CaptureScreenshot(fileN, 1);
        
        // Re-enable the canvas
        foreach (Canvas canva in canvas)
        {
            canva.enabled = true;
        }
    }

}