using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectPickupSystem : MonoBehaviour
{
    public Transform player;
    public Transform holdPoint;
    public float holdDistance = 2.0f;
    public LayerMask interactableLayer;
    public Image statusIcon;
    public Sprite objectHeldIcon;
    public Sprite noObjectIcon;
    public AudioClip pickupSound;
    public AudioClip dropSound;
    public float interactionDistance = 3.0f;
    public float objectPullbackStrength = 10f;
    public float collisionForceLimit = 50f;

    private GameObject heldObject;
    private Rigidbody heldObjectRb;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        UpdateStatusIcon();
    }

    void Update()
    {
        HandlePickup();
        HandleDrop();
        HandleHeldObjectPosition();
    }

    void HandlePickup()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Pickup key
        {
            if (heldObject == null)
            {
                Ray ray = new Ray(player.position, player.forward);
                if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance, interactableLayer))
                {
                    PickupObject(hit.collider.gameObject);
                }
            }
        }
    }

    void HandleDrop()
    {
        if (Input.GetKeyDown(KeyCode.R) && heldObject != null) // Drop key
        {
            DropObject();
        }
    }

    void PickupObject(GameObject obj)
    {
        heldObject = obj;
        heldObjectRb = obj.GetComponent<Rigidbody>();
        if (heldObjectRb != null)
        {
            heldObjectRb.isKinematic = false;
            heldObjectRb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            heldObject.transform.position = holdPoint.position;
            heldObject.transform.SetParent(holdPoint);
        }

        PlaySound(pickupSound);
        UpdateStatusIcon();
    }

    void DropObject()
    {
        if (heldObjectRb != null)
        {
            heldObjectRb.isKinematic = false;
            heldObjectRb.collisionDetectionMode = CollisionDetectionMode.Discrete;
            heldObject.transform.SetParent(null);
        }

        heldObject = null;
        heldObjectRb = null;

        PlaySound(dropSound);
        UpdateStatusIcon();
    }

    void HandleHeldObjectPosition()
    {
        if (heldObject != null && heldObjectRb != null)
        {
            Vector3 desiredPosition = holdPoint.position;
            Vector3 currentPosition = heldObject.transform.position;
            Vector3 offset = desiredPosition - currentPosition;

            if (offset.magnitude > holdDistance)
            {
                heldObjectRb.velocity = Vector3.zero;
                heldObjectRb.angularVelocity = Vector3.zero;
                heldObjectRb.MovePosition(currentPosition + offset.normalized * objectPullbackStrength * Time.deltaTime);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (heldObject != null && collision.gameObject != player.gameObject && collision.relativeVelocity.magnitude > collisionForceLimit)
        {
            // Prevent collision forces from affecting the player
            heldObjectRb.velocity = Vector3.zero;
        }
    }

    void UpdateStatusIcon()
    {
        if (statusIcon != null)
        {
            statusIcon.sprite = heldObject == null ? noObjectIcon : objectHeldIcon;
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
