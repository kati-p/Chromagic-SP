using UnityEngine;
using UnityEngine.UI;

public class TurnIcon : MonoBehaviour
{
    public Animator icon1;
    public Animator icon2;

    [SerializeField]
    private Image playerTurnImage;

    [SerializeField]
    private Image enemyTurnImage;

    [SerializeField]
    private Sprite attackSprite;

    [SerializeField]
    private Sprite defenseSprite;

    private bool firstAttempt = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUp(bool isPlayerTurn)
    {
        if (isPlayerTurn)
        {
            playerTurnImage.sprite = attackSprite;
            enemyTurnImage.sprite = defenseSprite;
        }
        else
        {
            playerTurnImage.sprite = defenseSprite;
            enemyTurnImage.sprite = attackSprite;
        }

        enemyTurnImage.enabled = true;
        playerTurnImage.enabled = true;
    }

    public void Swap(bool isSwap)
    {

        if (isSwap)
        {
            if (firstAttempt)
            {
                icon1.enabled = true;
                icon2.enabled = true;
                firstAttempt = false;
            }
            else
            {
                icon1.SetTrigger("swap");
                icon2.SetTrigger("swap");
            }
        }
        
    }

    public void Close()
    {
        playerTurnImage.enabled = false;
        enemyTurnImage.enabled = false;
    }
}
