using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeControl : MonoBehaviour
{
    private Rigidbody rb;

    public float maxSlopeAngle = 45f; // Максимальный угол наклона
    public float maxVerticalSpeed = 5f; // Максимальная вертикальная скорость

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        CheckSlope();
        LimitVerticalSpeed();
    }

    private void CheckSlope()
    {
        Vector3 origin = transform.position;
        Vector3 direction = Vector3.down;
        float distance = 1.1f; // Дистанция для Raycast

        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance))
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            if (slopeAngle > maxSlopeAngle)
            {
                // Если угол наклона слишком большой, отменяем движение
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            }
        }
    }

    private void LimitVerticalSpeed()
    {
        if (rb.velocity.y > maxVerticalSpeed)
        {
            rb.velocity = new Vector3(rb.velocity.x, maxVerticalSpeed, rb.velocity.z);
        }
    }
}