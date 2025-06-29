using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider; // Ссылка на ползунок
    [SerializeField] private Text volumeText; // Ссылка на текст (опционально)

    void Start()
    {
        // Загружаем сохранённую громкость (по умолчанию 1f)
        float savedVolume = PlayerPrefs.GetFloat("Volume", 1f);
        volumeSlider.value = savedVolume; // Устанавливаем значение ползунка
        AdjustVolume(savedVolume); // Применяем громкость

        // Добавляем слушатель для отслеживания изменений ползунка
        volumeSlider.onValueChanged.AddListener(AdjustVolume);
    }

    public void AdjustVolume(float volume)
    {
        // Устанавливаем глобальную громкость
        AudioListener.volume = volume;

        // Сохраняем значение в PlayerPrefs
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();

        // Обновляем текст, если он есть
        if (volumeText != null)
        {
            volumeText.text = $"Volume: {(int)(volume * 100)}%";
        }
    }

    private void OnDestroy()
    {
        // Удаляем слушатель при уничтожении объекта
        volumeSlider.onValueChanged.RemoveListener(AdjustVolume);
    }
}
