using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Lose_menu : MonoBehaviour
{
    public Tower_health tower1;
    public Tower_health tower2;
    public Tower_health tower3;
    public GameObject lose;
    public TextMeshProUGUI timer;
    public TextMeshProUGUI result;
    private AudioSource audioSource;
    public Pause_menu pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        lose.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (tower1.health <= 0 || tower2.health <= 0 || tower3.health <= 0)
        {
            if (!lose.activeSelf) // ����������� ������ ��� ������ ������������
            {
                lose.SetActive(true);

                // ������������� ��� ��������� �����
                foreach (var source in FindObjectsOfType<AudioSource>())
                {
                    if (source != audioSource) // �� ������������� ������� ��������
                    {
                        source.Pause();
                    }
                }

                if (!audioSource.isPlaying) // ���������, ��� ���� ��� �� ������
                {
                    audioSource.Play();
                }
                if (pauseMenu != null) // ��������� Pause_menu
                {
                    pauseMenu.enabled = false;
                }
                Time.timeScale = 0f;
            }
        }

        //result.text = $"Score: {timer.text}";
    }
}
