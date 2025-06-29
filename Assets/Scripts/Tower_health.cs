using System.Collections; // ���������� ������������ ���� ��� ������ � �����������, ��������, IEnumerator.
using System.Collections.Generic; // ���������� ������������ ���� ��� ������ � ������ ���������, ������ ��� List.
using TMPro; // ���������� ������������ ���� TextMeshPro ��� ������ � ���������� ����������.
using UnityEngine; // ���������� ������������ ���� Unity ��� �������� �����������.

public class Tower_health : MonoBehaviour // ��������� �����, ������� ����� ���������� � ������� �����.
{
    public int health = 100; // �������� �����, �� ��������� 100.
    private bool isPlayerInside = false; // ����, �����������, ��������� �� ����� � ���� ��������������.
    public Sprite sprite1; // ������ ��� �������� ���� 66.
    public Sprite sprite2; // ������ ��� �������� �� 33 �� 66.
    public Sprite sprite3; // ������ ��� �������� ���� 33.
    private SpriteRenderer spriteRenderer; // ��������� ��� ��������� ������� �������.
    private bool isUnderAttack = false; // ����, �����������, ��������� �� �����.
    private Coroutine damageCoroutine; // ������ �� ��������, ������� ��������� ��������.
    public Transform player; // ������ �� ������ ������.
    public float interactionDistance = 2f; // ������������ ��������� ��� ��������������.
    private NewBehaviourScript playerScript; // ������ �� ������ ������.
    public TextMeshProUGUI gearText; // UI-������� ��� ����������� ���������� "����������".
    private Coroutine flashCoroutine; // ������ �� �������� ��� ����������� ������� ������.
    private bool isFlashing = false; // ����, �����������, ����������� �� ������� ������.
    public bool isMain; // ����, �����������, �������� �� ��� ����� �������.

    public void Start() // ����� ���������� ��� ������ �������.
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // �������� ��������� SpriteRenderer �������� �������.
        UpdateSprite(); // ��������� ������ � ����������� �� �������� ��������.
        playerScript = player.GetComponent<NewBehaviourScript>(); // �������� ������ ������ ��� ��������������.
    }

    void Update() // ����� ���������� ������ ����.
    {
        // InteractWithPlayer(); // ������������������ ����� ������ (�� ������������).
        UpdateSprite(); // ��������� ������ �����.
        if (health < 1 && !isMain) // ���� �������� ���� 1 � ����� �� �������� �������.
        {
            Destroy(gameObject); // ���������� ������.
        }
        if (isPlayerInside && Input.GetKeyDown(KeyCode.E)) // ���� ����� ������ ���� � ������ ������� E.
        {
            if (playerScript.gearCount >= 10 && health < 100 && isMain) // ���� � ������ ���������� "����������", �������� ������ 100, � ����� �������.
            {
                health += 10; // ����������� �������� �� 10.
                playerScript.gearCount -= 10; // ��������� ���������� "����������" ������.
                UpdateSprite(); // ��������� ������.
            }
            else // ���� ������� �� ���������.
            {
                flashCoroutine = StartCoroutine(FlashGearText()); // ��������� ������� ������.
            }
        }
    }

    void UpdateSprite() // ����� ��� ���������� ������� �����.
    {
        if (health > 66) // ���� �������� ������ 66.
        {
            spriteRenderer.sprite = sprite1; // ������������� ������ ������.
        }
        else if (health > 33) // ���� �������� �� 33 �� 66.
        {
            spriteRenderer.sprite = sprite2; // ������������� ������ ������.
        }
        else // ���� �������� ������ ��� ����� 33.
        {
            spriteRenderer.sprite = sprite3; // ������������� ������ ������.
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) // ����� ����������, ����� ������ ������ � �������.
    {
        if (collision.gameObject.CompareTag("Player")) // ���� ������ - �����.
        {
            isPlayerInside = true; // ������������� ����, ��� ����� ������.
        }
    }

    private void OnTriggerExit2D(Collider2D collision) // ����� ����������, ����� ������ �������� �������.
    {
        if (collision.gameObject.CompareTag("Player")) // ���� ������ - �����.
        {
            isPlayerInside = false; // ���������� ����, ��� ����� ������.
        }
    }

    void OnCollisionExit2D(Collision2D collision) // ����� ����������, ����� ������ ��������� ������������.
    {
        if (collision.gameObject.CompareTag("Enemy")) // ���� ������ - ����.
        {
            isUnderAttack = false; // ���������� ���� �����.
            if (damageCoroutine != null) // ���� �������� ��������.
            {
                StopCoroutine(damageCoroutine); // ������������� ��������.
            }
        }
    }

    IEnumerator DamageOverTime() // �������� ��� ���������� �������� � �������� �������.
    {
        while (isUnderAttack) // ���� ����� ���������.
        {
            health -= 10; // ��������� �������� �� 10.
            UpdateSprite(); // ��������� ������.
            yield return new WaitForSeconds(3f); // ���� 3 �������.
        }
    }

    IEnumerator FlashGearText() // �������� ��� ������� ������.
    {
        isFlashing = true; // ������������� ���� �������.
        Color originalColor = gearText.color; // ��������� ������������ ���� ������.
        Color flashColor = Color.red; // ������������� ���� ������� (�������).

        for (int i = 0; i < 4; i++) // ���� �������.
        {
            gearText.color = flashColor; // ������ ���� �� �������.
            yield return new WaitForSeconds(0.1f); // ���� 0.1 �������.
            gearText.color = originalColor; // ���������� ������������ ����.
            yield return new WaitForSeconds(0.1f); // ���� 0.1 �������.
        }
        isFlashing = false; // ���������� ���� �������.
    }
}
