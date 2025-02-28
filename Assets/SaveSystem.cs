using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private string savePath;

    private void Awake()
    {
        savePath = Application.persistentDataPath + "/savefile.json";
        Debug.Log("Save Path: " + savePath);
    }

    public void SaveGame(Vector3 playerPosition, List<GameObject> draggableObjects)
    {
        // Создаем данные для сохранения
        GameData data = new GameData
        {
            playerPositionX = playerPosition.x,
            playerPositionY = playerPosition.y,
            playerPositionZ = playerPosition.z
        };

        // Добавляем данные о каждом Draggable объекте
        foreach (var obj in draggableObjects)
        {
            DraggableData draggableData = new DraggableData
            {
                objectName = obj.name,
                positionX = obj.transform.position.x,
                positionY = obj.transform.position.y,
                positionZ = obj.transform.position.z
            };
            data.draggableObjects.Add(draggableData);
        }

        // Преобразуем данные в JSON
        string json = JsonUtility.ToJson(data, true);

        // Записываем JSON в файл
        File.WriteAllText(savePath, json);
        Debug.Log("Game Saved!");
    }

    public GameData LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            GameData data = JsonUtility.FromJson<GameData>(json);
            Debug.Log("Game Loaded!");
            return data;
        }
        else
        {
            Debug.LogWarning("Save file not found!");
            return null;
        }
    }
}


[System.Serializable]
public class GameData
{
    public float playerPositionX;
    public float playerPositionY;
    public float playerPositionZ;

    // Список данных о предметах с тегом Draggable
    public List<DraggableData> draggableObjects = new List<DraggableData>();
}

[System.Serializable]
public class DraggableData
{
    public string objectName; // Уникальное имя объекта
    public float positionX;
    public float positionY;
    public float positionZ;
}