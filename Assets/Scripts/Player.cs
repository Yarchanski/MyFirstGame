using System.Collections;               // ������������ ��� ��� ������ � �����������.
using System.Collections.Generic;      // ������������ ��� ��� ������ � ����������� �����������.
using TMPro;                           // ������������ ��� ��� ������������� TextMeshPro.
using UnityEditor;                     // ������������ ��� ��� ������������ ��������� Unity (������������ ������ � ���������).
using UnityEngine;                     // ������������ ��� ��� Unity API.
using UnityEngine.UI;                  // ������������ ��� ��� ������ � UI-����������.
using UnityEngine.UIElements;          // ������������ ��� ��� ������ � �������� UIElements (�� ������������ � ���� ����).

public class NewBehaviourScript : MonoBehaviour // ����� ��������� MonoBehaviour, ��� ������ ��� ����������� Unity.
{
    public GameObject Wall;             // ������ ����� ��� ������.
    public GameObject Turret;           // ������ ������ ��� ������.
    public GameObject Freeze;           // ������ ��������������� �������� ��� ������.
    private Coroutine flashCoroutine;   // ������ �� �������� ��� ��������� ������.
    public float speed;                 // �������� �������� ������.
    public float jumpForce;             // ���� ������ ������.
    public Rigidbody2D rb;              // ������ �� ��������� Rigidbody2D ��� ������.
    public Vector2 moveVector;          // ������ ����������� ��������.
    public Animator anim;               // ������ �� ��������� Animator ��� ��������.
    public SpriteRenderer sr;           // ������ �� SpriteRenderer ��� ���������� ��������.
    public bool onGround = false;       // ����, �����������, ��������� �� ����� �� �����.
    public Transform GroundCheck;       // ����� �������� ������� �����.
    public float checkRadius;           // ������ �������� �����.
    public LayerMask Ground;            // ����, ������������ �����.
    private int jumpCount = 0;          // ������� ������� � �������.
    public int maxJumpValue = 2;        // ������������ ���������� �������.
    public int gearCount = 5;           // ���������� ��������� (������� ������).
    public TextMeshProUGUI gearText;    // UI-������� ��� ����������� ���������� ���������.
    private bool isFlashing = false;    // ����, �����������, ������ �� �����.
    public float offsetX = 1f;          // �������� �� X ��� ������ ��������.
    private AudioSource audioSource;    // �������� �����.

    private void Start()                // �����, ���������� ��� ������ �����.
    {
        rb = GetComponent<Rigidbody2D>();    // �������� ��������� Rigidbody2D.
        anim = GetComponent<Animator>();     // �������� ��������� Animator.
        sr = GetComponent<SpriteRenderer>(); // �������� ��������� SpriteRenderer.
        gearText.text = gearCount.ToString(); // ������������� ��������� ����� ��� ����������� ���������.
        sr.sortingOrder = 10;                // ������������� ������� ��������� ��� �������.
        Time.timeScale = 1f;                 // ������������� ���������� �������� ����.
        audioSource = GetComponent<AudioSource>(); // �������� ��������� AudioSource.
        audioSource.Play();                  // ����������� ����.
    }

    private void Update() // �����, ���������� ������ ����.
    {
        walk();                               // ������������ ��������.
        Flip();                               // ������������ ������.
        Jump();                               // ������������ ������.
        CheckingGround();                     // ���������, ��������� �� ����� �� �����.
        gearText.text = gearCount.ToString(); // ��������� ����� ���������� ���������.
        float savedVolume = PlayerPrefs.GetFloat("Volume", 1f); // �������� ���������� ���������.
        AudioListener.volume = savedVolume;  // ��������� ���������.
    }

    void walk() // ����� ��� ��������� ��������.
    {
        moveVector.x = Input.GetAxis("Horizontal"); // �������� �������������� �������� �� �����.
        if (transform.position.x >= 11.25 && moveVector.x > 0) // ������������ �������� ������.
        {
            moveVector.x = 0;
        }
        anim.SetFloat("moveX", Mathf.Abs(moveVector.x)); // ������� �������� �������� ��������.
        rb.velocity = new Vector2(moveVector.x * speed, rb.velocity.y); // ����� �������� Rigidbody.
    }

