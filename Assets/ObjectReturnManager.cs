using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectReturnManager : MonoBehaviour
{
    [SerializeField] private float distanceFromCamera = 2f; // ���������� �� ������ ��� ������������ �������
    [SerializeField] private float activationDistance = 2f; // ����������, �� ������� �������� LadderReturner

    [Header("Effects")]
    [SerializeField] private ParticleSystem smokeEffectPrefab; // ������ ������� ����
    [SerializeField] private AudioClip teleportSound; // ���� ������������
    [SerializeField] private float smokeEffectDuration = 2f; // ������������ ������� ����

    [Header("Optional")]
    [SerializeField] private AudioClip tooFarSound; // ����, ����� ����� ������� ������ (�������������)

    private Camera mainCamera;
    private GameObject draggableObject;
    private AudioSource audioSource;
    private Transform playerTransform; // ��������� ������

    void Start()
    {
        mainCamera = Camera.main;
        // ������� ������ � ����� Draggable
        draggableObject = GameObject.FindGameObjectWithTag("Draggable");

        // �������� ��������� ������ (��������������, ��� ������ - �������� ������ ������)
        playerTransform = mainCamera.transform.parent;
        if (playerTransform == null)
        {
            // ���� ������ �� �������� �������� ��������, ���������� � ���������
            playerTransform = mainCamera.transform;
        }

        // ��������� � ����������� AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ��� ������� ����� ������ ����
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("LadderReturner"))
                {
                    // ��������� ���������� ����� ������� � LadderReturner
                    float distanceToReturner = Vector3.Distance(playerTransform.position, hit.collider.transform.position);

                    if (distanceToReturner <= activationDistance)
                    {
                        TeleportObject();
                    }
                    else
                    {
                        // ���� ����� ������� ������, ����� ��������� ���� ��� �������� ���������
                        if (tooFarSound != null && audioSource != null)
                        {
                            audioSource.PlayOneShot(tooFarSound);
                        }
                        Debug.Log("��������� ����� � ������� ��� ���������");
                    }
                }
            }
        }
    }

    void TeleportObject()
    {
        if (draggableObject != null)
        {
            // ������� ������ ���� �� ������ �������
            if (smokeEffectPrefab != null)
            {
                ParticleSystem oldPosSmoke = Instantiate(smokeEffectPrefab, draggableObject.transform.position, Quaternion.identity);
                Destroy(oldPosSmoke.gameObject, smokeEffectDuration);
            }

            // ��������� ���������� ������ ����� �������
            Vector3 targetPosition = mainCamera.transform.position + mainCamera.transform.forward * distanceFromCamera;
            draggableObject.transform.position = targetPosition;

            // ������� ������ ���� �� ����� �������
            if (smokeEffectPrefab != null)
            {
                ParticleSystem newPosSmoke = Instantiate(smokeEffectPrefab, targetPosition, Quaternion.identity);
                Destroy(newPosSmoke.gameObject, smokeEffectDuration);
            }

            // ����������� ���� ������������
            if (teleportSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(teleportSound);
            }
        }
    }
}