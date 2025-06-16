using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HPController : MonoBehaviour
{
    private Image HPBarImage;
    private RectTransform HPBarRt;
    private Image HeaderColorImage;
    private Image HPShield;
    private TextMeshProUGUI HPNumberText;

    private float maxHPBarWidth;
    private float currentHP;
    private float maxHP;
    private Color currentColor;

    public float MaxHP { get { return maxHPBarWidth; } }

    private void Awake()
    {
        HPBarImage = transform.Find("Bar").GetComponent<Image>();
        HPBarRt = transform.Find("Bar").GetComponent<RectTransform>();
        maxHPBarWidth = HPBarRt.sizeDelta.x;
        HeaderColorImage = transform.Find("Header/Color").GetComponent<Image>();
        HPShield = transform.Find("Shield").GetComponent<Image>();
        HPNumberText = transform.Find("Header/Text").GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {

    }

    private void Update()
    {

    }

    private void UpdateUI()
    {
        float newAmountHP = (this.currentHP / this.maxHP);
        float newWidthHPBar = newAmountHP * maxHPBarWidth;
        HPBarRt.sizeDelta = new Vector2 (newWidthHPBar, HPBarRt.sizeDelta.y);
        //HPShield.fillAmount = newAmountHP;
        if (this.currentHP <= 0 )
        {
            HPShield.fillAmount = 0;
        }

        HPNumberText.text = $"{this.currentHP:F0}";

        HPBarImage.color = currentColor;
        HeaderColorImage.color = currentColor;
        HPShield.color = new Color(currentColor.r, currentColor.g, currentColor.b, 1f);

        Color bestContrast = Chromagic.GetBestContrast(currentColor);

        HPNumberText.color = bestContrast;
    }

    public void SetUp(float currentHP, float maxHP, Color HPcolor)
    {
        if (currentHP > maxHP)
        {
            throw new System.Exception("current hp more than max hp.");
        }
        else if (currentHP == 0 || maxHP == 0)
        {
            throw new System.Exception("can't assign 0 to current or max HP.");
        }
        
        this.currentHP = currentHP;
        this.maxHP = maxHP;
        this.currentColor = HPcolor;

        UpdateUI();
        
    }

    public bool TakeDamage(float damage)
    {
        // is default value
        if (maxHP == 0)
        {
            throw new System.Exception("You need to setUp first.");
        }

        bool isDead = false;

        currentHP = currentHP - damage;
        if (currentHP <= 0)
        {
            currentHP = 0;
            isDead = true;
        }

        UpdateUI();

        return isDead;
    }
    
    public void ChangeColor(Color color)
    {
        this.currentColor = color;
        UpdateUI();
    }

    public Color32 getHPColor()
    {
        return currentColor;
    }
}
