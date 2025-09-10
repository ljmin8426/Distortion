using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    [SerializeField] private List<DungeonRoom> roomList;
    private int currentRoomIndex = 0;

    private void Start()
    {
        foreach (DungeonRoom room in roomList)
        {
            room.gameObject.SetActive(false);
            room.OnRoomCleared += HandleRoomCleared;
        }

        // 첫 방 활성화 + 몬스터 스폰
        if (roomList.Count > 0)
        {
            roomList[0].gameObject.SetActive(true);
            roomList[0].ActivateRoom();
        }
    }

    private void HandleRoomCleared(DungeonRoom clearedRoom)
    {
        Debug.Log($"{clearedRoom.name} Cleared!");

        currentRoomIndex++;
        if (currentRoomIndex < roomList.Count)
        {
            roomList[currentRoomIndex].gameObject.SetActive(true);
            roomList[currentRoomIndex].ActivateRoom();
        }
        else
        {
            Debug.Log("던전 클리어!");
        }
    }
}
