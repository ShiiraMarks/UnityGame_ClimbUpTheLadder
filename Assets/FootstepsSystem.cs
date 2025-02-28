using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepsSystem : MonoBehaviour
{
    [Header("�������� ���������")]
    public AudioSource audioSource;
    public float stepInterval = 0.5f; // ������� �������� ����� ������ (��� ������)
    public float moveThreshold = 0.1f; // ����������� �������� ��� ������������ �����
    public float raycastDistance = 2.36f; // ���������� �������� �����
    public bool debugMode = false; // ����� �������
    public LayerMask groundLayer = -1; // ���� ��� �������� ����� (�� ��������� ��� ����)

    [Header("��������� ��������")]
    public float walkSpeed = 2f; // ���������� �������� ������
    public float runSpeed = 6f; // �������� ����
    [Range(0.2f, 0.8f)]
    public float runStepInterval = 0.3f; // �������� ����� ������ ��� ����

    [Header("����� ����� ��� ������ ������������")]
    public AudioClip[] grassSteps;
    public AudioClip[] concreteSteps;
    public AudioClip[] woodSteps;
    public AudioClip[] metalSteps;
    public AudioClip[] defaultSteps;
    public AudioClip[] ladderSteps;
    public AudioClip[] stoneSteps;

    [Header("��������� �����")]
    [Range(0f, 1f)]
    public float footstepsVolume = 1f; // ��������� �����
    [Range(0.5f, 1.5f)]
    public float minPitch = 0.95f; // ����������� ������ ����� ��� ������
    [Range(0.5f, 1.5f)]
    public float maxPitch = 1.05f; // ������������ ������ ����� ��� ������
    [Range(1f, 2f)]
    public float runPitchMultiplier = 1.2f; // ��������� ������ ����� ��� ����

    private float stepTimer;
    private CharacterController characterController;
    private Rigidbody rb;
    private bool isMoving;
    private string currentSurface = "";
    private float currentSpeed;
    private float currentStepInterval;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            Debug.Log("AudioSource ��� ������ �������������");
        }

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f;
        audioSource.volume = footstepsVolume;

        if (grassSteps.Length == 0 && concreteSteps.Length == 0 &&
            woodSteps.Length == 0 && metalSteps.Length == 0)
        {
            Debug.LogWarning("��������: �� ��������� ����� �����!");
        }

        Debug.Log($"FootstepsSystem ���������������. CharacterController: {characterController != null}, Rigidbody: {rb != null}");
    }

    private void Update()
    {
        currentSpeed = 0f;
        if (characterController != null)
        {
            currentSpeed = characterController.velocity.magnitude;
        }
        else if (rb != null)
        {
            currentSpeed = rb.velocity.magnitude;
        }

        isMoving = currentSpeed > moveThreshold;
        bool isGrounded = IsGrounded();

        // ��������� ������� �������� ����� ������ �� ������ ��������
        float speedRatio = Mathf.Clamp01((currentSpeed - walkSpeed) / (runSpeed - walkSpeed));
        currentStepInterval = Mathf.Lerp(stepInterval, runStepInterval, speedRatio);

        if (isMoving && isGrounded)
        {
            stepTimer += Time.deltaTime;

            if (stepTimer >= currentStepInterval)
            {
                PlayFootstepSound(speedRatio);
                stepTimer = 0f;
            }
        }

        if (debugMode)
        {
            Debug.Log($"Speed: {currentSpeed:F2}, SpeedRatio: {speedRatio:F2}, StepInterval: {currentStepInterval:F2}, IsGrounded: {isGrounded}");
        }
    }

    private bool IsGrounded()
    {
        bool result = false;
        RaycastHit hit;

        Vector3 rayStart = transform.position + Vector3.up * 0.5f;

        result = Physics.Raycast(rayStart, Vector3.down, out hit, raycastDistance, groundLayer);

        if (!result)
        {
            Vector3 forward = rayStart + transform.forward * 0.2f;
            result = Physics.Raycast(forward, Vector3.down, out hit, raycastDistance, groundLayer);
        }

        if (!result)
        {
            Vector3 back = rayStart - transform.forward * 0.2f;
            result = Physics.Raycast(back, Vector3.down, out hit, raycastDistance, groundLayer);
        }

        if (!result && characterController != null)
        {
            result = characterController.isGrounded;
        }

        if (debugMode)
        {
            Debug.DrawRay(rayStart, Vector3.down * raycastDistance, result ? Color.green : Color.red);
            Debug.DrawRay(rayStart + transform.forward * 0.2f, Vector3.down * raycastDistance, result ? Color.green : Color.yellow);
            Debug.DrawRay(rayStart - transform.forward * 0.2f, Vector3.down * raycastDistance, result ? Color.green : Color.blue);
        }

        return result;
    }

    private void PlayFootstepSound(float speedRatio)
    {
        if (audioSource == null) return;

        RaycastHit hit;
        Vector3 rayStart = transform.position + Vector3.up * 0.5f;
        if (Physics.Raycast(rayStart, Vector3.down, out hit, raycastDistance, groundLayer))
        {
            string surfaceTag = hit.collider.tag;
            currentSurface = surfaceTag;
            AudioClip[] currentSurfaceClips = null;

            switch (surfaceTag.ToLower())
            {
                case "grass":
                    currentSurfaceClips = grassSteps;
                    break;
                case "deafault":
                    currentSurfaceClips = defaultSteps;
                    break;
                case "concrete":
                    currentSurfaceClips = concreteSteps;
                    break;
                case "wood":
                    currentSurfaceClips = woodSteps;
                    break;
                case "metal":
                    currentSurfaceClips = metalSteps;
                    break;
                case "draggable":
                    currentSurfaceClips = ladderSteps;
                    break;
                case "stone":
                    currentSurfaceClips = stoneSteps;
                    break;
                default:
                    currentSurfaceClips = defaultSteps; // �� ��������� ���������� ���� ������
                    if (debugMode)
                    {
                        Debug.LogWarning($"����������� ��� �����������: {surfaceTag}, ���������� ���� ������");
                    }
                    break;
            }

            if (currentSurfaceClips != null && currentSurfaceClips.Length > 0)
            {
                AudioClip randomClip = currentSurfaceClips[Random.Range(0, currentSurfaceClips.Length)];
                if (randomClip != null)
                {
                    // ����������� ������ ����� � ����������� �� ��������
                    float pitchMultiplier = Mathf.Lerp(1f, runPitchMultiplier, speedRatio);
                    audioSource.pitch = Random.Range(minPitch, maxPitch) * pitchMultiplier;
                    audioSource.PlayOneShot(randomClip, footstepsVolume);

                    if (debugMode)
                    {
                        Debug.Log($"���: �����������={surfaceTag}, ��������={currentSpeed:F2}, ������ �����={audioSource.pitch:F2}");
                    }
                }
            }
        }
    }
}

