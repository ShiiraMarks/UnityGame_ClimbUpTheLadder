using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectReturnManager : MonoBehaviour
{
    [SerializeField] private float distanceFromCamera = 2f; // Расстояние от камеры для телепортации объекта
    [SerializeField] private float activationDistance = 2f; // Расстояние, на котором работает LadderReturner

    [Header("Effects")]
    [SerializeField] private ParticleSystem smokeEffectPrefab; // Префаб эффекта дыма
    [SerializeField] private AudioClip teleportSound; // Звук телепортации
    [SerializeField] private float smokeEffectDuration = 2f; // Длительность эффекта дыма

    [Header("Optional")]
    [SerializeField] private AudioClip tooFarSound; // Звук, когда игрок слишком далеко (необязательно)

    private Camera mainCamera;
    private GameObject draggableObject;
    private AudioSource audioSource;
    private Transform playerTransform; // Трансформ игрока

    void Start()
    {
        mainCamera = Camera.main;
        // Находим объект с тегом Draggable
        draggableObject = GameObject.FindGameObjectWithTag("Draggable");

        // Получаем трансформ игрока (предполагается, что камера - дочерний объект игрока)
        playerTransform = mainCamera.transform.parent;
        if (playerTransform == null)
        {
            // Если камера не является дочерним объектом, используем её трансформ
            playerTransform = mainCamera.transform;
        }

        // Добавляем и настраиваем AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // При нажатии левой кнопки мыши
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("LadderReturner"))
                {
                    // Проверяем расстояние между игроком и LadderReturner
                    float distanceToReturner = Vector3.Distance(playerTransform.position, hit.collider.transform.position);

                    if (distanceToReturner <= activationDistance)
                    {
                        TeleportObject();
                    }
                    else
                    {
                        // Если игрок слишком далеко, можно проиграть звук или показать сообщение
                        if (tooFarSound != null && audioSource != null)
                        {
                            audioSource.PlayOneShot(tooFarSound);
                        }
                        Debug.Log("Подойдите ближе к объекту для активации");
                    }
                }
            }
        }
    }

    void TeleportObject()
    {
        if (draggableObject != null)
        {
            // Создаем эффект дыма на старой позиции
            if (smokeEffectPrefab != null)
            {
                ParticleSystem oldPosSmoke = Instantiate(smokeEffectPrefab, draggableObject.transform.position, Quaternion.identity);
                Destroy(oldPosSmoke.gameObject, smokeEffectDuration);
            }

            // Мгновенно перемещаем объект перед камерой
            Vector3 targetPosition = mainCamera.transform.position + mainCamera.transform.forward * distanceFromCamera;
            draggableObject.transform.position = targetPosition;

            // Создаем эффект дыма на новой позиции
            if (smokeEffectPrefab != null)
            {
                ParticleSystem newPosSmoke = Instantiate(smokeEffectPrefab, targetPosition, Quaternion.identity);
                Destroy(newPosSmoke.gameObject, smokeEffectDuration);
            }

            // Проигрываем звук телепортации
            if (teleportSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(teleportSound);
            }
        }
    }
}