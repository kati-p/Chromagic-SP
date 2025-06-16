using TMPro;
using UnityEngine;

public class StatusWindow : MonoBehaviour
{
    TextMeshProUGUI characterNameText;
    TextMeshProUGUI magicPowerText;
    TextMeshProUGUI magicDefenceText;
    TextMeshProUGUI speedText;
    TextMeshProUGUI magicCountText;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //characterNameText = transform.Find("CharacterName").GetComponent<TextMeshProUGUI>();
        //magicPowerText = transform.Find("MagicPower").GetComponent<TextMeshProUGUI>();
        //magicDefenceText = transform.Find("MagicDefence").GetComponent<TextMeshProUGUI>();
        //speedText = transform.Find("Speed").GetComponent<TextMeshProUGUI>();
        //magicCountText = transform.Find("MagicCount").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUp(string characterName, float magicPower, float magicDefence, float speed, int magicCount)
    {
        characterNameText = transform.Find("CharacterName").GetComponent<TextMeshProUGUI>();
        magicPowerText = transform.Find("MagicPower").GetComponent<TextMeshProUGUI>();
        magicDefenceText = transform.Find("MagicDefence").GetComponent<TextMeshProUGUI>();
        speedText = transform.Find("Speed").GetComponent<TextMeshProUGUI>();
        magicCountText = transform.Find("MagicCount").GetComponent<TextMeshProUGUI>();

        UpdateUI(characterName, magicPower, magicDefence, speed, magicCount);
    }

    public void UpdateUI(string characterName, float magicPower, float magicDefence, float speed, int magicCount)
    {
        characterNameText.text = characterName;
        magicPowerText.text = $"{magicPower:F0}";
        magicDefenceText.text = $"{magicDefence:F0}";
        speedText.text = $"{speed:F0}";
        magicCountText.text = $"{magicCount}";
    }

    public void UpdateUICharcterName(string characterName)
    {
        characterNameText.text = characterName;
    }

    public void UpdateUIMagicPower(float magicPower)
    {
        magicPowerText.text = $"{magicPower:F0}";
    }

    public void UpdateUIMagicDefence(float magicDefence)
    {
        magicDefenceText.text = $"{magicDefence:F0}";
    }

    public void UpdateUISpeed(float speed)
    {
        speedText.text = $"{speed:F0}";
    }

    public void UpdateUIMagicCount(int magicCount)
    {
        magicCountText.text = $"{magicCount}";
    }
}
