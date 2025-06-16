using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class APController : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> APGems = new List<GameObject>();

    private int currentAP = 0;

    private void UpdateUI()
    {
        for (int i = 0; i < APGems.Count; i++)
        {
            if (i < currentAP)
            {
                APGems[i].SetActive(true);
            } 
            else
            {
                APGems[i].SetActive(false);
            }
            
        }
    }

    private void UpdateUI(Color color)
    {
        for (int i = 0; i < APGems.Count; i++)
        {
            if (i < currentAP)
            {
                APGems[i].SetActive(true);
            }
            else
            {
                APGems[i].SetActive(false);
            }

            APGems[i].GetComponent<Image>().color = color;
              
        }
    }
    
    private IEnumerator BlinkGem(int index)
    {
        while (true)
        {
            APGems[index].SetActive(true);
            yield return new WaitForSeconds(0.3f);
            APGems[index].SetActive(false);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void GoingDecreaseAP(int number)
    {
        if (currentAP <= 0 || number <= 0 || number > currentAP)
        {
            StopBlink();
            return;
        }

        StopBlink();

        for (int i = 0; i < number; i++)
        {
            int index = currentAP - 1 - i;
            StartCoroutine(BlinkGem(index));
        }
    }

    public void StopBlink()
    {
        StopAllCoroutines();
        UpdateUI();
    }

    public void SetAP(int number)
    {
        if (number > APGems.Count)
        {
            currentAP = APGems.Count;
        }
        else
        {
            currentAP = number;
        }

        UpdateUI();
    }

    public void SetAP(int number, Color color)
    {
        if (number > APGems.Count)
        {
            currentAP = APGems.Count;
        }
        else
        {
            currentAP = number;
        }

        UpdateUI(color);
    }

    public int GetAP()
    {
        return currentAP;
    }

    public void DecreaseAP(int number)
    {
        currentAP -= number;
        UpdateUI();
    }
}
