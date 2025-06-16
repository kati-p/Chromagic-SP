using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class MagicOrder
{
    private string titleOnToggled;
    private string titleOffToggled;
    public string TitleOnToggled { get { return titleOnToggled; } set { titleOnToggled = value; } }
    public string TitleOffToggled { get { return titleOffToggled; } set { titleOffToggled = value; } }
    public string Title { get { return OnToggled ? titleOnToggled : titleOffToggled; } }
    public string Description { get; set; }
    public Color? PageColor { get; set; }
    public Color TextColor { get; set; }
    public bool OnToggled { get; set; }
    public bool isShowPressEnter { get; set; }
    public int Cost { get; set; }

    public MagicOrder(string titileOnToggled, string titleOffToggled, string description, Color? pageColor, Color textColor, bool onToggled, bool isShowPressEnter,int cost = 0)
    {
        this.titleOnToggled = titileOnToggled;
        this.titleOffToggled = titleOffToggled;
        this.Description = description;
        this.PageColor = pageColor;
        this.TextColor = textColor;
        this.OnToggled = onToggled;
        this.isShowPressEnter = isShowPressEnter;
        this.Cost = cost;
    }
}

public class MagicBookController : MonoBehaviour
{
    [SerializeField] 
    private List<MagicPageController> pages = new List<MagicPageController>();

    [SerializeField] 
    private MagicPageController pageSelecting;

    [SerializeField]
    private int leftPage = 6;

    [SerializeField]
    private int rightPage = 6;

    [SerializeField]
    private Animator enterAnimaiton;

    public APController playerAP;

    /*
    * DATA STRCUTURE
    * ARRAY 2D
    * 
    * Row                          Column
    * 
    * (0) RedGroup          Order<Red>(0)
    * (1) RedMagic          Magic<id1>(0) Magic<id2>(1) Magic<id3>(2) ...
    * (2) PinkGroup         Order<Pink>(0)
    * (3) PinkMagic         Magic<id1>(0) Magic<id2>(1) Magic<id3>(2) ...
    * (4) OrageGroup        Order<Orage>(0)
    * (5) OrageMagic        Magic<id1>(0) Magic<id2>(1) Magic<id3>(2) ...
    * (6) YellowGroup       Order<Yellow>(0)
    * (7) YellowMagic       Magic<id1>(0) Magic<id2>(1) Magic<id3>(2) ...
    * (8) PurpleGroup       Order<Puple>(0)
    * (9) PurpleMagic       Magic<id1>(0) Magic<id2>(1) Magic<id3>(2) ...
    * (10) MidSelectGroup    Order<MidSelectGroup>(0)
    * (11) GreenGroup        Order<Green>(0)
    * (12) GreenMagic       Magic<id1>(0) Magic<id2>(1) Magic<id3>(2) ...
    * (13) BlueGroup        Order<Blue>(0)
    * (14) BlueMagic        Magic<id1>(0) Magic<id2>(1) Magic<id3>(2) ...
    * (15) BrownGroup       Order<Brown>(0)
    * (16) BrownMagic       Magic<id1>(0) Magic<id2>(1) Magic<id3>(2) ...
    * (17) WhiteGroup       Order<White>(0)
    * (18) WhiteMagic       Magic<id1>(0) Magic<id2>(1) Magic<id3>(2) ...
    * (19) GrayGroup        Order<Gray>(0)
    * (20) GrayMagic        Magic<id1>(0) Magic<id2>(1) Magic<id3>(2) ...
    * */

    // object contains Magic or MagicOrder
    private List<List<object>> magicBookDetails = new List<List<object>>();
    private int currentIndexRow = 0;
    private int currentIndexColumn = 0;
    private int midIndexRow = 0;
    private List<bool> rowIsShow = new List<bool>();
    private List<int> indicesColorGroupRow = new List<int>();
    private List<int> indicesMagicColorRow = new List<int>();

    private List<List<object>> manualBookDetails = new List<List<object>>();
    private bool isSwitchToManualBook = false;

    List<List<object>> bookDetails;

    private Magic[] playerMagics;
    
    // (magicId, charge)
    private Dictionary<int, int> chargesDictionary = new Dictionary<int, int>();

    private Dictionary<int, Magic> chromagicDictionary;

    private int[] redGroupMagicId = 
    {
        39,
        61,
        62,
        67,
        75,
        28,
        38,
        18,
        8
    };

    private int[] pinkGroupMagicId =
    {
        90,
        88,
        59,
        45,
        25,
        51
    };

    private int[] orangeGroupMagicId =
    {
        63,
        55,
        47,
        65,
        76
    };

