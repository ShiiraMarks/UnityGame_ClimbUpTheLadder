using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform player; // �������� ������ ������ � ��� ���� � ����������
    private SaveSystem saveSystem;

    private void Awake()
    {
        saveSystem = GetComponent<SaveSystem>();
    }

    private void Start()
    {
        // ��������� ������ ��� ������ ����
        GameData data = saveSystem.LoadGame();
        if (data != null)
        {
            // ��������������� ������� ������
            Vector3 loadedPosition = new Vector3(data.playerPositionX, data.playerPositionY, data.playerPositionZ);
            player.position = loadedPosition;

            // ��������������� ������� �������� � ����� Draggable
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
        // �������� ��� ������� � ����� Draggable
        List<GameObject> draggableObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("Draggable"));
        // ��������� ����
        saveSystem.SaveGame(player.position, draggableObjects);
    }
}