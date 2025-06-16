using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed;
    float speedX, speedY;
    Rigidbody2D rb;
    bool canMove = true;

    Animator animator;
    Animator shadowAnimator;

    public GameObject transtionBattle;

    [SerializeField]
    private AudioClip encounterSFX;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        animator = transform.Find("Image").GetComponent<Animator>();
        shadowAnimator = transform.Find("Shadow").GetComponent<Animator>();

        Vector2 playerPosition = GameData.Instance.PlayerPosition;

        if (playerPosition != Vector2.zero )
        {
            rb.position = playerPosition;
        }
        
    }

    // Update is called once per frame
    void Update()
    {

        // movement
        if (canMove)
        {
            speedX = Input.GetAxis("Horizontal") * movementSpeed;
            speedY = Input.GetAxis("Vertical") * movementSpeed;
            rb.linearVelocity = new Vector2(speedX, speedY);

            Vector2 movement = new Vector2(speedX, speedY);
            animator.SetFloat("speed", movement.sqrMagnitude);
            shadowAnimator.SetFloat("speed", movement.sqrMagnitude);
        }

    }

    private IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyController enemy = collision.GetComponent<EnemyController>();

            if (enemy != null)
            {
                GameData.Instance.EnemyEncouter = enemy.EnemyData;
                GameData.Instance.EnemyEncouterPosition = enemy.gameObject.transform.position;

                canMove = false;
                rb.linearVelocity = new Vector2(0, 0);

                SoundManager.Instance.StopMusic();
                SoundManager.Instance.PlaySoundFXClip(encounterSFX, 1f, transform);

                OverWorldSystem.isInTransition = true;
                transtionBattle.SetActive(true);
                yield return new WaitForSeconds(transtionBattle.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);

                SceneManager.LoadScene("Battle");
            }
            else
            {
                Debug.Log("enemy's script is null");
            }
        }
    }
}
