using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData : MonoBehaviour
{
    public List<int> day1;
    public List<int> day2;
    public List<int> day3;

    public PlayerData(GameManager player)
    {
        day1 = player.day1;
        day2 = player.day2;
        day3 = player.day3;
    }
}
