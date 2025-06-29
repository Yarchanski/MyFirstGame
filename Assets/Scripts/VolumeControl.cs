using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider; // ������ �� ��������
    [SerializeField] private Text volumeText; // ������ �� ����� (�����������)

    void Start()
    {
        // ��������� ���������� ��������� (�� ��������� 1f)
        float savedVolume = PlayerPrefs.GetFloat("Volume", 1f);
        volumeSlider.value = savedVolume; // ������������� �������� ��������
        AdjustVolume(savedVolume); // ��������� ���������

        // ��������� ��������� ��� ������������ ��������� ��������
        volumeSlider.onValueChanged.AddListener(AdjustVolume);
    }

    public void AdjustVolume(float volume)
    {
        // ������������� ���������� ���������
        AudioListener.volume = volume;

        // ��������� �������� � PlayerPrefs
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();

        // ��������� �����, ���� �� ����
        if (volumeText != null)
        {
            volumeText.text = $"Volume: {(int)(volume * 100)}%";
        }
    }

    private void OnDestroy()
    {
        // ������� ��������� ��� ����������� �������
        volumeSlider.onValueChanged.RemoveListener(AdjustVolume);
    }
}
