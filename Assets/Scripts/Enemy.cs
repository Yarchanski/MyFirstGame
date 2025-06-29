using System.Collections; // ���������� ������������ ���� ��� ������ � �����������, ��������, ��� IEnumerator.
using System.Collections.Generic; // ���������� ������������ ���� ��� ������ � ����������� ���� List.
using UnityEngine; // ���������� ������������ ���� Unity ��� �������� �����������, ������ ��� Rigidbody2D � Collider2D.

public class Enemy : MonoBehaviour // ��������� ����� ��� �����, ������� ����� ���������� � ������� �����.
{
    public delegate void EnemyDestroyedHandler(); // ������� ��� ��������� ������� ����������� �����.
    public event EnemyDestroyedHandler OnEnemyDestroyed; // �������, ������� ����� ���������� ��� ����������� �����.

    private bool isMain = false; // ����, ������� ���������, �������� �� ���� ��������.
    private bool freezed = false; // ����, �����������, ��������� �� ����.
    public float speed = 5f; // �������� �������� �����.
    public int damageAmount = 10; // ������� ����� ������� ����.
    public float damageInterval = 3f; // �������� ����� ������� �����.
    public bool isJump = true; // ����, ������������, ����� �� ���� �������������� �� ������ ����� ������.
    private bool isDead = false; // ����, �����������, ����� �� ����.

    private Rigidbody2D rb; // ��������� Rigidbody2D ��� �������� �����.
    public GameObject destructionEffectPrefab; // ������ ������� ���������� (�����, ��������).
    private NewBehaviourScript playerScript; // ������ �� ������ ������ ��� ��������������.
    private Coroutine damageCoroutine; // ���������� ��� �������� ������ �� ��������, ������� ������� ����.
    private AudioSource audioSource; // �������� ����� ��� ������������ �������� ��������.

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // �������� ��������� Rigidbody2D.
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; // ������������ ��������, ����� ���� �� �������������.
        GameObject player = GameObject.FindGameObjectWithTag("Player"); // ���� ������ ������.
        playerScript = player.GetComponent<NewBehaviourScript>(); // �������� ������ ������ ��� ��������������.
        audioSource = GetComponent<AudioSource>(); // �������� ��������� AudioSource ��� ��������������� ������.
    }

    void Update()
    {
        rb.velocity = new Vector2(-speed, rb.velocity.y); // ������� ����� �����, �������� ������������ �������� ��� ���������.
    }

    void OnCollisionEnter2D(Collision2D collision) // ����� ���������� ��� ������������ � ������ ��������.
    {
        if (collision.gameObject.CompareTag("Tower")) // ���� ������������ � �������� "Tower".
        {
            Tower_health towerHealth = collision.gameObject.GetComponent<Tower_health>(); // �������� ��������� �������� �����.
            if (towerHealth != null) // ���� ����� ����������.
            {
                damageCoroutine = StartCoroutine(DamageTowerOverTime(towerHealth)); // ��������� �������� ��� ��������� ����� �����.
            }
        }

        if (collision.gameObject.CompareTag("Trap")) // ���� ������������ � �������� "Trap".
        {
            Turret health = collision.gameObject.GetComponent<Turret>(); // �������� ��������� �������� ������.
            if (health != null) // ���� ������ ����������.
            {
                damageCoroutine = StartCoroutine(DamageTurretOverTime(health)); // ��������� �������� ��� ��������� ����� ������.
            }
        }

        if (collision.gameObject.CompareTag("Bullet")) // ���� ������������ � �������.
        {
            OnEnemyDestroyed?.Invoke(); // �������� ������� ����������� �����.
            Destroy(gameObject); // ���������� ������ �����.
            Instantiate(destructionEffectPrefab, transform.position, Quaternion.identity); // ������� ������ ����������.
        }
    }

    void OnCollisionExit2D(Collision2D collision) // ����� ����������, ����� ������ ������� �� ������������.
    {
        if (collision.gameObject.CompareTag("Tower") && damageCoroutine != null) // ���� ���� ������ �� �������� �����.
        {
            StopCoroutine(damageCoroutine); // ������������� �������� ��������� �����.
            damageCoroutine = null; // �������� ������ �� ��������.
        }
    }

    void OnTriggerEnter2D(Collider2D collision) // ����� ���������� ��� ����� � �������.
    {
        if (collision.gameObject.CompareTag("Player")) // ���� ���� ���������� � �������.
        {
            death(); // �������� ����� ������ �����.
            if (isJump) // ���� ���� ����� �������.
            {
                playerScript.rb.velocity = new Vector2(playerScript.rb.velocity.x, 5f); // ������������ ������ �����.
            }
        }

        if (collision.gameObject.CompareTag("Bullet")) // ���� ���� ���������� � �������.
        {
            death(); // �������� ����� ������ �����.
        }
    }

    IEnumerator DamageTowerOverTime(Tower_health tower) // �������� ��� ��������� ����� ����� � �������� �������.
    {
        while (true) // ����������� ����, ���� ���� ������� �����.
        {
            tower.health -= damageAmount; // ��������� �������� �����.
            yield return new WaitForSeconds(damageInterval); // ���� �������� ����� �������.
        }
    }

    IEnumerator DamageTurretOverTime(Turret turret) // �������� ��� ��������� ����� ������ � �������� �������.
    {
        while (true) // ����������� ����, ���� ���� ������� ������.
        {
            turret.health -= damageAmount; // ��������� �������� ������.
            yield return new WaitForSeconds(damageInterval); // ���� �������� ����� �������.
        }
    }

    void death() // �����, ������� ����������, ����� ���� �������.
    {
        if (isDead) return; // ���� ���� ��� �����, ������ �� ������.

        isDead = true; // ������������� ���� ���������.
        OnEnemyDestroyed?.Invoke(); // �������� ������� ����������� �����.

        // ������� ��������� ������ ��� ��������������� �����.
        GameObject soundObject = new GameObject("DeathSound");
        AudioSource tempAudioSource = soundObject.AddComponent<AudioSource>();
        tempAudioSource.clip = audioSource.clip; // ���������� ��� �� �������� ������.
        tempAudioSource.Play(); // ������������� ����.

        // ������� ��������� ������ ����� ���������� �����.
        Destroy(soundObject, tempAudioSource.clip.length);

        // �������� �� ��������� ������� � ������ 40%.
        if (Random.Range(0, 100) < 40)
        {
            Instantiate(destructionEffectPrefab, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);
        }

        Destroy(gameObject); // ���������� ������ �����.
    }

    public void toFreeze() // ����� ��� ������������� �����.
    {
        if (freezed) return; // ���� ���� ��� ���������, ������ �� ������.

        freezed = true; // ������������� ���� ���������.
        speed = 0f; // ������������� �������� �����.

        Animator animator = GetComponent<Animator>(); // �������� ��������� Animator.
        if (animator != null)
        {
            animator.enabled = false; // ��������� ��������.
        }

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>(); // �������� ��������� SpriteRenderer.
        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(0.5f, 0.8f, 1f); // ������ ���� �����, ����� �������� ��������������.
        }
    }
}
