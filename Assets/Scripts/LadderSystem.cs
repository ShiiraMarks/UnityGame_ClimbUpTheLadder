using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderSystem : MonoBehaviour
{
    public GameObject playerInventory; // Ссылка на объект инвентаря игрока
    public GameObject ghostLadderPrefab; // Префаб призрака лестницы
    public float pickupDistance = 3f; // Расстояние для подбора лестницы

    private GameObject pickedLadder; // Лестница, которая была взята
    private GameObject ghostLadder; // Призрак лестницы
    private bool hasPickedLadder = false; // Флаг, показывающий, взял ли игрок лестницу

    void Update()
    {
        if (!hasPickedLadder)
        {
            if (Input.GetMouseButtonDown(0)) // ЛКМ
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, pickupDistance))
                {
                    if (hit.transform.CompareTag("Ladder"))
                    {
                        pickedLadder = hit.transform.gameObject;
                        pickedLadder.SetActive(false); // Скрываем лестницу

                        hasPickedLadder = true;
                    }
                }
            }
        }

        if (hasPickedLadder && Input.GetMouseButtonDown(1)) // ПКМ
        {
            ghostLadder = Instantiate(ghostLadderPrefab, playerInventory.transform.position, Quaternion.identity);
        }

        if (ghostLadder != null && Input.GetMouseButtonDown(0)) // ЛКМ
        {
            pickedLadder.SetActive(true); // Показываем лестницу
            pickedLadder.transform.position = ghostLadder.transform.position;

            Destroy(ghostLadder); // Уничтожаем призрак лестницы
            hasPickedLadder = false;
        }
    }
}