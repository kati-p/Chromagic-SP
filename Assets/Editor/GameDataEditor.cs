using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameDataEditor
{
    [MenuItem("Assets/GameData/Load All Magics to Player")]
    private static void LoadPlayerMagics()
    {
        GameData.Instance.PlayerData.Magics.Clear();

        List<int> orderedMagicId = new List<int>
        {
            39,
            61,
            62,
            67,
            75,
            28,
            38,
            18,
            8,
            90,
            88,
            59,
            45,
            25,
            51,
            63,
            55,
            47,
            65,
            76,
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
            73,
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
            34,
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
            30,
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
            4,
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
            7,
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
            115,
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

        for (int i = 0; i < orderedMagicId.Count; i++)
        {
            string assetPath = $"Assets/Resources/ScriptTableObjects/Magics/Magic_{orderedMagicId[i]}.asset";

            Magic magic = AssetDatabase.LoadAssetAtPath<Magic>(assetPath);

            if (magic == null)
            {
                Debug.Log($"magic {i + 1} is null");
                continue;
            }

            // Edit here
            GameData.Instance.PlayerData.Magics.Add(magic);
        }
    }

    // For Initialize
    private static Color[] Colors = {
        new Color32(0, 0, 0, 255),
        new Color32(0, 0, 128, 255),
        new Color32(0, 0, 139, 255),
        new Color32(25, 25, 112, 255),
        new Color32(75, 0, 130, 255),
        new Color32(0, 0, 205, 255),
        new Color32(128, 0, 0, 255),
        new Color32(139, 0, 0, 255),
        new Color32(128, 0, 128, 255),
        new Color32(72, 61, 139, 255),
        new Color32(47, 79, 79, 255),
        new Color32(0, 0, 255, 255),
        new Color32(139, 0, 139, 255),
        new Color32(102, 51, 153, 255),
        new Color32(0, 100, 0, 255),
        new Color32(139, 69, 19, 255),
        new Color32(165, 42, 42, 255),
        new Color32(178, 34, 34, 255),
        new Color32(148, 0, 211, 255),
        new Color32(138, 43, 226, 255),
        new Color32(85, 107, 47, 255),
        new Color32(153, 50, 204, 255),
        new Color32(160, 82, 45, 255),
        new Color32(105, 105, 105, 255),
        new Color32(199, 21, 133, 255),
        new Color32(106, 90, 205, 255),
        new Color32(0, 128, 0, 255),
        new Color32(220, 20, 60, 255),
        new Color32(65, 105, 225, 255),
        new Color32(0, 128, 128, 255),
        new Color32(34, 139, 34, 255),
        new Color32(46, 139, 87, 255),
        new Color32(128, 128, 0, 255),
        new Color32(123, 104, 238, 255),
        new Color32(0, 139, 139, 255),
        new Color32(70, 130, 180, 255),
        new Color32(112, 128, 144, 255),
        new Color32(255, 0, 0, 255),
        new Color32(205, 92, 92, 255),
        new Color32(128, 128, 128, 255),
        new Color32(186, 85, 211, 255),
        new Color32(107, 142, 35, 255),
        new Color32(147, 112, 219, 255),
        new Color32(119, 136, 153, 255),
        new Color32(255, 20, 147, 255),
        new Color32(210, 105, 30, 255),
        new Color32(255, 69, 0, 255),
        new Color32(184, 134, 11, 255),
        new Color32(30, 144, 255, 255),
        new Color32(255, 0, 255, 255),
        new Color32(219, 112, 147, 255),
        new Color32(95, 158, 160, 255),
        new Color32(205, 133, 63, 255),
        new Color32(100, 149, 237, 255),
        new Color32(255, 99, 71, 255),
        new Color32(218, 112, 214, 255),
        new Color32(188, 143, 143, 255),
        new Color32(60, 179, 113, 255),
        new Color32(255, 105, 180, 255),
        new Color32(32, 178, 170, 255),
        new Color32(240, 128, 128, 255),
        new Color32(250, 128, 114, 255),
        new Color32(255, 127, 80, 255),
        new Color32(169, 169, 169, 255),
        new Color32(255, 140, 0, 255),
        new Color32(238, 130, 238, 255),
        new Color32(233, 150, 122, 255),
        new Color32(218, 165, 32, 255),
        new Color32(143, 188, 143, 255),
        new Color32(0, 191, 255, 255),
        new Color32(50, 205, 50, 255),
        new Color32(221, 160, 221, 255),
        new Color32(189, 183, 107, 255),
        new Color32(244, 164, 96, 255),
        new Color32(255, 160, 122, 255),
        new Color32(255, 165, 0, 255),
        new Color32(210, 180, 140, 255),
        new Color32(0, 206, 209, 255),
        new Color32(102, 205, 170, 255),
        new Color32(154, 205, 50, 255),
        new Color32(72, 209, 204, 255),
        new Color32(222, 184, 135, 255),
        new Color32(192, 192, 192, 255),
        new Color32(176, 196, 222, 255),
        new Color32(135, 206, 235, 255),
        new Color32(135, 206, 250, 255),
        new Color32(216, 191, 216, 255),
        new Color32(255, 182, 193, 255),
        new Color32(64, 224, 208, 255),
        new Color32(255, 192, 203, 255),
        new Color32(173, 216, 230, 255),
        new Color32(211, 211, 211, 255),
        new Color32(176, 224, 230, 255),
        new Color32(144, 238, 144, 255),
        new Color32(255, 215, 0, 255),
        new Color32(0, 250, 154, 255),
        new Color32(0, 255, 0, 255),
        new Color32(220, 220, 220, 255),
        new Color32(0, 255, 127, 255),
        new Color32(124, 252, 0, 255),
        new Color32(255, 218, 185, 255),
        new Color32(245, 222, 179, 255),
        new Color32(127, 255, 0, 255),
        new Color32(175, 238, 238, 255),
        new Color32(255, 222, 173, 255),
        new Color32(240, 230, 140, 255),
        new Color32(152, 251, 152, 255),
        new Color32(0, 255, 255, 255),
        new Color32(238, 232, 170, 255),
        new Color32(255, 228, 181, 255),
        new Color32(230, 230, 250, 255),
        new Color32(173, 255, 47, 255),
        new Color32(255, 228, 196, 255),
        new Color32(127, 255, 212, 255),
        new Color32(255, 228, 225, 255),
        new Color32(250, 235, 215, 255),
        new Color32(255, 235, 205, 255),
        new Color32(255, 239, 213, 255),
        new Color32(250, 240, 230, 255),
        new Color32(245, 245, 220, 255),
        new Color32(255, 240, 245, 255),
        new Color32(245, 245, 245, 255),
        new Color32(253, 245, 230, 255),
        new Color32(255, 245, 238, 255),
        new Color32(255, 255, 0, 255),
        new Color32(240, 248, 255, 255),
        new Color32(250, 250, 210, 255),
        new Color32(255, 248, 220, 255),
        new Color32(255, 250, 205, 255),
        new Color32(248, 248, 255, 255),
        new Color32(224, 255, 255, 255),
        new Color32(255, 250, 240, 255),
        new Color32(240, 255, 240, 255),
        new Color32(255, 250, 250, 255),
        new Color32(240, 255, 255, 255),
        new Color32(245, 255, 250, 255),
        new Color32(255, 255, 224, 255),
        new Color32(255, 255, 240, 255),
        new Color32(255, 255, 255, 255),
    };
    private static string[] ColorNames = {
        "black",
        "navy",
        "darkblue",
        "midnightblue",
        "indigo",
        "mediumblue",
        "maroon",
        "darkred",
        "purple",
        "darkslateblue",
        "darkslategray",
        "blue",
        "darkmagenta",
        "rebeccapurple",
        "darkgreen",
        "saddlebrown",
        "brown",
        "firebrick",
        "darkviolet",
        "blueviolet",
        "darkolivegreen",
        "darkorchid",
        "sienna",
        "dimgray",
        "mediumvioletred",
        "slateblue",
        "green",
        "crimson",
        "royalblue",
        "teal",
        "forestgreen",
        "seagreen",
        "olive",
        "mediumslateblue",
        "darkcyan",
        "steelblue",
        "slategray",
        "red",
        "indianred",
        "gray",
        "mediumorchid",
        "olivedrab",
        "mediumpurple",
        "lightslategray",
        "deeppink",
        "chocolate",
        "orangered",
        "darkgoldenrod",
        "dodgerblue",
        "fuchsia / magenta",
        "palevioletred",
        "cadetblue",
        "peru",
        "cornflowerblue",
        "tomato",
        "orchid",
        "rosybrown",
        "mediumseagreen",
        "hotpink",
        "lightseagreen",
        "lightcoral",
        "salmon",
        "coral",
        "darkgray",
        "darkorange",
        "violet",
        "darksalmon",
        "goldenrod",
        "darkseagreen",
        "deepskyblue",
        "limegreen",
        "plum",
        "darkkhaki",
        "sandybrown",
        "lightsalmon",
        "orange",
        "tan",
        "darkturquoise",
        "mediumaquamarine",
        "yellowgreen",
        "mediumturquoise",
        "burlywood",
        "silver",
        "lightsteelblue",
        "skyblue",
        "lightskyblue",
        "thistle",
        "lightpink",
        "turquoise",
        "pink",
        "lightblue",
        "lightgray",
        "powderblue",
        "lightgreen",
        "gold",
        "mediumspringgreen",
        "lime",
        "gainsboro",
        "springgreen",
        "lawngreen",
        "peachpuff",
        "wheat",
        "chartreuse",
        "paleturquoise",
        "navajowhite",
        "khaki",
        "palegreen",
        "aqua / cyan",
        "palegoldenrod",
        "moccasin",
        "lavender",
        "greenyellow",
        "bisque",
        "aquamarine",
        "mistyrose",
        "antiquewhite",
        "blanchedalmond",
        "papayawhip",
        "linen",
        "beige",
        "lavenderblush",
        "whitesmoke",
        "oldlace",
        "seashell",
        "yellow",
        "aliceblue",
        "lightgoldenrodyellow",
        "cornsilk",
        "lemonchiffon",
        "ghostwhite",
        "lightcyan",
        "floralwhite",
        "honeydew",
        "snow",
        "azure",
        "mintcream",
        "lightyellow",
        "ivory",
        "white",
    };
    private static string savePath = "Assets/Game/Resources/Magics";

    [MenuItem("Assets/Magic/Create All ScriptTableObject Magics")]
    private static void GenerateMagic()
    {
        for (int i = 0; i < Colors.Length; i++)
        {
            Magic newMagic = ScriptableObject.CreateInstance<Magic>();

            Color32 color = Colors[i];
            string colorName = ColorNames[i];
            int id = i + 1;

            newMagic.MagicID = id;
            newMagic.Color = color;
            newMagic.ColorName = colorName;
            newMagic.Cost = 3;
            newMagic.AttackPower = 1;
            newMagic.LuminanceWCAG = Chromagic.CalculateLuminace(color.r, color.g, color.b);

            string assetPath = $"{savePath}/Magic_{id}.asset";
            Debug.Log(assetPath);
            AssetDatabase.CreateAsset(newMagic, assetPath);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    [MenuItem("Assets/Magic/Add ScriptTableObject Magics Best Contrast")]
    private static void AddMagicBestContrast()
    {
        for (int i = 0; i < Colors.Length; i++)
        {
            string assetPath = $"{savePath}/Magic_{i + 1}.asset";

            Magic magic = AssetDatabase.LoadAssetAtPath<Magic>(assetPath);

            if (magic == null)
            {
                Debug.Log($"magic {i + 1} is null");
                continue;
            }

            // Edit here
            float crBlack = Chromagic.CalculateContrastRatio(Colors[0], magic.Color);
            float crWhite = Chromagic.CalculateContrastRatio(Colors[Colors.Length - 1], magic.Color);
            if (crBlack <= crWhite)
            {
                magic.BestContrast = Colors[Colors.Length - 1];
            }
            else
            {
                magic.BestContrast = Colors[0];
            }

            EditorUtility.SetDirty(magic);
        }
    }

    [MenuItem("Assets/Magic/Add ScriptTableObject Magics Balance")]
    private static void AddMagicBalance()
    {
        for (int i = 0; i < Colors.Length; i++)
        {
            string assetPath = $"{savePath}/Magic_{i + 1}.asset";

            Magic magic = AssetDatabase.LoadAssetAtPath<Magic>(assetPath);

            if (magic == null)
            {
                Debug.Log($"magic {i + 1} is null");
                continue;
            }

            // Edit here
            float luminance = magic.LuminanceWCAG;

            Debug.Log(luminance);

            if (luminance <= 0.044775)
            {
                magic.Cost = 6;
                magic.AttackPower = 10;
            }
            else if (luminance <= 0.08955)
            {
                magic.Cost = 5;
                magic.AttackPower = 5;
            }
            else if (luminance <= 0.134325)
            {
                magic.Cost = 4;
                magic.AttackPower = 3;
            }
            else if (luminance <= 0.1791)
            {
                magic.Cost = 3;
                magic.AttackPower = 1;
            }
            else if (luminance <= 0.384325)
            {
                magic.Cost = 3;
                magic.AttackPower = 1;
            }
            else if (luminance <= 0.58955)
            {
                magic.Cost = 4;
                magic.AttackPower = 3;
            }
            else if (luminance <= 0.794775)
            {
                magic.Cost = 5;
                magic.AttackPower = 5;
            }
            else if (luminance <= 1.1f)
            {
                magic.Cost = 6;
                magic.AttackPower = 10;
            }
            else
            {
                throw new Exception("Somethings wrong. luminace > 1");
            }

            EditorUtility.SetDirty(magic);
        }
    }

    [MenuItem("Assets/Magic/Load All Magic")]
    private static void LoadAllMagics()
    {
        Chromagic.Instance.Magics.Clear();

        for (int i = 0; i < Colors.Length; i++)
        {
            string assetPath = $"{savePath}/Magic_{i + 1}.asset";

            Magic magic = AssetDatabase.LoadAssetAtPath<Magic>(assetPath);

            if (magic == null)
            {
                Debug.Log($"magic {i + 1} is null");
                continue;
            }

            // Edit here
            Chromagic.Instance.Magics.Add(magic);
        }

        EditorUtility.SetDirty(Chromagic.Instance);
    }
}