    private int[] yellowGroupMagicId =
    {
        95,
        125,
        137,
        129,
        127,
        118,
        110,
        101,
        109,
        106,
        73
    };

    private int[] purpleGroupMagicId =
    {
        111,
        87,
        72,
        66,
        56,
        50,
        41,
        43,
        14,
        20,
        19,
        22,
        13,
        9,
        5,
        26,
        10,
        34
    };

    private int[] greenGroupMagicId = 
    {
        112,
        103,
        100,
        97,
        71,
        107,
        94,
        96,
        99,
        58,
        32,
        31,
        27,
        15,
        80,
        42,
        33,
        21,
        79,
        69,
        60,
        35,
        30
    };

    private int[] blueGroupMagicId =
    {
        108,
        131,
        104,
        114,
        89,
        81,
        78,
        52,
        36,
        84,
        93,
        91,
        85,
        86,
        70,
        49,
        54,
        29,
        12,
        6,
        3,
        2,
        4
    };

    private int[] brownGroupMagicId =
    {
        128,
        117,
        113,
        105,
        102,
        82,
        77,
        57,
        74,
        68,
        48,
        53,
        46,
        16,
        23,
        17,
        7
    };

    private int[] whiteGroupMagicId =
    {
        139,
        134,
        133,
        136,
        135,
        126,
        130,
        122,
        124,
        120,
        123,
        132,
        138,
        116,
        119,
        121,
        115
    };

    private int[] grayGroupMagicId =
    {
        98,
        92,
        83,
        64,
        40,
        24,
        44,
        37,
        11,
        1
    };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void UpdateUI()
    {
        // selected page
        object selectedMagicBookDetail = bookDetails[currentIndexRow][currentIndexColumn];
        if (selectedMagicBookDetail is MagicOrder selectedOrder)
        {
            pageSelecting.UpdateUI(selectedOrder.Title, selectedOrder.Description, selectedOrder.PageColor, selectedOrder.TextColor, selectedOrder.isShowPressEnter, selectedOrder.Cost);
        }
        else if (selectedMagicBookDetail is Magic selectedMagic)
        {
            int hasCharge = chargesDictionary[selectedMagic.magicID];
            pageSelecting.UpdateUI(selectedMagic, hasCharge, IsMagicCanUse(selectedMagic, hasCharge));
        }
        else if (selectedMagicBookDetail == null)
        {
            Debug.LogError("selected magic book detail is null.");
            pageSelecting.UpdateUI();
        }

        int left = 1;
        int right = 1;

        // left pages
        int indexLeftRow = 0;
        int indexLeftColumn = 0;
        (indexLeftRow, indexLeftColumn) = FindBeforeRowColumnIndex(currentIndexRow, currentIndexColumn);
        while (left <= leftPage)
        {
            int indexPage = leftPage - left;

            if (indexLeftRow >= 0 && indexLeftColumn >= 0 && indexLeftRow < bookDetails.Count && indexLeftColumn < bookDetails[indexLeftRow].Count)
            {
                object currentMagicBookDetail = bookDetails[indexLeftRow][indexLeftColumn];
                if (currentMagicBookDetail is MagicOrder currentOrder)
                {
                    pages[indexPage].UpdateUI(currentOrder.Title, currentOrder.Description, currentOrder.PageColor, currentOrder.TextColor, currentOrder.isShowPressEnter, currentOrder.Cost);
                }
                else if (currentMagicBookDetail is Magic currentMagic)
                {
                    int hasCharge = chargesDictionary[currentMagic.magicID];
                    pages[indexPage].UpdateUI(currentMagic, hasCharge, IsMagicCanUse(currentMagic, hasCharge));
                }
                else if (currentMagicBookDetail == null)
                {
                    Debug.LogError("selected magic book detail is null.");
                    pages[indexPage].UpdateUI();
                }
            }
            else
            {
                pages[indexPage].UpdateUI();
            }

            left++;
            (indexLeftRow, indexLeftColumn) = FindBeforeRowColumnIndex(indexLeftRow, indexLeftColumn);
        }

        // right pages
        int indexRightRow = 0;
        int indexRightColumn = 0;
        (indexRightRow, indexRightColumn) = FindNextRowColumnIndex(currentIndexRow, currentIndexColumn);
        while (right <= rightPage)
        {
            int indexPage = leftPage - 1 + right;

            if (indexRightRow >= 0 && indexRightColumn >= 0 && indexRightRow < bookDetails.Count && indexRightColumn < bookDetails[indexRightRow].Count)
            {
                object currentMagicBookDetail = bookDetails[indexRightRow][indexRightColumn];
                if (currentMagicBookDetail is MagicOrder currentOrder)
                {
                    pages[indexPage].UpdateUI(currentOrder.Title, currentOrder.Description, currentOrder.PageColor, currentOrder.TextColor, currentOrder.isShowPressEnter, currentOrder.Cost);
                }
                else if (currentMagicBookDetail is Magic currentMagic)
                {
                    int hasCharge = chargesDictionary[currentMagic.magicID];
                    pages[indexPage].UpdateUI(currentMagic, hasCharge, IsMagicCanUse(currentMagic, hasCharge));
                }
                else if (currentMagicBookDetail == null)
                {
                    Debug.LogError("selected magic book detail is null.");
                    pages[indexPage].UpdateUI();
                }
            }
            else
            {
                pages[indexPage].UpdateUI();
            }

            right++;
            (indexRightRow, indexRightColumn) = FindNextRowColumnIndex(indexRightRow, indexRightColumn);
        }
    }

