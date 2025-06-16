using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;

    public EnemyData EnemyData { get { return enemyData; } set { enemyData = value; } }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetUp();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetUp()
    {
        bool isDefeated = false;

        if (GameData.Instance.EnemiesDefeated.ContainsKey(enemyData.EnemyId))
        {
            isDefeated = GameData.Instance.EnemiesDefeated[enemyData.EnemyId];
        }

        if (isDefeated)
        {
            gameObject.SetActive(false);
        }
    }
}
