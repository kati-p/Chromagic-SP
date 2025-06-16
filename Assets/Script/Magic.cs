using UnityEngine;

[CreateAssetMenu(fileName = "Magic", menuName = "Scriptable Objects/Magic")]
public class Magic : ScriptableObject
{
    [SerializeField]
    public int magicID;

    [SerializeField]
    private Color color;

    [SerializeField]
    private string colorName;

    [SerializeField]
    private int cost;

    [SerializeField]
    private float attackPower;

    [SerializeField]
    private float luminanceWCAG;

    [SerializeField]
    private Color bestContrast;

    public int MagicID { get { return magicID; } set { magicID = value; } }

    public Color Color { get { return color; } set { color = value; } }

    public string ColorName { get { return colorName; } set {colorName = value; } }

    public int Cost { get { return cost; } set { cost = value; } }

    public float AttackPower { get { return attackPower; } set { attackPower = value; } }

    public float LuminanceWCAG { get { return luminanceWCAG; } set { luminanceWCAG = value; } }

    public Color BestContrast { get { return bestContrast; } set { bestContrast = value; } }

}
