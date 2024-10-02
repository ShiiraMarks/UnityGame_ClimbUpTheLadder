using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderSystem : MonoBehaviour
{
    public GameObject playerInventory; // ������ �� ������ ��������� ������
    public GameObject ghostLadderPrefab; // ������ �������� ��������
    public float pickupDistance = 3f; // ���������� ��� ������� ��������

    private GameObject pickedLadder; // ��������, ������� ���� �����
    private GameObject ghostLadder; // ������� ��������
    private bool hasPickedLadder = false; // ����, ������������, ���� �� ����� ��������

    void Update()
    {
        if (!hasPickedLadder)
        {
            if (Input.GetMouseButtonDown(0)) // ���
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, pickupDistance))
                {
                    if (hit.transform.CompareTag("Ladder"))
                    {
                        pickedLadder = hit.transform.gameObject;
                        pickedLadder.SetActive(false); // �������� ��������

                        hasPickedLadder = true;
                    }
                }
            }
        }

        if (hasPickedLadder && Input.GetMouseButtonDown(1)) // ���
        {
            ghostLadder = Instantiate(ghostLadderPrefab, playerInventory.transform.position, Quaternion.identity);
        }

        if (ghostLadder != null && Input.GetMouseButtonDown(0)) // ���
        {
            pickedLadder.SetActive(true); // ���������� ��������
            pickedLadder.transform.position = ghostLadder.transform.position;

            Destroy(ghostLadder); // ���������� ������� ��������
            hasPickedLadder = false;
        }
    }
}