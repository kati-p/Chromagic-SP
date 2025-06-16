using System;
using System.Collections.Generic;
using UnityEngine;

public class Chromagic : MonoBehaviour
{
    private static Chromagic instance;

    public static Chromagic Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<Chromagic>();

                if (instance == null)
                {
                    GameObject gameObject = new GameObject();
                    instance = gameObject.AddComponent<Chromagic>();
                    gameObject.name = "Chromagic";
                    DontDestroyOnLoad(gameObject);
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        foreach (Magic magic in magics)
        {
            magicsDictionaryById.Add(magic.magicID, magic);
        }
    }


    [SerializeField]
    private List<Magic> magics = new List<Magic>();

    private Dictionary<int, Magic> magicsDictionaryById = new Dictionary<int, Magic>();

    public List<Magic> Magics { get { return magics; } }

    public Dictionary<int, Magic> MagicsDictionaryById { get { return magicsDictionaryById; } }

    public static float CalculateLuminace(int r, int g, int b)
    {
        float[] rgb = { r, g, b };

        // Map function equivalent: Transform RGB values
        float[] a = Array.ConvertAll(rgb, v =>
        {
            float normalized = v / 255f;
            return normalized <= 0.03928f
                ? normalized / 12.92f
                : (float)Math.Pow((normalized + 0.055f) / 1.055f, 2.4f);
        });

        // Calculate luminance
        return a[0] * 0.2126f + a[1] * 0.7152f + a[2] * 0.0722f;
    }

    public static float CalculateContrastRatio(Color color1, Color color2)
    {
        Color32 convertedColor1 = color1;
        Color32 convertedColor2 = color2;

        float color1Luminance = CalculateLuminace(convertedColor1.r, convertedColor1.g, convertedColor1.b);
        float color2Luminance = CalculateLuminace(convertedColor2.r, convertedColor2.g, convertedColor2.b);

        float ratio = color1Luminance > color2Luminance
            ? (color1Luminance + 0.05f) / (color2Luminance + 0.05f)
            : (color2Luminance + 0.05f) / (color1Luminance + 0.05f);

        return ratio;
    }

    public static float CalculateContrastRatio(Magic magic1, Magic magic2)
    {
        float color1Luminance = magic1.LuminanceWCAG;
        float color2Luminance = magic2.LuminanceWCAG;

        float ratio = color1Luminance > color2Luminance
            ? (color1Luminance + 0.05f) / (color2Luminance + 0.05f)
            : (color2Luminance + 0.05f) / (color1Luminance + 0.05f);

        return ratio;
    }

    public static Color CalculateColorMixing(Color color1, Color color2)
    {
        float mixedR = (color1.r + color2.r) / 2f;
        float mixedG = (color1.g + color2.g) / 2f;
        float mixedB = (color1.b + color2.b) / 2f;

        return new Color(mixedR, mixedG, mixedB, 1f);
    }

    public static Color NaiveNearestNeighborColors(Color color)
    {
        Color colorNearest = new Color();

        return colorNearest;
    }

    public static Color[] RandomColor()
    {
        List<Magic> magics = Instance.Magics;
        int amount = magics.Count;
        Color[] rndColor = new Color[amount];
        int[] numbers = new int[amount];

        for (int i = 0; i < numbers.Length; i++)
        {
            numbers[i] = i;
        }

        // Fisher-Yates Shuffle
        for (int i = numbers.Length - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);

            int temp = numbers[i];
            numbers[i] = numbers[j];
            numbers[j] = temp;
        }

        for (int i = 0; i < amount; i++)
        {
            rndColor[i] = magics[numbers[i]].Color;
        }

        return rndColor;
    }

    public static Color GetBestContrast(Color color)
    {
        float CRBlack = CalculateContrastRatio(color, Color.black);
        float CRWhite = CalculateContrastRatio(color, Color.white);

        if (CRBlack > CRWhite)
        {
            return Color.black;
        }
        else
        {
            return Color.white;
        }
    }
}
