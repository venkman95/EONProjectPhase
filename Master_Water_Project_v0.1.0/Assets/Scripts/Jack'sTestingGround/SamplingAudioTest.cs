using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamplingAudioTest : MonoBehaviour
{
    [Header("References")]
    public float speed;
    public MovementManager movementManager;
    public AudioManager audioManager;
    public Utility utility;
    public GameObject aeratorStart;
    public GameObject jar;

    public GameObject pauseMenu;
    public GameObject pauseButton;
    public Material inletHighlight;
    public Material outletHighlight;
    [HideInInspector] public string correctValve;
    [HideInInspector] public bool handleSelected;
    public float highlightSpeed = 2;
    public void StartStory()
    {
        utility.canSelectObjects = false;
        StartCoroutine(Story());
        //aeratorStart.SetActive(false);
        jar.SetActive(false);
    }

    public IEnumerator ValveHighlight(bool inOrOutlet)
    {
        int direction = 1;

        Material highlightMaterial= inletHighlight;

        //Determines whether the inlet or outlet will be highlighted
        /*if (inOrOutlet)
        {
            highlightMaterial = inletHighlight;
            correctValve = "Inlet";
        }
        else
        {
            highlightMaterial = outletHighlight;
            correctValve = "Outlet";
        }
        */
        //Stops animatinig once the valve has been clicked
        while (!handleSelected)
        {
            //Based off of the materials alpha, the direction will change which will lead to the object fading in or out
            if (highlightMaterial.color.a >= 1)
                direction = -1;

            else if (highlightMaterial.color.a <= 0)
                direction = 1;

            highlightMaterial.color = new Color(highlightMaterial.color.r, highlightMaterial.color.g, highlightMaterial.color.b, highlightMaterial.color.a + Time.deltaTime * highlightSpeed * direction);
            yield return null;
        }
    }
    private IEnumerator Story()
    {

        yield return new WaitForSeconds(0.5f);
        //Introduction audio played on activation of QR Marker
        audioManager.PlaySound("Test1");

        while (true)
        {
            //Breaks free of loop after audio has completed
            if (audioManager.GetSound("Test1").hasCompleted)
                
                break;

            yield return null;
        }
        //allows the selection of other objects
        StartCoroutine(ValveHighlight(false));
        utility.canSelectObjects = true;



        while (true)
        {
            handleSelected = true;
            StartCoroutine(ValveHighlight(false));


            if (utility.lotovalvestory.hitName == "Left")
            {   //needed to keep audio from infinitely activating
                if(audioManager.GetSound("Cold Handle").playing == false)
                {
                audioManager.PlaySound("Cold Handle");
                //hitname must be cleared to stop from repeating
                utility.lotovalvestory.hitName = "";
                }
                
            }

            if (audioManager.GetSound("Cold Handle").hasCompleted)
            {
             
                if (audioManager.GetSound("Aerator").playing == false && audioManager.GetSound("Aerator").hasCompleted == false)
                {
                    audioManager.PlaySound("Aerator");
                    utility.lotovalvestory.hitName = "";    
                }

                if (audioManager.GetSound("Aerator").hasCompleted)
                {
                    aeratorStart.SetActive(false);
                    jar.SetActive(true);
                    if (audioManager.GetSound("Jar").playing == false && audioManager.GetSound("Jar").hasCompleted == false)
                    {
                        audioManager.PlaySound("Jar");
                    }
                }



                
            }

            if (utility.lotovalvestory.hitName == "Right")
            {
                audioManager.PlaySound("Hot Handle");
                break;
            }

            yield return null;
        }
        
        yield return new WaitForSeconds(5);
        pauseMenu.SetActive(true);
        pauseButton.SetActive(false);
        utility.PauseAll();
    }
}