    // return -1 when out of bound
    private (int, int) FindNextRowColumnIndex(int row, int column)
    {
        if (row < 0 || column < 0)
        {
            return (-1, -1);
        }
        
        column++;   // next in column
        if (column >= bookDetails[row].Count)
        {
            row++;
            if (row >= bookDetails.Count)
            {
                return (-1, -1);
            }
            else
            {
                column = 0;
            }

            // skip when row is not show
            while (!rowIsShow[row])
            {
                row++;

                if (row >= bookDetails.Count)
                {
                    return (-1, -1);
                }
                //else
                //{
                //    column = 0;
                //}
            }
           
        }

        return (row, column);
    }

    // return -1 when out of bound
    private (int, int) FindBeforeRowColumnIndex(int row, int column)
    {
        if (row < 0 || column < 0)
        {
            return (-1, -1);
        }

        column--;   // before in column
        if (column < 0)
        {
            row--;
            if (row < 0)
            {
                return (-1, -1);
            }
            else
            {
                column = bookDetails[row].Count - 1;
            }

            // skip when row is not show
            while (!rowIsShow[row])
            {
                row--;

                if (row < 0)
                {
                    return (-1, -1);
                }
                else
                {
                    column = bookDetails[row].Count - 1;
                }

            }

        }

        return (row, column);
    }

    public void IncreaseSelectedIndex()
    {
        int nextIndexRow = 0;
        int nextIndexColumn = 0;
        (nextIndexRow, nextIndexColumn) = FindNextRowColumnIndex(currentIndexRow, currentIndexColumn);
        if (nextIndexRow >= 0 && nextIndexColumn >= 0)
        {
            (currentIndexRow, currentIndexColumn) = (nextIndexRow, nextIndexColumn);
            UpdateUI();
        }
    }

    public void DecreaseSelectedIndex()
    {
        int nextIndexRow = 0;
        int nextIndexColumn = 0;
        (nextIndexRow, nextIndexColumn) = FindBeforeRowColumnIndex(currentIndexRow, currentIndexColumn);
        if (nextIndexRow >= 0 && nextIndexColumn >= 0)
        {
            (currentIndexRow, currentIndexColumn) = (nextIndexRow, nextIndexColumn);
            UpdateUI();
        }
    }

    public void ToggleManualOrMagicBook()
    {
        isSwitchToManualBook = !isSwitchToManualBook;

        if (isSwitchToManualBook)
        {
            bookDetails = manualBookDetails;
            (currentIndexRow, currentIndexColumn) = (0, 0);
        }
        else
        {
            bookDetails = magicBookDetails;
            (currentIndexRow, currentIndexColumn) = (midIndexRow, 0);
        }

        UpdateUI();
    }

    public  IEnumerator PlayAnimationEnter()
    {
        enterAnimaiton.gameObject.SetActive(true);

        yield return new WaitForSeconds(enterAnimaiton.GetCurrentAnimatorClipInfo(0)[0].clip.length);

        enterAnimaiton.gameObject.SetActive(false);
    }

