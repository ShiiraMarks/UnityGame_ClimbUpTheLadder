using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactSoundManager : MonoBehaviour
{
    [Header("Impact Settings")]
    [SerializeField] private AudioClip[] impactSounds; // Массив звуков для случайного воспроизведения
    [SerializeField] private float minImpactForce = 0.1f; // Минимальная сила удара для воспроизведения звука
    [SerializeField] private float maxImpactForce = 10f; // Максимальная сила удара для максимальной громкости
    [SerializeField] private float volumeMultiplier = 1f; // Общий множитель громкости
    [Range(0f, 1f)]
    [SerializeField] private float baseVolume = 1f; // Базовая громкость звука

    private AudioSource audioSource;

    void Start()
    {
        // Проверяем, что объект имеет тег Draggable
        if (!gameObject.CompareTag("Draggable"))
        {
            Debug.LogWarning("ImpactSoundManager: объект должен иметь тег Draggable");
        }

        // Добавляем и настраиваем AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f; // Полное 3D звучание
    }

    void OnCollisionEnter(Collision collision)
    {
        if (impactSounds.Length == 0)
            return;

        // Получаем силу удара
        float impactForce = collision.relativeVelocity.magnitude;

        // Проверяем, достаточно ли сильный удар
        if (impactForce < minImpactForce)
            return;

        // Нормализуем силу удара для громкости
        float normalizedForce = Mathf.Clamp01((impactForce - minImpactForce) / (maxImpactForce - minImpactForce));

        // Выбираем случайный звук из массива
        AudioClip randomSound = impactSounds[Random.Range(0, impactSounds.Length)];

        if (randomSound != null)
        {
            // Вычисляем итоговую громкость
            float finalVolume = baseVolume * normalizedForce * volumeMultiplier;

            // Воспроизводим звук
            audioSource.PlayOneShot(randomSound, finalVolume);
        }
    }
}