using System;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    [SerializeField] private List<DungeonRoom> roomList;

    [Header("BossSetting")]
    [SerializeField] private string bossName;
    [SerializeField] private Transform bossSpawnPoint;

    private int currentRoomIndex = 0;

    public static event Action<BossController> OnAllClear;

    private void Start()
    {
        foreach (DungeonRoom room in roomList)
        {
            room.gameObject.SetActive(false);
            room.OnRoomCleared += HandleRoomCleared;
        }

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
            PoolObject obj = PoolManager.Instance.SpawnFromPool(bossName, bossSpawnPoint.position, bossSpawnPoint.rotation);

            BossController boss = obj.GetComponent<BossController>();

            OnAllClear?.Invoke(boss);
        }
    }
}
