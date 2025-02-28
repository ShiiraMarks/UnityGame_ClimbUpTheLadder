using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // ��� ������ � UI

public class ObjectDragAndDrop : MonoBehaviour
{
    public GameObject camera;
    public float distance = 15f;
    public AudioClip pickUpSound; // ���� �������� �������
    public AudioClip dropSound; // ���� ������������ �������
    public Image icon; // ������ �� UI-������
    public Sprite holdingIcon; // ������ ��� ��������� "������ ������"
    public Sprite defaultIcon; // ������ ��� ��������� "��� �������"

    private GameObject ladder;
    private bool canPickUp = false;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        UpdateIcon();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) TryPickUp();
        if (Input.GetKeyDown(KeyCode.Q)) Drop();
    }

    void TryPickUp()
    {
        if (canPickUp)
        {
            // ���� ������ ��� ������, ������ �� ������
            return;
        }

        PickUp();
    }

    void PickUp()
    {
        RaycastHit hit;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, distance))
        {
            if (hit.transform.tag == "Draggable")
            {
                ladder = hit.transform.gameObject;
                ladder.GetComponent<Rigidbody>().isKinematic = true;
                ladder.GetComponent<Collider>().isTrigger = true;
                ladder.transform.parent = transform;
                ladder.transform.localPosition = Vector3.zero;
                ladder.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
                canPickUp = true;

                // ������������ ����� ��������
                PlaySound(pickUpSound);

                // ���������� ������
                UpdateIcon();
            }
        }
    }

    void Drop()
    {
        if (ladder == null) return;

        ladder.transform.parent = null;
        ladder.GetComponent<Rigidbody>().isKinematic = false;
        ladder.GetComponent<Collider>().isTrigger = false;
        canPickUp = false;

        // ������������ ����� ������������
        PlaySound(dropSound);

        // ����� ������ �� ������
        ladder = null;

        // ���������� ������
        UpdateIcon();
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    void UpdateIcon()
    {
        if (icon != null)
        {
            icon.sprite = canPickUp ? holdingIcon : defaultIcon;
        }
    }
}
