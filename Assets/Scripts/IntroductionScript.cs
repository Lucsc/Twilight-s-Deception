using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroductionScript : MonoBehaviour
{
    [SerializeField]
    GameObject Camera;
    Vector3 cameraStartPosition = new Vector3(86.405f, 2.255f, -10f);
    [SerializeField]
    GameObject LetterCanvas;
    [SerializeField]
    GameObject DialogueWindow;
    [SerializeField]
    GameObject Letter;
    private bool instructionsComplete = false;

    private void Start()
    {
        Invoke("ShowInstructions", 4f);
        
    }

    public void cycle() 
    {
        Camera.transform.position = cameraStartPosition;
    }

    private void ShowInstructions()
    {
        DialogueWindow.SetActive(true);
    }

    private void Update()
    {
        if (!instructionsComplete && Input.GetKeyDown(KeyCode.E))
        {
            StartLetterRoll();
            instructionsComplete = true;
            DialogueWindow.SetActive(false);
        }
    }

    public void StartLetterRoll()
    {
        CancelInvoke();
        LetterCanvas.GetComponent<Animator>().SetBool("Start", true);
        Letter.SetActive(false);
        //Letter.GetComponent<Canvas>().enabled = true;
    }

    public void EndLetterRoll()
    {
        LetterCanvas.GetComponent<Animator>().SetBool("End", true);
        Invoke("StartTutorial", 2f);
    }

    private void StartTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
