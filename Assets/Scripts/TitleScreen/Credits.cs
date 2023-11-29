using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{

    public void OpenCreditPage(GameObject creditPage)
    {
        if (creditPage.activeSelf)
        {
            creditPage.SetActive(false);
            return;
        }
        GameObject[] creditPages = GameObject.FindGameObjectsWithTag("Credits");
        foreach (GameObject page in creditPages)
        {
            page.SetActive(false);
        }
        creditPage.SetActive(true);
    }
}
