using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    GameObject Horse;
    [SerializeField]
    GameObject Player;
    [SerializeField]
    GameObject Butler;
    [SerializeField]
    GameObject Dialogue;
    [SerializeField]
    GameObject BlackoutCanvas;
    [SerializeField]
    TextMeshProUGUI DialogueText;
    [SerializeField]
    TextMeshProUGUI SpeakerText;
    [SerializeField]
    Sprite PlayerAccused;
    [SerializeField]
    Sprite ButlerAccused;
    [SerializeField]
    GameObject[] Characters;

    /// <summary>
    /// Order: Player, Butler, GuardA, GuardB, Alice, Ben
    /// </summary>
    Vector3[] Positions = {
    new Vector3(-58.6300011f,-44.5999985f,0f),
    new Vector3(-59.7099991f,-43.9099998f,15.7200003f),
    new Vector3(-61.1499977f,-42.4700012f,0f),
    new Vector3(-59.9799995f,-42.4300003f,0f),
    new Vector3(-60.0099983f,-45.0400009f,0f),
    new Vector3(-63.8378944f,-46.371418f,0f)
    };

    private void Start()
    {
        AudioManager.instance.SetVolume(PlayerPrefs.GetFloat("Volume", 0.5f));
        Invoke("Disembark", 5.12f);
        Butler.transform.position = new Vector3(-2.12f, -1.52f, -0.51f);
        Player.transform.position = new Vector3(-0.39f, 0.18f, -0.386f);
    }

    private void Disembark()
    {
        Player.GetComponent<Animator>().SetBool("Disembark", true);
        Dialogue.SetActive(true);
        SpeakerText.text = "Butler";
        DialogueText.text = "Welcome! \nMayor Maxwell has been eagerly awaiting your visit. Here let me take your bags and guide you there. ";
        Invoke("WalkToHouse", 2f);
    }

    private void WalkToHouse()
    {
        Butler.GetComponent<Animator>().SetBool("WalkToHouse", true);
        Player.GetComponent<Animator>().SetBool("WalkToHouse", true);
        Invoke("GoIn", 6f);
    }

    private void GoIn()
    {
        DialogueText.text = "You can go in to see the Mayor. He should be having lunch in the dining room at this hour. "+
            "I'll give you a moment to say hi before bringing you to settle in.";
        Invoke("EnterHouse", 6f);
    }

    public void EnterHouse()
    {
        Transition();
        Invoke("MovePlayer", .7f);
    }

    private void Transition()
    {
        BlackoutCanvas.GetComponent<Animator>().SetBool("Transition", true);
    }

    private void MovePlayer()
    {

        BlackoutCanvas.GetComponent<Animator>().SetBool("Transition", false);
        Player.GetComponent<Animator>().SetBool("EnterHouse", true);
        SpeakerText.text = "Detective";
        DialogueText.text = "Hey Maxwell! I finally got here and you're not even going to say hi?";
        Invoke("Discover", 3.33f);
        Invoke("PlayerStop", 6.95f);
    }

    private void Discover()
    {
        DialogueText.text += "\nMax?";
    }
    private void PlayerStop()
    {
        DialogueText.text = "<size=16px>MAXWELL!<size=8.5px>";
        Invoke("GettingCaught", 4f);
    }

    private void GettingCaught()
    {
        Butler.GetComponent<Animator>().SetBool("Left", true);
        SpeakerText.text = "Butler";
        DialogueText.text = "What's wrong? I heard someone scream.";


        Invoke("ButlerGrieving", 3.5f);
    }


    private void ButlerGrieving()
    {
        DialogueText.text = "MAYOR MAXWELL! NO! THIS CAN'T BE TRUE!";
        Invoke("Transition", 4f);
        Invoke("Accusation", 4.7f);
    }

    private void Accusation()
    {
        BlackoutCanvas.GetComponent<Animator>().SetBool("Transition", false);

        Butler.GetComponent<Animator>().SetBool("Accuse", true);
        Player.GetComponent<Animator>().SetBool("Berated", true);
        for (int i = 0; i < Characters.Length; i++)
        {
            Characters[i].SetActive(true);
        }
        SpeakerText.text = "Butler";
        DialogueText.text = "<size=13px>How could you murder your own friend? What did he ever do to deserve this!?<size=8.5px>";
        Invoke("Transition", 5.7f);
        Invoke("AliceDefense", 6.4f);
    }

    private void AliceDefense()
    {
        BlackoutCanvas.GetComponent<Animator>().SetBool("Transition", false);
        for (int i = 0; i < 3; i++)
        {
            Characters[i].transform.position = Positions[2+i];
        }
        SpeakerText.text = "Alice";
        DialogueText.text = "There's no way he killed my father! Please! Give him time to help us find out what truly happened.";

        Invoke("Judgement", 6f);
    }

    private void Judgement()
    {
        SpeakerText.text = "Butler";
        DialogueText.text = "Out of respect for Alice's pleas to vouch for her father's potential killer, I will give you one chance.\n";
        Invoke("Threat", 6f);
    }

    private void Threat()
    {
        DialogueText.text = "You have <color=\"red\">3 Days</color>. Search the town and find a different conclusion. Or else you and Alice will be gone.";

        Invoke("EndScene", 6f);
    }
    //Invoke("StartTutorial", 2f);

    private void EndScene()
    {
        BlackoutCanvas.GetComponent<Animator>().SetBool("End", true);
        Invoke("StartGame", 1.5f);
    }
    private void StartGame()
    {
        SceneManager.LoadScene("Day1");
    }
}
