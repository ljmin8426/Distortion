using UnityEngine;

public class DungeonDoor : MonoBehaviour
{
    public delegate void DoorOpened();
    public static event DoorOpened OnDoorOpened;

    [SerializeField] private GameObject doorObject;

    private bool isStart = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isStart)
        {
            if (other.CompareTag("Player"))
            {
                if (doorObject != null)
                {
                    doorObject.SetActive(false);
                    OnDoorOpened?.Invoke();
                }

                isStart = true;
            }
        }
    }
}
