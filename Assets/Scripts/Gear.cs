using System.Collections; // ������������ ���� ��� ������ � ����������.
using UnityEngine; // ������������ ���� Unity ��� �������� �����������.

public class Gear : MonoBehaviour // ����� ��� ���������� ���������� ������� "Gear".
{
    private SpriteRenderer spriteRenderer; // ��������� ���������� ������������ �������.
    private bool isBlinking = false; // ����, �����������, ������� �� �������.
    private AudioSource audioSource; // �������� ����� ��� �������.
    private bool isCollected = true; // ����, ������������, ��� �� ������ ������.

    void Start() // �����, ���������� ��� ������� �������.
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // �������� ��������� SpriteRenderer ��� ���������� ��������.
        audioSource = GetComponent<AudioSource>(); // �������� ��������� AudioSource ��� ��������������� �����.

        // ��������� ��������, ������� ��������� ������ ����� �������� �����.
        StartCoroutine(DestroyAfterDelay());
    }

    IEnumerator DestroyAfterDelay() // �������� ��� ����������� ������� � ���������.
    {
        yield return new WaitForSeconds(5f); // ��� 5 ������ ����� ������� �������.

        StartCoroutine(Blink()); // ��������� �������� �������.

        yield return new WaitForSeconds(5f); // ��� ��� 5 ������, ���� ��� �������.

        isCollected = false; // ����������, ��� ������ �� ��� ������.
        Destroy(gameObject); // ���������� ������.
    }

    IEnumerator Blink() // �������� ��� ������� �������.
    {
        isBlinking = true; // ������������� ����, ��� ������� �������.
        while (isBlinking)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled; // ����������� ��������� �������.
            yield return new WaitForSeconds(0.2f); // ��� 0.2 ������� ����� �������������.
        }
    }

    private void OnDestroy() // �����, ���������� ��� ����������� �������.
    {
        if (isCollected) // ���� ������ ��� ������.
        {
            // ������ ��������� ������ ��� ������������ �����.
            GameObject soundObject = new GameObject("DeathSound");
            AudioSource tempAudioSource = soundObject.AddComponent<AudioSource>();
            tempAudioSource.clip = audioSource.clip; // �������� �������� ������� �� ������������� ���������.
            tempAudioSource.Play(); // ����������� ����.

            // ���������� ��������� ������ ����� ��������� ��������������� �����.
            Destroy(soundObject, audioSource.clip.length);
        }
        isBlinking = false; // ������������� �������.
    }
}
