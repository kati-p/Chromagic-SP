using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class CharacterBattle : MonoBehaviour
{
    [SerializeField]
    protected string characterName;
    
    [SerializeField] 
    protected HPController HP;

    [SerializeField] 
    protected APController AP;

    [SerializeField]
    protected float _MP;

    [SerializeField]
    protected float _MD;

    [SerializeField]
    protected float _SPD;

    [SerializeField]
    protected Image characterImage;

    [SerializeField]
    protected AudioClip hurtImmuneSFX;
    [SerializeField]
    protected AudioClip[] hurtLowSFX;
    [SerializeField]
    protected AudioClip[] hurtHighSFX;
    
    public GameObject damageTextPrefab;

    public CameraShake CameraShake;

    public StatusWindow statusWindow;

    public abstract void SetUp();

    public string CharacterName { get { return characterName; } set { characterName = value; } }

    public float MP { get { return _MP; } set { _MP = value; } }

    public float MD { get { return _MD; } set { _MD = value; } }

    public float SPD { get { return _SPD; } set { _SPD = value; } }

    public void SetAP(int number)
    {
        AP.SetAP(number);
    }

    public int GetAP()
    {
        return AP.GetAP();
    }

    public void DecreaseAP(int number)
    {
        AP.DecreaseAP(number);
    }

    public void GoingDecreaseAP(int number)
    {
        AP.GoingDecreaseAP(number);
    }

    public void StopBlinkAP()
    {
        AP.StopBlink();
    }

    public Color GetDefenseColor()
    {
        return HP.getHPColor();
    }

    public void ChangeDefenseColor(Color color)
    {
        HP.ChangeColor(color);
    }

    public bool TakeDamage(float damage, Color attackColor, float contrastRatio = -1f)
    {
        bool isDead = false;
        bool isSameColor = false;
        
        if (attackColor == HP.getHPColor())
        {
            isSameColor = true;
            damage = 0;

            SoundManager.Instance.PlaySoundFXClip(hurtImmuneSFX, 1f, transform);
        } else
        {
            damage = damage - MD;

            if (damage <= 0)
            {
                damage = 0;
            }
            isDead = HP.TakeDamage(damage);

            if (contrastRatio < 0)
            {
                SoundManager.Instance.PlayRandomSoundFXClip(hurtHighSFX, 1f, transform);
            }
            else if (contrastRatio < 3f)
            {
                SoundManager.Instance.PlayRandomSoundFXClip(hurtLowSFX, 1f, transform);
            }
            else
            {
                SoundManager.Instance.PlayRandomSoundFXClip(hurtHighSFX, 1f, transform);
            }
        }


        float damageMagnitude = damage > HP.MaxHP ? HP.MaxHP : damage;
        float maginitude = (damageMagnitude / HP.MaxHP) + 0.005f;
        CameraShake.Shake(1f, maginitude);

        StartCoroutine(showDamageText(damage, contrastRatio, attackColor, isSameColor));

        return isDead;
    }


    private IEnumerator showDamageText(float damage, float contrastRatio, Color color, bool isSameColor)
    {
        float formatedContrastRatio = (float)((int)(contrastRatio * 10)) / 10;

        string damageStr = contrastRatio <= 0
            ? $"{damage:F0}"
            : $"{damage:F0}/CR{formatedContrastRatio}";

        RectTransform rt = GetComponent<RectTransform>();
        Vector2 textPos = rt.anchoredPosition
            + new Vector2(
                Random.Range(-rt.sizeDelta.x / 2, rt.sizeDelta.x / 2),
                Random.Range(-rt.sizeDelta.y / 3, rt.sizeDelta.y / 3));
        
        GameObject damageText = Instantiate(damageTextPrefab, new Vector2(0, 0), Quaternion.identity);
        GameObject spawnParent = GameObject.FindGameObjectWithTag("Spawn DamageText");
        damageText.transform.SetParent(spawnParent.transform, false);
        spawnParent.GetComponent<RectTransform>().anchoredPosition = textPos;

        TextMeshProUGUI text = damageText.GetComponent<TextMeshProUGUI>();
        text.text = isSameColor
            ? "IMMUNE SAMECOLOR"
            : damageStr;
        text.color = color;

        float textDisplayDuration = damageText.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length;

        yield return new WaitForSeconds(textDisplayDuration);

        Destroy(damageText);
    }

    public void IncreaseSPD(float speedIncrease)
    {
        _SPD += speedIncrease;

        statusWindow.UpdateUISpeed(_SPD);
    }
}
