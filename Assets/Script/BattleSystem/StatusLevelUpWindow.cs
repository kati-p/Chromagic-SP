using TMPro;
using UnityEngine;

public class StatusLevelUpWindow : MonoBehaviour
{
    TextMeshProUGUI characterNameText;
    TextMeshProUGUI healthPowerText;
    TextMeshProUGUI magicPowerText;
    TextMeshProUGUI magicDefenceText;
    TextMeshProUGUI speedText;
    TextMeshProUGUI healthPowerLevelUpText;
    TextMeshProUGUI magicPowerLevelUpText;
    TextMeshProUGUI magicDefenceLevelUpText;
    TextMeshProUGUI speedLevelUpText;

    public void SetUpLevelUp(string characterName, float healthPower, float magicPower, float magicDefence, float speed,
        float healthPowerLevelUp, float magicPowerLevelUp, float magicDefenceLevelUp, float speedLevelUp)
    {
        characterNameText = transform.Find("CharacterName").GetComponent<TextMeshProUGUI>();
        healthPowerText = transform.Find("HealthPower").GetComponent<TextMeshProUGUI>();
        magicPowerText = transform.Find("MagicPower").GetComponent<TextMeshProUGUI>();
        magicDefenceText = transform.Find("MagicDefence").GetComponent<TextMeshProUGUI>();
        speedText = transform.Find("Speed").GetComponent<TextMeshProUGUI>();
        healthPowerLevelUpText = transform.Find("HealthPowerLevelUp").GetComponent<TextMeshProUGUI>();
        magicPowerLevelUpText = transform.Find("MagicPowerLevelUp").GetComponent<TextMeshProUGUI>();
        magicDefenceLevelUpText = transform.Find("MagicDefenceLevelUp").GetComponent<TextMeshProUGUI>();
        speedLevelUpText = transform.Find("SpeedLevelUp").GetComponent<TextMeshProUGUI>();

        UpdateLevelUpUI(characterName,  healthPower,  magicPower,  magicDefence,  speed,
         healthPowerLevelUp,  magicPowerLevelUp,  magicDefenceLevelUp,  speedLevelUp);
    }

    public void UpdateLevelUpUI(string characterName, float healthPower, float magicPower, float magicDefence, float speed,
        float healthPowerLevelUp, float magicPowerLevelUp, float magicDefenceLevelUp, float speedLevelUp)
    {
        characterNameText.text = characterName;
        healthPowerText.text = $"{healthPower:F0}";
        magicPowerText.text = $"{magicPower:F0}";
        magicDefenceText.text = $"{magicDefence:F0}";
        speedText.text = $"{speed:F0}";
        healthPowerLevelUpText.text = $"{healthPowerLevelUp:F0}";
        magicPowerLevelUpText.text = $"{magicPowerLevelUp:F0}";
        magicDefenceLevelUpText.text = $"{magicDefenceLevelUp:F0}";
        speedLevelUpText.text = $"{speedLevelUp:F0}";
    }
}