    public void EnterOnSelectedIndex()
    {
        if (isSwitchToManualBook)
        {
            return;
        }

        /*
        * Row
        * 
        * (0) RedGroup
        * (1) RedMagic
        * (2) PinkGroup
        * (3) PinkMagic
        * (4) OrageGroup
        * (5) OrageMagic
        * (6) YellowGroup
        * (7) YellowMagic
        * (8) PurpleGroup
        * (9) PurpleMagic
        * (10) MidSelectGroup
        * (11) GreenGroup
        * (12) GreenMagic
        * (13) BlueGroup
        * (14) BlueMagic
        * (15) BrownGroup
        * (16) BrownMagic
        * (17) WhiteGroup
        * (18) WhiteMagic
        * (19) GrayGroup
        * (20) GrayMagic
        * 
        * */
        switch (currentIndexRow)
        {
            case 0:
            case 2:
            case 4:
            case 6:
            case 8:
            case 11:
            case 13:
            case 15:
            case 17:
            case 19:
                // colors group
                rowIsShow[currentIndexRow + 1] = !rowIsShow[currentIndexRow + 1];
                if (magicBookDetails[currentIndexRow][0] is MagicOrder colorOrder)
                {
                    colorOrder.OnToggled = !colorOrder.OnToggled;
                    StartCoroutine(PlayAnimationEnter());
                }
                UpdateUI();
                break;
            case 10:
                // mid selected group
                if (magicBookDetails[10][0] is MagicOrder midOrder)
                {
                    if (midOrder.OnToggled)
                    {
                        // wanna group
                        foreach (int i in indicesColorGroupRow)
                        {
                            rowIsShow[i] = true;
                            if (magicBookDetails[i][0] is MagicOrder colorGroupOrder)
                            {
                                colorGroupOrder.OnToggled = false;
                            }
                        }
                        foreach (int i in indicesMagicColorRow)
                        {
                            rowIsShow[i] = false;
                        }
                    }
                    else
                    {
                        // wanna ungroup
                        foreach (int i in indicesColorGroupRow)
                        {
                            rowIsShow[i] = false;
                        }
                        foreach (int i in indicesMagicColorRow)
                        {
                            rowIsShow[i] = true;
                        }
                    }

                    midOrder.OnToggled = !midOrder.OnToggled;
                    StartCoroutine(PlayAnimationEnter());
                }
                UpdateUI();
                break;
            // others is magic. do not execute
        }
    }

    private void SetUpOrderColorToMagicBookDetails(int[] playerMagicsId, Magic colorGroup, int[] colorGroupMagicId)
    {
        int[] colorGroupMagicIdIntersected = colorGroupMagicId.Intersect(playerMagicsId).ToArray();

        // ColorGroup in row
        // value is struct Order
        magicBookDetails.Add(new List<object> { 
            new MagicOrder(
                $"contract {colorGroup.ColorName}",
                $"expand {colorGroup.ColorName}",
                "", 
                colorGroup.Color, 
                colorGroup.BestContrast,
                false,
                true) });
        rowIsShow.Add(false);
        int indexColorGroup = magicBookDetails.Count - 1;
        indicesColorGroupRow.Add(indexColorGroup);

        // MagicColor in next row
        magicBookDetails.Add(new List<object>());
        rowIsShow.Add(true);
        int indexMagicColor = magicBookDetails.Count - 1;
        indicesMagicColorRow.Add(indexMagicColor);
        foreach (int magicId in colorGroupMagicIdIntersected)
        {
            // value is class Magic
            magicBookDetails[indexMagicColor].Add(chromagicDictionary[magicId]);
        }
    }

