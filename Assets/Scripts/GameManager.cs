using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int day;
    
    public List<int> day1;
    public List<int> day2;
    public List<int> day3;
    public List<int> trial;

    public List<Item> inventoryItems;
    private GameObject[] itemInScene;

    private Button backDay1;
    private Button backDay2;
    private Button backDay3;

    public bool hasShovel;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        day1 = new List<int>();
        day2 = new List<int>();
        day3 = new List<int>();
        inventoryItems = new List<Item>();

        DontDestroyOnLoad(instance);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartDay();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartDay()
    {
        itemInScene = GameObject.FindGameObjectsWithTag("Item");

        foreach (GameObject item in itemInScene)
        {
            if (inventoryItems.Contains(item.GetComponent<ItemPrefab>().item))
            {
                item.gameObject.SetActive(false);
            }
        }

        if(day == 4)
        {

            StartCoroutine(StartTrial());
        }
    }

    IEnumerator StartTrial()
    {
        yield return new WaitForSeconds(3);
        GameObject trial = GameObject.Find("GuardA");
        trial.GetComponent<NPCs>().trigger.TriggerDialogueTree();
    }

    public void EndDay()
    {
        // Save all today's decisions
        if (day1.Count == 0)
        {
            foreach (int decision in Decisions.instance.playerDecisions)
            {
                day1.Add(decision);
            }
        }
        else if (day2.Count == 0)
        {
            foreach (int decision in Decisions.instance.playerDecisions)
            {
                day2.Add(decision);
            }
            day2.RemoveAll(x => day1.Contains(x));
        }
        else if (day3.Count == 0)
        {
            foreach (int decision in Decisions.instance.playerDecisions)
            {
                day3.Add(decision);
            }
            day3.RemoveAll(x => day1.Contains(x));
            day3.RemoveAll(x => day2.Contains(x));
        }

        // Change the day
        day++;
        if (day < 4)
        {
            SceneManager.LoadScene("Day" + day.ToString());
        }
        else
        {
            SceneManager.LoadScene("Decision Day");
        }
    }

    public void Ending(int ID)
    {      
        // Bad Ending
        if (ID == 29)
        {
            StartCoroutine(BadEnding());
        }
        // Neutral Ending
        else if (ID == 28)
        {
            StartCoroutine(NeutralEnding());
        }
        // True Ending
        else if (ID == 27)
        {
             StartCoroutine(TrueEnding());
        }
    }

    IEnumerator BadEnding()
    {
        GameObject endScreen = GameObject.Find("End Screen");
        endScreen.GetComponent<GraphicRaycaster>().enabled = true;
        GameObject container = GameObject.Find("End Screen/Container");
        endScreen.GetComponentInChildren<GDTFadeEffect>().enabled = true;
        yield return new WaitForSeconds(3);
        for (int i = 0; i < container.transform.childCount; i++)
        {
            GameObject childObject = container.transform.GetChild(i).gameObject;
            childObject.SetActive(true);
        }
        backDay1 = GameObject.Find("End Screen/Container/Day1").GetComponent<Button>();
        backDay1.onClick.AddListener(() => ReloadDay(0));
        backDay2 = GameObject.Find("End Screen/Container/Day2").GetComponent<Button>();
        backDay2.onClick.AddListener(() => ReloadDay(1));
        backDay3 = GameObject.Find("End Screen/Container/Day3").GetComponent<Button>();
        backDay3.onClick.AddListener(() => ReloadDay(2));
    }

    IEnumerator NeutralEnding()
    {
        GameObject endScreen = GameObject.Find("Innocent Screen");
        GameObject container = GameObject.Find("Innocent Screen/Container");
        endScreen.GetComponentInChildren<GDTFadeEffect>().enabled = true;
        yield return new WaitForSeconds(3);
        for (int i = 0; i < container.transform.childCount; i++)
        {
            GameObject childObject = container.transform.GetChild(i).gameObject;
            childObject.SetActive(true);
        }
        backDay1 = GameObject.Find("Innocent Screen/Container/Day1").GetComponent<Button>();
        backDay1.onClick.AddListener(() => ReloadDay(0));
        backDay2 = GameObject.Find("Innocent Screen/Container/Day2").GetComponent<Button>();
        backDay2.onClick.AddListener(() => ReloadDay(1));
        backDay3 = GameObject.Find("Innocent Screen/Container/Day3").GetComponent<Button>();
        backDay3.onClick.AddListener(() => ReloadDay(2));
    }

    IEnumerator TrueEnding()
    {
        GameObject endScreen = GameObject.Find("Found Killer Screen");
        yield return null;
    }

    public void ReloadDay(int dayToStart)
    {
        // Erase data of days that haven't occured
        switch (dayToStart)
        {
            case 0:
                Debug.Log("day1");
                day1.Clear();
                goto case 1;
            case 1:
                Debug.Log("day2");
                day2.Clear();
                goto case 2;
            case 2:
                Debug.Log("day3");
                day3.Clear();
                break;
        }
        Debug.Log("Cleared GameManager decisions");
        Decisions.instance.playerDecisions.Clear();
        Decisions.instance.playerDecisions.AddRange(day1);
        Decisions.instance.playerDecisions.AddRange(day2);
        Decisions.instance.playerDecisions.AddRange(day3);
        Debug.Log("Cleared decisions");
        foreach (Item item in inventoryItems.ToList())
        {
            if (!Decisions.instance.playerDecisions.Contains(item.ID))
            {
                inventoryItems.Remove(item);
            }
        }
        day = dayToStart + 1;
        SceneManager.LoadScene("Day" + (dayToStart + 1).ToString());
    }
}
