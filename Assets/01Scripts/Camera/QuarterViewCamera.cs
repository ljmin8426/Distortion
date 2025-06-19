using UnityEngine;

public class AdvancedCameraFollow : MonoBehaviour
{
    [Header("Camera Setting")]
    [SerializeField] private Transform target;

    [SerializeField] private Vector3 offset = new Vector3(-10, 10f, -10f);
    [SerializeField] private float cameraSmooth = 10f;

    [Header("Zoom Setting")]
    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private float minZoom = 5f;
    [SerializeField] private float maxZoom = 20f;
    [SerializeField] private float currentZoom;

    [Header("Fade Target")]
    [SerializeField] private LayerMask obstacleLayers;

    private Vector3 currentOffset;

    void Start()
    {
        currentZoom = offset.magnitude;
        currentOffset = offset.normalized * currentZoom; // 초기 방향 유지
    }

    void LateUpdate()
    {
        if (target == null) return;

        // 최종 위치 계산
        Vector3 desiredPosition = target.position + currentOffset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * cameraSmooth);

        transform.LookAt(target.position);

        HandleZoom();
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        currentZoom -= scroll * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

        currentOffset = offset.normalized * currentZoom; // 사용자가 설정한 방향으로 줌 거리 조절
    }

#if UNITY_EDITOR
    // 인스펙터 값 변경 시에도 바로 반영되게
    private void OnValidate()
    {
        if (!Application.isPlaying)
            LateUpdate();
    }
#endif
}
