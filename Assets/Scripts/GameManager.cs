using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public List<Item> inventoryItems;
    private GameObject[] itemInScene;

    private Button backDay1;
    private Button backDay2;
    private Button backDay3;

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
            backDay1 = GameObject.Find("Day1").GetComponent<Button>();
            backDay1.onClick.AddListener(() => ReloadDay(0));
            backDay2 = GameObject.Find("Day2").GetComponent<Button>();
            backDay2.onClick.AddListener(() => ReloadDay(1));
            backDay3 = GameObject.Find("Day3").GetComponent<Button>();
            backDay3.onClick.AddListener(() => ReloadDay(2));
        }
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
        Debug.Log("day1: " + day1[0]);
        Decisions.instance.playerDecisions.Clear();
        Decisions.instance.playerDecisions.AddRange(day1);
        Decisions.instance.playerDecisions.AddRange(day2);
        Decisions.instance.playerDecisions.AddRange(day3);
        Debug.Log("decisions: " + Decisions.instance.playerDecisions[0]);
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
