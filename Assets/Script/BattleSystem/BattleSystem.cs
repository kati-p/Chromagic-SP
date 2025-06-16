using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST}

public enum MagicState { NONE, CHARGING, FIRING, BLASTING, DEFENCING }

public class BattleSystem : MonoBehaviour
{
    public PlayerBattle playerUnit;
    public GameObject playerGO;

    public EnemyBattle enemyUnit;
    public GameObject enemyGO;
    public GameObject enemyAura;

    public MagicBookController magicBook;

    public GameObject magicPrefab;
    public GameObject magicMixingPrefab;

    public Animator transition;

    [SerializeField]
    private TextMeshProUGUI turnNumberText;

    [SerializeField]
    private TextMeshProUGUI actionText;

    [SerializeField]
    private Animator enterActionButtonAnimation;

    [SerializeField]
    private GameObject notificationSpawner;

    [SerializeField]
    private GameObject notificationPrefab;

    [SerializeField]
    private GameObject AZField;

    [SerializeField]
    private TurnIcon turnIcon;

    [SerializeField]
    private StatusWindow enemyStatusWindow;
    [SerializeField]
    private StatusWindow playerStatusWindow;
    [SerializeField]
    private StatusLevelUpWindow statusLevelUpWindow;

    [SerializeField]
    private GameObject magicBookSparking;

    [SerializeField]
    private const int APStart = 3;

    // sound FX
    [SerializeField]
    private AudioClip magicChargingSFX;
    [SerializeField]
    private AudioClip magicFiringSFX;
    [SerializeField]
    private AudioClip magicBlastingSFX;
    [SerializeField]
    private AudioClip magicSparkingSFX;
    [SerializeField]
    private AudioClip magicMixingSFX;
    [SerializeField]
    private AudioClip wonSFX;
    [SerializeField]
    private AudioClip lostSFX;
    [SerializeField]
    private AudioClip notificationSFX;
    [SerializeField]
    private AudioClip typingSFX;
    [SerializeField]
    private AudioClip refillAPSFX;
    [SerializeField]
    private AudioClip enterSFX;
    [SerializeField]
    private AudioClip moveSFX;
    [SerializeField]
    private AudioClip moveMagicBookPageSFX;
    [SerializeField]
    private AudioClip castSheildSFX;
    [SerializeField]
    private AudioClip levelUpSFX;

    public BattleState battleState;
    public MagicState magicState;
    public bool isAttacking = false;
    private bool isSkiping = false;
    private bool canControl = true;

    private string inputKeyboard = "";
    private int turnNumber = 0;
    private int APIncrease = 0;
    private int statusWindowPointer = 0;
    private GameObject playerMagicGO;
    private GameObject playerMagicDefenceGO;
    private Magic playerMagic;
    private IEnumerator runningSlider;