    void Flip() // ����� ��� ����� ����������� �������.
    {
        if (moveVector.x > 0) sr.flipX = false; // ���� �������� ������, �� ��������������.
        else if (moveVector.x < 0) sr.flipX = true; // ���� �����, ��������������.
    }

    void Jump() // ����� ��� ��������� �������.
    {
        if (Input.GetKeyDown(KeyCode.S)) // ���� ������ ������� S.
        {
            Physics2D.IgnoreLayerCollision(6, 7, true); // ���������� ������������ ����� ������.
            Invoke("IgnoreLayerOff", 0.3f); // ��������� ������������� ����� 0.3 �������.
        }
        if (Input.GetKeyDown(KeyCode.Space)) // ���� ������ ������� ������.
        {
            if (onGround) // ���� �� �����, ��������� ������.
            {
                rb.velocity = new Vector2(rb.velocity.x, 0); // �������� ������������ ��������.
                rb.AddForce(Vector2.up * jumpForce); // ��������� ��������� ����.
            }
            else if (++jumpCount < maxJumpValue) // ���� ����� �������� ��� ���.
            {
                rb.velocity = new Vector2(0, 15); // ��������� ������������ ��������.
            }
        }
        if (onGround) jumpCount = 0; // ���������� ������� ������� ��� ������� �����.
    }

    void CheckingGround() // ���������, ��������� �� ����� �� �����.
    {
        onGround = Physics2D.OverlapCircle(GroundCheck.position, checkRadius, Ground); // ��������� ����������� � �����.
        anim.SetBool("onGround", onGround); // ������� ��������� ����� � ��������.
    }

    void IgnoreLayerOff() // ����� ��� ���������� ������������� ��������.
    {
        Physics2D.IgnoreLayerCollision(6, 7, false);
    }

    void OnTriggerEnter2D(Collider2D collision) // ����� ��� ��������� ���������.
    {
        if (collision.gameObject.CompareTag("Gear")) // ���� ������ � ����� "Gear".
        {
            if (!collision.gameObject.activeInHierarchy) return; // ���� ������ ���������, ������ �� ������.
            gearCount++; // ����������� ������� ���������.
            collision.gameObject.SetActive(false); // ������������ ������.
            Destroy(collision.gameObject); // ���������� ������.
        }
    }

    public void SpawnWall() // ����� ��� ������ �����.
    {
        if (gearCount > 4) // ���� ������� ���������.
        {
            gearCount -= 5; // ��������� ������� ���������.
            Vector3 spawnPosition = transform.position + new Vector3(offsetX, 0, 0); // ���������� �������.
            Instantiate(Wall, spawnPosition, Quaternion.identity); // ������ ������.
        }
        else flashCoroutine = StartCoroutine(FlashGearText()); // ����� ��������� ������� ������.
    }

    public void SpawnTurret() // ����� ��� ������ ������.
    {
        if (gearCount > 4)
        {
            gearCount -= 5;
            Vector3 spawnPosition = transform.position + new Vector3(offsetX, 0, 0);
            Instantiate(Turret, spawnPosition, Quaternion.identity);
        }
        else flashCoroutine = StartCoroutine(FlashGearText());
    }

    public void SpawnFreeze() // ����� ��� ������ �������� ���������.
    {
        if (gearCount > 4)
        {
            gearCount -= 5;
            Vector3 spawnPosition = transform.position + new Vector3(offsetX, 0, 0);
            Instantiate(Freeze, spawnPosition, Quaternion.identity);
        }
        else flashCoroutine = StartCoroutine(FlashGearText());
    }

    IEnumerator FlashGearText() // �������� ��� ������� ������.
    {
        isFlashing = true; // ������������� ���� �������.
        Color originalColor = gearText.color; // ��������� �������� ���� ������.
        Color flashColor = Color.red; // ���� �������.

        for (int i = 0; i < 4; i++) // ������� 4 ����.
        {
            gearText.color = flashColor;
            yield return new WaitForSeconds(0.1f);
            gearText.color = originalColor;
            yield return new WaitForSeconds(0.1f);
        }
        isFlashing = false; // ���������� ����.
    }

    public void PlayAudio() // ����� ��� ��������������� �����.
    {
        if (audioSource != null) audioSource.Play();
    }

    public void PauseAudio() // ����� ��� ����� ��
    {
        if (audioSource != null)
        {
            audioSource.Pause();
        }
    }
}