    public void SetUp(Magic[] playerMagics)
    {
        this.playerMagics = playerMagics;

        chromagicDictionary = Chromagic.Instance.MagicsDictionaryById;
        // create ARRAY2D
        /*
        * Row                          Column
        * 
        * (0) RedGroup          Order<Red>(0)
        * (1) RedMagic          Magic<id1>(0) Magic<id2>(1) Magic<id3>(2) ...
        * (2) PinkGroup         Order<Pink>(0)
        * (3) PinkMagic         Magic<id1>(0) Magic<id2>(1) Magic<id3>(2) ...
        * (4) OrageGroup        Order<Orage>(0)
        * (5) OrageMagic        Magic<id1>(0) Magic<id2>(1) Magic<id3>(2) ...
        * (6) YellowGroup       Order<Yellow>(0)
        * (7) YellowMagic       Magic<id1>(0) Magic<id2>(1) Magic<id3>(2) ...
        * (8) PurpleGroup       Order<Puple>(0)
        * (9) PurpleMagic       Magic<id1>(0) Magic<id2>(1) Magic<id3>(2) ...
        * (10) MidSelectGroup    Order<MidSelectGroup>(0)
        * (11) GreenGroup        Order<Green>(0)
        * (12) GreenMagic       Magic<id1>(0) Magic<id2>(1) Magic<id3>(2) ...
        * (13) BlueGroup        Order<Blue>(0)
        * (14) BlueMagic        Magic<id1>(0) Magic<id2>(1) Magic<id3>(2) ...
        * (15) BrownGroup       Order<Brown>(0)
        * (16) BrownMagic       Magic<id1>(0) Magic<id2>(1) Magic<id3>(2) ...
        * (17) WhiteGroup       Order<White>(0)
        * (18) WhiteMagic       Magic<id1>(0) Magic<id2>(1) Magic<id3>(2) ...
        * (19) GrayGroup        Order<Gray>(0)
        * (20) GrayMagic        Magic<id1>(0) Magic<id2>(1) Magic<id3>(2) ...
        * */

        // red pink orange yellow purple | Select group | green blue brown white gray
        int[] playerMagicsId = playerMagics.Select(m => m.magicID).ToArray();
        foreach (int magicId in playerMagicsId)
        {
            chargesDictionary[magicId] = 1;
        }

        // red
        Magic redMagic = chromagicDictionary[38];
        SetUpOrderColorToMagicBookDetails(playerMagicsId, redMagic, redGroupMagicId);

        // pink
        Magic pinkMagic = chromagicDictionary[90];
        SetUpOrderColorToMagicBookDetails(playerMagicsId, pinkMagic, pinkGroupMagicId);

        // orange
        Magic orangeMagic = chromagicDictionary[76];
        SetUpOrderColorToMagicBookDetails(playerMagicsId, orangeMagic, orangeGroupMagicId);

        // yellow
        Magic yellowMagic = chromagicDictionary[125];
        SetUpOrderColorToMagicBookDetails(playerMagicsId, yellowMagic, yellowGroupMagicId);

        // purple
        Magic purpleMagic = chromagicDictionary[9];
        SetUpOrderColorToMagicBookDetails(playerMagicsId, purpleMagic, purpleGroupMagicId);

        // mid
        magicBookDetails.Add(new List<object> { 
            new MagicOrder(
                "group color",
                "ungroup color", 
                "", 
                null, 
                Color.black, 
                true,
                true) });
        rowIsShow.Add(true);
        midIndexRow = magicBookDetails.Count - 1;
        currentIndexRow = midIndexRow;

        // green
        Magic greenMagic = chromagicDictionary[27];
        SetUpOrderColorToMagicBookDetails(playerMagicsId, greenMagic, greenGroupMagicId);

        // blue
        Magic blueMagic = chromagicDictionary[12];
        SetUpOrderColorToMagicBookDetails(playerMagicsId, blueMagic, blueGroupMagicId);

        // brown
        Magic brownMagic = chromagicDictionary[17];
        SetUpOrderColorToMagicBookDetails(playerMagicsId, brownMagic, brownGroupMagicId);

        // white
        Magic whiteMagic = chromagicDictionary[139];
        SetUpOrderColorToMagicBookDetails(playerMagicsId, whiteMagic, whiteGroupMagicId);

        // gray
        Magic grayMagic = chromagicDictionary[40];
        SetUpOrderColorToMagicBookDetails(playerMagicsId, grayMagic, grayGroupMagicId);

        // Manual book
        manualBookDetails.Add(new List<object> {
            new MagicOrder(
                "",
                "refill",
                "use 10 action point to refill all magics' charge",
                Color.black,
                Color.white,
                false,
                false,
                10) });

        bookDetails = magicBookDetails;

        UpdateUI();
    }

    public Magic FilterMagicChargingByString(string filter)
    {
        Magic magic = playerMagics
            .Where(m => m.ColorName.StartsWith(filter, StringComparison.OrdinalIgnoreCase))
            .OrderBy(m => m.ColorName.Length > filter.Length)
            .FirstOrDefault();

        return magic;
    }

    public void UseChargeByMagic(Magic magic)
    {
        int charge = chargesDictionary[magic.magicID];
        if (charge > 0)
        {
            chargesDictionary[magic.magicID] -= 1;
        }

        UpdateUI();
    }

    public int GetChargeByMagic(Magic magic)
    {
        int charge = chargesDictionary[magic.magicID];

        return charge;
    }

    public void ResetCharages()
    {
        foreach (var key in chargesDictionary.Keys.ToList())
        {
            chargesDictionary[key] = 1;
        }

        UpdateUI();
    }

    private bool IsMagicCanUse(Magic magic, int charge)
    {
        bool isMagicCanuse = true;

        if (magic.Cost > playerAP.GetAP()) { isMagicCanuse = false; }
        if (charge <= 0) { isMagicCanuse = false; }

        return isMagicCanuse;
    }

    public Magic GetMagicWithSelectedPage()
    {
        // selected page
        object selectedMagicBookDetail = bookDetails[currentIndexRow][currentIndexColumn];
        if (selectedMagicBookDetail is Magic selectedMagic)
        {
            return selectedMagic;
        }

        return null;
    }
}
