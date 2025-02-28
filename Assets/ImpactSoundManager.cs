using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactSoundManager : MonoBehaviour
{
    [Header("Impact Settings")]
    [SerializeField] private AudioClip[] impactSounds; // ������ ������ ��� ���������� ���������������
    [SerializeField] private float minImpactForce = 0.1f; // ����������� ���� ����� ��� ��������������� �����
    [SerializeField] private float maxImpactForce = 10f; // ������������ ���� ����� ��� ������������ ���������
    [SerializeField] private float volumeMultiplier = 1f; // ����� ��������� ���������
    [Range(0f, 1f)]
    [SerializeField] private float baseVolume = 1f; // ������� ��������� �����

    private AudioSource audioSource;

    void Start()
    {
        // ���������, ��� ������ ����� ��� Draggable
        if (!gameObject.CompareTag("Draggable"))
        {
            Debug.LogWarning("ImpactSoundManager: ������ ������ ����� ��� Draggable");
        }

        // ��������� � ����������� AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f; // ������ 3D ��������
    }

    void OnCollisionEnter(Collision collision)
    {
        if (impactSounds.Length == 0)
            return;

        // �������� ���� �����
        float impactForce = collision.relativeVelocity.magnitude;

        // ���������, ���������� �� ������� ����
        if (impactForce < minImpactForce)
            return;

        // ����������� ���� ����� ��� ���������
        float normalizedForce = Mathf.Clamp01((impactForce - minImpactForce) / (maxImpactForce - minImpactForce));

        // �������� ��������� ���� �� �������
        AudioClip randomSound = impactSounds[Random.Range(0, impactSounds.Length)];

        if (randomSound != null)
        {
            // ��������� �������� ���������
            float finalVolume = baseVolume * normalizedForce * volumeMultiplier;

            // ������������� ����
            audioSource.PlayOneShot(randomSound, finalVolume);
        }
    }
}