using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDragAndDrop : MonoBehaviour
{
    public GameObject camera;
    public float distance = 15f;
    GameObject ladder;
    bool canPickUp = false;



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) PickUp();
        if (Input.GetKeyDown(KeyCode.Q)) Drop();
    }

    void PickUp()
    {
        RaycastHit hit;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, distance))
        {
            if (hit.transform.tag == "Draggable")
            {
                if (canPickUp) Drop();

                ladder = hit.transform.gameObject;
                ladder.GetComponent<Rigidbody>().isKinematic = true;
                ladder.GetComponent<Collider>().isTrigger = true;
                ladder.transform.parent = transform;
                ladder.transform.localPosition = Vector3.zero;
                ladder.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
                canPickUp = true;
            }
        }


    }

    void Drop()
    {
        ladder.transform.parent = null;
        ladder.GetComponent<Rigidbody>().isKinematic = false;
        ladder.GetComponent<Collider>().isTrigger = false;
        canPickUp = false;
        ladder = null;
    }
}
