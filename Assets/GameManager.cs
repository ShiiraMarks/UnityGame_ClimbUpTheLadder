using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform player; // Перетащи объект игрока в это поле в инспекторе
    private SaveSystem saveSystem;

    private void Awake()
    {
        saveSystem = GetComponent<SaveSystem>();
    }

    private void Start()
    {
        // Загружаем данные при старте игры
        GameData data = saveSystem.LoadGame();
        if (data != null)
        {
            // Восстанавливаем позицию игрока
            Vector3 loadedPosition = new Vector3(data.playerPositionX, data.playerPositionY, data.playerPositionZ);
            player.position = loadedPosition;

            // Восстанавливаем позиции объектов с тегом Draggable
            foreach (var draggableData in data.draggableObjects)
            {
                GameObject obj = GameObject.Find(draggableData.objectName);
                if (obj != null)
                {
                    obj.transform.position = new Vector3(
                        draggableData.positionX,
                        draggableData.positionY,
                        draggableData.positionZ
                    );
                }
                else
                {
                    Debug.LogWarning($"Object {draggableData.objectName} not found!");
                }
            }
        }
    }

    private void OnApplicationQuit()
    {
        // Собираем все объекты с тегом Draggable
        List<GameObject> draggableObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("Draggable"));
        // Сохраняем игру
        saveSystem.SaveGame(player.position, draggableObjects);
    }
}