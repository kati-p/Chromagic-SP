using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MagicPageController : MonoBehaviour
{
    private TextMeshProUGUI nameText;
    private APController cost;
    private GameObject chargeGO;
    private Image chargeImage;
    private GameObject lightParticle;
    private ParticleSystem ps;
    private Image colorPalette;
    private Image background;
    private TextMeshProUGUI description;
    private TextMeshProUGUI pressEnterText;

    [SerializeField]
    private bool isSelectedPage;

    private void Awake()
    {
        nameText = transform.Find("Name").GetComponent<TextMeshProUGUI>();

        cost = transform.Find("Cost").GetComponent<APController>();

        chargeGO = transform.Find("Charge").gameObject;
        chargeImage = chargeGO.GetComponent<Image>();

        lightParticle = transform.Find("ColorLightEffect").gameObject;
        ps = lightParticle.GetComponent<ParticleSystem>();

        colorPalette = transform.Find("Color").GetComponent<Image>();

        background = transform.Find("Background").GetComponent<Image>();

        if ( isSelectedPage )
        {
            description = transform.Find("Description").GetComponent<TextMeshProUGUI>();
            pressEnterText = transform.Find("PressEnter").GetComponent<TextMeshProUGUI>();
        }
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateUI(Magic magic, int charge, bool isMagicCanUse)
    {
        background.color = Color.black;
        background.GetComponent<UICornersGradient>().enabled = false;

        colorPalette.color = magic.Color;
        colorPalette.gameObject.SetActive(true);

        nameText.text = magic.ColorName;
        nameText.color = magic.BestContrast;
        nameText.gameObject.SetActive(true);

        if (isSelectedPage)
        {
            this.description.gameObject.SetActive(false);
            this.pressEnterText.gameObject.SetActive(false);
        }

        cost.SetAP(magic.Cost, magic.Color);

        if (charge <= 0)
        {
            chargeGO.SetActive(false);
        }
        else
        {
            chargeGO.SetActive(true);
            chargeImage.color = magic.Color;
        }

        if (isMagicCanUse)
        {
            lightParticle.SetActive(true);
            var main = ps.main;
            main.startColor = magic.Color;
            ps.Clear();
            ps.Play();
        }
        else
        {
            lightParticle.SetActive(false);
        }

    }

    public void UpdateUI(string title, string description, Color? pageColor, Color textColor, bool isShowPressEnter,int cost = 0, Color? costColor = null)
    {
        if (pageColor == null)
        {
            background.color = Color.white;
            background.GetComponent<UICornersGradient>().enabled = true;
        }
        else
        {
            background.color = (Color)pageColor;
            background.GetComponent<UICornersGradient>().enabled = false;
        }
        
        
        colorPalette.gameObject.SetActive(false);

        nameText.text = title;
        nameText.color = textColor;
        nameText.gameObject.SetActive(true);

        if (isSelectedPage)
        {
            this.description.text = description;
            this.description.color = textColor;
            this.description.gameObject.SetActive(true);

            if (isShowPressEnter)
            {
                this.pressEnterText.color = textColor;
                this.pressEnterText.gameObject.SetActive(true);
            }
            else
            {
                this.pressEnterText.gameObject.SetActive(false);
            }
        }

        if (costColor != null)
        {
            this.cost.SetAP(cost, (Color)costColor);
        }
        else
        {
            // default color
            this.cost.SetAP(cost, Color.white);
        }
        

        chargeGO.SetActive(false);

        lightParticle.SetActive(false);
    }

    public void UpdateUI()
    {
        background.color = Color.black;
        background.GetComponent<UICornersGradient>().enabled = false;

        nameText.gameObject.SetActive(false);

        if (isSelectedPage)
        {
            this.description.gameObject.SetActive(false);
            this.pressEnterText.gameObject.SetActive(false);
        }

        colorPalette.gameObject.SetActive(false);

        cost.SetAP(0);

        chargeGO.SetActive(false);

        lightParticle.SetActive(false);
    }

    //private int GetNameFontSizeByFilterString(string filterString)
    //{
    //    int stringLenght = filterString.Length;

    //    if (stringLenght < 5)
    //    {
    //        return 50;
    //    }
    //    else if (stringLenght < 9)
    //    {
    //        return 36;
    //    }
    //    else if (stringLenght < 16)
    //    {
    //        return 31;
    //    }
    //    else if (stringLenght < 21)
    //    {
    //        return 26;
    //    }
    //    else
    //    {
    //        throw new System.Exception("string's lenght > 20.");
    //    }
    //}
}
