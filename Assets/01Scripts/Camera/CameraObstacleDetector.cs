using UnityEngine;

public class CameraObstacleDetector : MonoBehaviour
{
    public Transform target; // 플레이어
    public LayerMask obstacleLayer;

    void LateUpdate()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, target.position);

        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, distance, obstacleLayer);

        foreach (var hit in hits)
        {
            TransparentObject[] transparents = hit.transform.GetComponentsInChildren<TransparentObject>();

            foreach (var obj in transparents)
            {
                obj.BecomeTransparent();
            }
        }
    }
}