    private AudioSource sparkingAS;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        StartCoroutine(SetUpBattle());
    }

    // Update is called once per frame
    void Update()
    {

        // Slider
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            StopSlideSelecter();

            runningSlider = SlideSelecter(
                    () => magicBook.DecreaseSelectedIndex()
                );
            StartCoroutine(runningSlider);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            StopSlideSelecter();

            runningSlider = SlideSelecter(
                    () => magicBook.IncreaseSelectedIndex()
                );
            StartCoroutine(runningSlider);
        }
        if ((Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
            && !(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)))
        {
            StopSlideSelecter();
        }

        // Arrow Down
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            magicBook.ToggleManualOrMagicBook();
            SoundManager.Instance.PlaySoundFXClip(moveSFX, 1f, transform);
        }

        // Arrow Up
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            HandleStatusWindow();
            SoundManager.Instance.PlaySoundFXClip(moveSFX, 1f, transform);
        }

        // Slider with mouse
        if (Input.GetAxis("Mouse ScrollWheel") < 0f) // scroll mouse backward
        {
            magicBook.IncreaseSelectedIndex();    
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0f) // scroll mouse forward
        {
            magicBook.DecreaseSelectedIndex();
        }


        // Press Enter
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            HandlePressEnter();
        }

        // Action Button
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HandlePressActionButton();
        }

        // receive a-z 
        foreach (char c in Input.inputString)
        {
            if (canControl && (battleState == BattleState.PLAYERTURN || battleState == BattleState.ENEMYTURN)) {
                if (c == '\b')  // Backspace
                {
                    if (inputKeyboard.Length > 0)
                    {
                        inputKeyboard = inputKeyboard.Substring(0, inputKeyboard.Length - 1);
                        HandleAZInputChange();
                    }
                }
                else if (char.IsLetter(c) && c >= 'a' && c <= 'z')
                {
                    inputKeyboard += c;
                    HandleAZInputChange();
                }
                else if (char.IsLetter(c) && c >= 'A' && c <= 'Z')
                {
                    inputKeyboard += char.ToLower(c);
                    HandleAZInputChange();
                }
            }        
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            SceneManager.LoadScene("OverWorld");
        }

    }

    // Slider
    [SerializeField] 
    private const float slideVelocity = 0.06f;
    [SerializeField] 
    private const float startSlideDelay = 0.25f;
    [SerializeField] 
    private const float maxSlideDelay = 0.00719424f;
    float slideDelay = startSlideDelay;

    private IEnumerator SlideSelecter(Action action)
    {
        SoundManager.Instance.PlaySoundFXClip(moveMagicBookPageSFX, 1f, transform);

        // TODO change magic page animation

        slideDelay = startSlideDelay;

        while (true)
        {
            action();

            yield return new WaitForSeconds(slideDelay);

            if (slideDelay > maxSlideDelay)
            {
                slideDelay -= slideVelocity;
            }
            else if (slideDelay < maxSlideDelay)
            {
                slideDelay = maxSlideDelay;
            }
        }
    }

    private void StopSlideSelecter()
    {
        if (runningSlider != null)
        {
            StopCoroutine(runningSlider);
            runningSlider = null;
        }

        // TODO stop magic page animation
    }

    private IEnumerator DisableControl(float second)
    {
        canControl = false;

        yield return new WaitForSeconds(second);

        canControl = true;
    }

    private void HandleStatusWindow()
    {
        statusWindowPointer = (statusWindowPointer + 1) % 3;

        switch (statusWindowPointer)
        {
            case 0:
                playerStatusWindow.gameObject.SetActive(false); 
                enemyStatusWindow.gameObject.SetActive(false);
                break;
            case 1:
                playerStatusWindow.gameObject.SetActive(true);
                enemyStatusWindow.gameObject.SetActive(false);
                break;
            case 2:
                playerStatusWindow.gameObject.SetActive(false);
                enemyStatusWindow.gameObject.SetActive(true);
                break;
        }
    }

    private void RefillAllMagicCharge()
    {
        // use 10 action point to refill all magics' charge
        if (10 > playerUnit.GetAP())
        {
            StartCoroutine(UseNotification("No action point", 2f));

        }
        else
        {
            playerUnit.DecreaseAP(10);
            magicBook.ResetCharages();

            SoundManager.Instance.PlaySoundFXClip(refillAPSFX, 1f, transform);
        }      

        UndoAZFilter();
    }

    private void HandlePressEnter()
    {
        SoundManager.Instance.PlaySoundFXClip(enterSFX, 1f, transform);

        if (inputKeyboard == "")
        {
            magicBook.EnterOnSelectedIndex();

            return;
        }
        
        if (battleState != BattleState.PLAYERTURN && battleState != BattleState.ENEMYTURN)
        {
            return;
        }
        
        // input keyboard != ""
        if (playerMagic != null && playerMagic.ColorName == inputKeyboard)
        {
            if (battleState == BattleState.PLAYERTURN
                    && magicState == MagicState.CHARGING)
            {
                StartCoroutine(PlayerAttack());
            }
            else if (battleState == BattleState.ENEMYTURN)
            {
                PlayerDefense();
            }
        }
        else if (inputKeyboard == "refill")     // refill order
        {
            RefillAllMagicCharge();
        }
        else
        {
            StartCoroutine(UseNotification("Spelling error", 2f));
            UndoAZFilter();
        }        
    }

    private IEnumerator PlayAnimationEnterActionButton()
    {

        enterActionButtonAnimation.gameObject.SetActive(true);

        yield return new WaitForSeconds(enterActionButtonAnimation.GetCurrentAnimatorClipInfo(0)[0].clip.length);

        enterActionButtonAnimation.gameObject.SetActive(false);
    }

    private void HandlePressActionButton()
    {
        if (!canControl)
        {
            return;
        }
        
        StartCoroutine(PlayAnimationEnterActionButton());

        SoundManager.Instance.PlaySoundFXClip(enterSFX, 1f, transform);

        if (magicState == MagicState.CHARGING || magicState == MagicState.NONE)
        {
            if (battleState == BattleState.PLAYERTURN)
            {
                EndTurn();
            }
            else if (battleState == BattleState.ENEMYTURN)
            {
                isSkiping = true;
            }
        }

        if (battleState == BattleState.WON)
        {
            isSkiping = true;
        }

    }

    private void UndoAZFilter()
    {
        if (inputKeyboard != "")
        {
            inputKeyboard = "";
            AZField.SetActive(false);
            SetMagicBookSparking();
            playerUnit.StopBlinkAP();

            if (!isAttacking || battleState != BattleState.PLAYERTURN)
            {
                DestroyPlayerMagic();
            }
        }
        
    }

    private void DestroyPlayerMagic()
    {
        GameObject.Destroy(playerMagicGO);
        GameObject.Destroy(playerMagicDefenceGO);
        magicState = MagicState.NONE;
        playerMagic = null;
    }

    private void StopSparkingSFX()
    {
        if (sparkingAS != null)
        {
            sparkingAS.Stop();
        }
    }

    private void PlaySparkingSFX()
    {
        if (sparkingAS == null)
        {
            sparkingAS = SoundManager.Instance.PlaySoundFXClip(magicSparkingSFX, 1f, transform, true);
        }
        else
        {
            sparkingAS.Play();
        }
    }

    private void SetMagicBookSparking()
    {
        if (inputKeyboard == "" || playerMagic == null)
        {
            magicBookSparking.GetComponent<Animator>().SetFloat("MatchRatio", 0f);
            StopSparkingSFX();
            return;
        }
        
        // set match ratio
        int matchCount = 0;
        Magic selectedMagic = magicBook.GetMagicWithSelectedPage();
        Magic magicCompare = (selectedMagic != null && selectedMagic.ColorName.StartsWith(inputKeyboard, StringComparison.OrdinalIgnoreCase)) ? 
            selectedMagic : playerMagic;
        string strCompare = magicCompare.ColorName;
        for (int i = 0; i < inputKeyboard.Length; i++)
        {
            if (inputKeyboard[i] == strCompare[i])
            {
                matchCount++;
            }
        }

        float matchRatio = (float)matchCount / strCompare.Length;

        if (matchRatio > 0f)
        {
            magicBookSparking.GetComponent<Image>().color = magicCompare.Color;
            TextMeshProUGUI textUI = magicBookSparking.transform.Find("SparkingText").GetComponent<TextMeshProUGUI>();
            textUI.text = magicCompare.ColorName;
            textUI.color = magicCompare.BestContrast;

            ParticleSystem ps = magicBookSparking.transform.Find("SparkEffect").GetComponent<ParticleSystem>();
            var main = ps.main;
            Color lighterColor = Chromagic.CalculateColorMixing(magicCompare.Color, Color.white);
            main.startColor = lighterColor;
        }

        if (matchRatio >= 1f)
        {
            PlaySparkingSFX();
        }
        else
        {
            StopSparkingSFX();
        }

        magicBookSparking.GetComponent<Animator>().SetFloat("MatchRatio", matchRatio);
    }

    [SerializeField]
    private List<string> otherOrderString = new List<string> { "refill" };

    public string FilterOtherOrderByString(string filter)
    {
        string str = otherOrderString
            .Where(s => s.StartsWith(filter, StringComparison.OrdinalIgnoreCase))
            .OrderBy(s => s.Length > filter.Length)
            .FirstOrDefault();

        return str;
    }

    private void HandleAZInputChange()
    {
        if (!canControl)
        {
            return;
        }

        if (battleState != BattleState.PLAYERTURN && battleState != BattleState.ENEMYTURN)
        {
            return;
        }

        playerMagic = inputKeyboard == "" ? null : magicBook.FilterMagicChargingByString(inputKeyboard);

        if (playerMagic == null)
        {
            string otherOrder = FilterOtherOrderByString(inputKeyboard);

            if (otherOrder == null)
            {
                // undo filter
                inputKeyboard = "";
            }

            DestroyPlayerMagic();
            playerUnit.StopBlinkAP();
        }
        else
        {
            // playerMagic != null and inputKeyBoard != ""

            if (battleState == BattleState.ENEMYTURN && playerMagicDefenceGO == null)
            {
                playerMagicDefenceGO = SpawnMagicDefencing(playerGO, playerMagic);
            }

            if (playerMagicGO == null)
            {
                playerMagicGO = SpawnMagicCharging(playerGO, playerMagic);
            }
            else
            {
                ChangePlayerMagicColor(playerMagic);
            }
            
            if (magicState == MagicState.NONE)
            {
                magicState = MagicState.CHARGING;
            }

            if (battleState == BattleState.PLAYERTURN)
            {
                playerUnit.GoingDecreaseAP(playerMagic.Cost);
            }
        }

        if (inputKeyboard != "")
        {
            AZField.SetActive(true);
            AZField.transform.Find("InputText").GetComponent<TextMeshProUGUI>().text = inputKeyboard;

            SoundManager.Instance.PlaySoundFXClip(typingSFX, 1f, transform);
        }
        else
        {
            AZField.SetActive(false);
        }

        SetMagicBookSparking();
    }

    IEnumerator SetUpBattle()
    {
        battleState = BattleState.START;
        actionText.text = "-";

        turnIcon.Close();

        playerUnit.SetAP(0);
        enemyUnit.SetAP(0);

        List<Magic> playerMagics = GameData.Instance.PlayerData.Magics;

        playerUnit.SetUp();
        enemyUnit.SetUp();
        magicBook.SetUp(playerMagics.ToArray());
        AZField.SetActive(false);
        SetMagicBookSparking();

        AudioClip enemyTheme = GameData.Instance.EnemyEncouter.EnemyTheme;
        if (enemyTheme != null)
        {
            SoundManager.Instance.PlayMusic(enemyTheme, 1f);
        }
        else
        {
            SoundManager.Instance.StopMusic();
        }

        yield return UseNotification("Ready to Battle ?", 3f);

        EndTurn();
    }


    void EndTurn()
    {
        Debug.Log("END TURN");
        Debug.Log(battleState);

        UndoAZFilter();

        if (battleState == BattleState.START)
        {
            bool isPlayerTurn = playerUnit.SPD >= enemyUnit.SPD;
            turnIcon.SetUp(isPlayerTurn);
        }

        if (battleState == BattleState.WON || battleState == BattleState.LOST)
        {
            return;
        }

        bool isSwap = false;
        bool isEvenTurnNumber = turnNumber % 2 == 0;

        if (isEvenTurnNumber)
        {
            bool isPlayerMoreSpd = playerUnit.SPD >= enemyUnit.SPD;

            if (isPlayerMoreSpd)
            {
                if (battleState == BattleState.ENEMYTURN)
                {
                    isSwap = true;
                }
                StartCoroutine(PlayerTurn());
            }
            else
            {
                if (battleState == BattleState.PLAYERTURN)
                {
                    isSwap = true;
                }
                StartCoroutine(EnemyTurn());
            }

            SetBattleAPByDefaultMachanic(isPlayerMoreSpd);
        }
        else
        {
            if (battleState == BattleState.ENEMYTURN)
            {
                StartCoroutine(PlayerTurn());
            }
            else if (battleState == BattleState.PLAYERTURN)
            {    
                StartCoroutine(EnemyTurn());
            }

            isSwap = true;
        }

        turnIcon.Swap(isSwap);

        turnNumber++;
        turnNumberText.text = $"TURN NO : {turnNumber}";

    }

    private void SetBattleAPByDefaultMachanic(bool isPlayerMoreSpd)
    {

        int currentAP = APIncrease + APStart;
        float spdDiff;

        if (isPlayerMoreSpd)
        {
            spdDiff = playerUnit.SPD - enemyUnit.SPD;   
        }
        else
        {
            spdDiff = enemyUnit.SPD - playerUnit.SPD; 
        }

        // spdDiff range = (0, 95+)
        if (spdDiff > 95)
        {
            spdDiff = 95;
        }
        else if (spdDiff < 0)
        {
            Debug.LogError("speed different is less than 0.");
        }
        int apPlus = (int)Math.Round(spdDiff / 10, MidpointRounding.AwayFromZero);

        if (isPlayerMoreSpd)
        {
            playerUnit.SetAP(currentAP + apPlus);
            enemyUnit.SetAP(currentAP);
        }
        else
        {
            playerUnit.SetAP(currentAP);
            enemyUnit.SetAP(currentAP + apPlus);
        }

        // calculate AP's increase next time
        if (currentAP < 6)
        {
            APIncrease += 1;
        }
        else if (currentAP < 12)
        {
            APIncrease += 2;
        }
        else if (currentAP < 18)
        {
            APIncrease += 3;
        }
        else if (currentAP < 24)
        {
            APIncrease = 17;
        }
        
        magicBook.UpdateUI();
        
    }

    private IEnumerator Attack(CharacterBattle attackerUnit, CharacterBattle targetUnit, GameObject targetGO, GameObject magic, Magic attackMagic)
    {
        isAttacking = true;

        float firingTime = 1f;

        attackerUnit.DecreaseAP(attackMagic.Cost);
        if (battleState == BattleState.PLAYERTURN)
        {
            magicBook.UpdateUI();
        }

        magicState = MagicState.FIRING;
        yield return PlayMagicFiring(magic, targetGO, firingTime);

        magicState = MagicState.BLASTING;
        StartCoroutine(PlayMagicBlasting(magic));

        yield return new WaitForSeconds(0.2f);

        Color defenseColor = targetUnit.GetDefenseColor();
        float contrastRatio = Chromagic.CalculateContrastRatio(attackMagic.Color, defenseColor);
        float damageBase = attackerUnit.MP;
        float damage = (damageBase + attackMagic.AttackPower) * contrastRatio;

        bool isDead = targetUnit.TakeDamage(damage, attackMagic.Color, contrastRatio);

        if (isDead)
        {
            if (battleState == BattleState.PLAYERTURN)
            {
                StartCoroutine(BattleWon());
            }
            else if (battleState == BattleState.ENEMYTURN)
            {
                StartCoroutine(BattleLost());
            }
            yield break;
        }

        yield return new WaitForSeconds(0.8f);  // wait for animation

        // TODO CR reach condition

        if (contrastRatio >= 3f )
        {
            // color mixng
            RectTransform targetRt = targetGO.GetComponent<RectTransform>();
            Vector2 targetPosition = targetRt.anchoredPosition;

            Color mixedColor = Chromagic.CalculateColorMixing(attackMagic.Color, defenseColor);

            yield return PlayMagicMixing(targetPosition);

            isDead = targetUnit.TakeDamage(damage, mixedColor);

            yield return PlayNewMagicBlasting(targetPosition, mixedColor);

            targetUnit.ChangeDefenseColor(mixedColor);

        }

        if (isDead)
        {
            if (battleState == BattleState.PLAYERTURN)
            {
                StartCoroutine(BattleWon());
            }
            else if (battleState == BattleState.ENEMYTURN)
            {
                StartCoroutine(BattleLost());
            }
            yield break;
        }

        magicState = MagicState.NONE;

        isAttacking = false;
    }

    IEnumerator PlayerAttack()
    {
        if (playerMagicGO == null)
        {
            Debug.Log("Player's magic gameobject is null.");

            yield break;
        }
        if (playerMagic == null)
        {
            Debug.Log("Player's magic is null.");

            yield break;
        }
        
        if (playerMagic.Cost > playerUnit.GetAP())
        {
            StartCoroutine(UseNotification("No action point", 2f));
            UndoAZFilter();

            yield break;
        }

        if (magicBook.GetChargeByMagic(playerMagic) <= 0)
        {
            StartCoroutine(UseNotification("No charge", 2f));
            UndoAZFilter();

            yield break;
        }

        Magic attackMagic = playerMagic;
        Color defenseColor = enemyUnit.GetDefenseColor();

        magicBook.UseChargeByMagic(attackMagic);

        GameObject clonePlayerMagicGO = Instantiate(playerMagicGO, playerMagicGO.transform.position, Quaternion.identity);
        clonePlayerMagicGO.transform.SetParent(GameObject.FindGameObjectWithTag("Spawn Magic").transform, false);

        UndoAZFilter();
        magicState = MagicState.CHARGING;

        yield return Attack(playerUnit, enemyUnit, enemyGO, clonePlayerMagicGO, attackMagic);

        HandleAZInputChange();
    }

    void PlayerDefense()
    {
        Color color = playerMagic.Color;
        playerUnit.ChangeDefenseColor(color);

        SoundManager.Instance.PlaySoundFXClip(castSheildSFX, 1f, transform);

        UndoAZFilter();
    }

    IEnumerator PlayerTurn()
    {
        battleState = BattleState.PLAYERTURN;

        actionText.text = "-";

        StartCoroutine(DisableControl(3f));
        yield return UseNotification("YOUR TURN !", 2.5f);

        actionText.text = "END TURN";

        

    }

    float CalculateEnemyChargingTime ()
    {
        float playerSPDDiff = playerUnit.SPD - enemyUnit.SPD;
        if (playerSPDDiff > 100)
        {
            playerSPDDiff = 100;
        }
        if (playerSPDDiff < -100)
        {
            playerSPDDiff = -100;
        }
        // range of player's speed different (-100, 100) to enemy charging time (1, 40)
        // y = mx + c
        // y = 0.195x + 20.5
        const float m = 0.195f;
        const float c = 20.5f;

        float enemyChargingTime = (m * playerSPDDiff) + c;

        return enemyChargingTime;
    }

    IEnumerator EnemyTurn()
    {
        battleState = BattleState.ENEMYTURN;

        actionText.text = "-";

        StartCoroutine(DisableControl(2.5f));
        yield return UseNotification("ENEMY TURN !", 2.5f);

        actionText.text = "RESOLVE ATTACK";

        float enemyChargingTime = CalculateEnemyChargingTime();

        // enemy script
        int enemyMagicCanUse = enemyUnit.GetMagicCountCanUseByAP();

        if (enemyMagicCanUse <= 0) 
        {
            Debug.Log("Enemy: ??? (No magics can use when start.)");
        }

        yield return new WaitForSeconds(1f);

        while (enemyMagicCanUse > 0)
        {
            Magic randomMagic = enemyUnit.RandomMagicCanUseByAP();
            
            magicState = MagicState.CHARGING;
            GameObject enemyMagic = SpawnMagicCharging(enemyGO, randomMagic);

            enemyUnit.GoingDecreaseAP(randomMagic.Cost);
            yield return WaitEnemyAttackUntillSkip(enemyChargingTime);
            enemyUnit.StopBlinkAP();

            yield return Attack(enemyUnit, playerUnit, playerGO, enemyMagic, randomMagic);

            // defence
            GameObject enemyDefenceMagic1 = SpawnMagicCharging(enemyGO, randomMagic);
            GameObject enemyDefenceMagic2 = SpawnMagicDefencing(enemyGO, randomMagic);
            enemyDefenceMagic2.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

            SoundManager.Instance.PlaySoundFXClip(castSheildSFX, 1f, transform);

            yield return new WaitForSeconds(1f);
            GameObject.Destroy(enemyDefenceMagic1);
            GameObject.Destroy(enemyDefenceMagic2);
            enemyUnit.ChangeDefenseColor(randomMagic.Color);

            enemyMagicCanUse = enemyUnit.GetMagicCountCanUseByAP();
        }

        actionText.text = "-";
        yield return new WaitForSeconds(2.5f);    // wait for magic animation

        EndTurn();
    }

    private void LevelUpPlayerAndShowStatusLevelUpWindow()
    {
        CharacterData playerData = GameData.Instance.PlayerData;
        PlayerStatLevelUp newStat = GameData.Instance.EnemyEncouter.PlayerStatLevelUpWhenWon;

        float newHP = playerData.StartedHealthPower + newStat.increaseStartedHP;
        float newMP = playerData.MagicPower + newStat.increaseMP;
        float newMD = playerData.MagicDefense + newStat.increaseMD;
        float newSPD = playerData.Speed + newStat.increaseSPD;

        statusLevelUpWindow.SetUpLevelUp(playerData.CharacterName, playerData.StartedHealthPower, playerData.MagicPower, playerData.MagicDefense, playerData.Speed, newHP, newMP, newMD, newSPD);

        statusLevelUpWindow.gameObject.SetActive(true);

        SoundManager.Instance.PlaySoundFXClip(levelUpSFX, 1f, transform);

        playerData.StartedHealthPower = newHP;
        playerData.MagicPower = newMP;
        playerData.MagicDefense = newMD;
        playerData.Speed = newSPD;
    }

    IEnumerator BattleWon()
    {
        battleState = BattleState.WON;
        actionText.text = "-";

        turnIcon.Close();
        UndoAZFilter();

        SoundManager.Instance.StopMusic();
        SoundManager.Instance.PlaySoundFXClip(wonSFX, 1f, transform);

        yield return UseNotification("YOU WON !", 3f);

        yield return UseNotification("Level Up !", 1f);

        LevelUpPlayerAndShowStatusLevelUpWindow();
        actionText.text = "OK";
        yield return WaitUntillSkip(15f);
        
        transition.SetTrigger("End");
        yield return new WaitForSeconds(1);

        GameData gameData = GameData.Instance;
        gameData.EnemiesDefeated[enemyUnit.EnemyId] = true;
        gameData.PlayerPosition = GameData.Instance.EnemyEncouterPosition;
        gameData.GameProgress += 1;

        gameData.SaveGame();

        SceneManager.LoadScene("OverWorld");
    }

    IEnumerator BattleLost()
    {
        battleState = BattleState.LOST;
        actionText.text = "-";

        turnIcon.Close();
        UndoAZFilter();

        SoundManager.Instance.StopMusic();
        SoundManager.Instance.PlaySoundFXClip(lostSFX, 1f, transform);

        yield return UseNotification("YOU LOST!", 5f);

        transition.SetTrigger("End");
        yield return new WaitForSeconds(1);

        SceneManager.LoadScene("MainMenu");
    }

    private IEnumerator WaitUntillSkip(float timeWait)
    {
        isSkiping = false;
        float elapsedTime = 0f;

        while (!isSkiping && elapsedTime < timeWait)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator WaitEnemyAttackUntillSkip(float timeWait)
    {
        isSkiping = false;
        float elapsedTime = 0f;
        float percentTime = (elapsedTime / timeWait) * 100;

        enemyAura.SetActive(true);

        while (!isSkiping && elapsedTime < timeWait)
        {
            percentTime = (elapsedTime / timeWait) * 100;

            enemyAura.GetComponent<Animator>().SetFloat("percent", percentTime);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        enemyAura.SetActive(false);
        enemyAura.GetComponent<Animator>().SetFloat("percent", 0);
    }

    IEnumerator UseNotification (string text, float notificationTime)
    {
        GameObject notification = Instantiate(notificationPrefab, Vector2.zero, Quaternion.identity);
        notification.transform.SetParent(notificationSpawner.transform, false);
        notification.transform.Find("NotificationText").GetComponent<TextMeshProUGUI>().text = text;

        SoundManager.Instance.PlaySoundFXClip(notificationSFX, 1f, transform);

        yield return new WaitForSeconds(notificationTime);

        Animator notificationAnimator = notification.GetComponent<Animator>();
        notificationAnimator.SetTrigger("Close");
        yield return new WaitForSeconds(notificationAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length);

        Destroy(notification);
    }

    private GameObject SpawnMagicCharging(GameObject unitCast, Magic magic)
    {
        RectTransform rt = unitCast.GetComponent<RectTransform>();

        Vector2 spawnPosition = rt.anchoredPosition + new Vector2(250, 50);
        GameObject magicSpawned = Instantiate(magicPrefab, spawnPosition, Quaternion.identity);
        magicSpawned.transform.SetParent(GameObject.FindGameObjectWithTag("Spawn Magic").transform, false);
        
        Image magicImage = magicSpawned.GetComponent<Image>();
        magicImage.color = magic.Color;

        SoundManager.Instance.PlaySoundFXClip(magicChargingSFX, 1f, transform);

        return magicSpawned;

    }

    private GameObject SpawnMagicDefencing(GameObject unitCast, Magic magic)
    {
        RectTransform rt = unitCast.GetComponent<RectTransform>();

        Vector2 spawnPosition = rt.anchoredPosition;
        GameObject magicSpawned = Instantiate(magicPrefab, spawnPosition, Quaternion.identity);
        magicSpawned.transform.SetParent(GameObject.FindGameObjectWithTag("Spawn Magic").transform, false);

        Image magicImage = magicSpawned.GetComponent<Image>();
        magicImage.color = magic.Color;

        Animator magicAnimation = magicSpawned.GetComponent<Animator>();
        magicAnimation.SetTrigger("Defencing");

        return magicSpawned;
    }

    private IEnumerator PlayMagicFiring(GameObject magic,  GameObject targetGO, float firingTime)
    {
        Animator magicAnimation = magic.GetComponent<Animator>();
        RectTransform magicRt = magic.GetComponent<RectTransform>();
        Vector2 startPosition = magicRt.position;
        RectTransform targetRt = targetGO.GetComponent<RectTransform>();
        Vector2 targetPosition = targetRt.position;

        if (magicRt.anchoredPosition.y > targetRt.anchoredPosition.y) 
        {
            magicRt.localRotation = Quaternion.Euler(0, 0, 180);
        }

        magicAnimation.SetTrigger("Firing");

        SoundManager.Instance.PlaySoundFXClip(magicFiringSFX, 1f, transform);

        float elapsedTime = 0f;
        while (elapsedTime < firingTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / firingTime); // Normalize time
            magicRt.position = Vector2.Lerp(startPosition, targetPosition, t);
            yield return null; // Wait for the next frame
        }
    }

    private IEnumerator PlayMagicBlasting(GameObject magic)
    {
        Animator magicAnimation = magic.GetComponent<Animator>();
        magicAnimation.SetTrigger("Blasting");

        SoundManager.Instance.PlaySoundFXClip(magicBlastingSFX, 1f, transform);

        yield return new WaitForSeconds(1f);    // animation time

        Destroy(magic);
    }

    private void ChangePlayerMagicColor(Magic magic)
    {
        if (playerMagicGO != null)
        {
            Image magicImage = playerMagicGO.GetComponent<Image>();
            magicImage.color = magic.Color;
        }

        if (playerMagicDefenceGO != null)
        {
            Image magicImage = playerMagicDefenceGO.GetComponent<Image>();
            magicImage.color = magic.Color;
        }
    }

    private IEnumerator PlayMagicMixing(Vector2 targetPosition)
    {
        // play mixing process
        GameObject mixing = Instantiate(magicMixingPrefab, targetPosition, Quaternion.identity);
        mixing.transform.SetParent(GameObject.FindGameObjectWithTag("Spawn Magic").transform, false);
        const float animTime = 1f;
        const float frequency = 0.02f;

        UIGradient uIGradient = mixing.GetComponent<UIGradient>();

        Color[] rndColor = Chromagic.RandomColor();

        uIGradient.m_color1 = rndColor[0];
        uIGradient.m_color2 = rndColor[1];

        SoundManager.Instance.PlaySoundFXClip(magicMixingSFX, 1f, transform);

        float elapsedTime = 0f;
        float lerpTime = 0f;
        int index = 2;
        while (elapsedTime < animTime)
        {
            elapsedTime += Time.deltaTime;
            lerpTime += Time.deltaTime;

            if (lerpTime > frequency)
            {
                if (index % 2 == 0)
                {
                    uIGradient.m_color1 = rndColor[index];
                }
                else
                {
                    uIGradient.m_color2 = rndColor[index];
                }

                lerpTime = 0f;
                index++;
            }
            if (index >= rndColor.Length - 1)
            {
                break;
            }

            yield return null; // Wait for the next frame
        }
    }

    private IEnumerator PlayNewMagicBlasting(Vector2 targetPosition, Color color)
    {
        SoundManager.Instance.PlaySoundFXClip(magicBlastingSFX, 1f, transform);

        GameObject magic = Instantiate(magicPrefab, targetPosition, Quaternion.identity);
        magic.transform.SetParent(GameObject.FindGameObjectWithTag("Spawn Magic").transform, false);

        Image magicImage = magic.GetComponent<Image>();
        magicImage.color = color;

        yield return PlayMagicBlasting(magic);
    }
}
