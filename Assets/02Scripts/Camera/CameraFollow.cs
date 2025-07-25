using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Camera Setting")]
    [SerializeField] private Transform target;

    [SerializeField] private Vector3 offset = new Vector3(-10, 10f, -10f);
    [SerializeField] private float cameraSmooth = 10f;

    [Header("Zoom Setting")]
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float minZoom = 5f;
    [SerializeField] private float maxZoom = 15f;
    [SerializeField] private float currentZoom;

    private Vector3 currentOffset;

    void Start()
    {
        currentZoom = offset.magnitude;
        currentOffset = offset.normalized * currentZoom;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // 최종 위치 계산
        Vector3 desiredPosition = target.position + currentOffset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * cameraSmooth);


        HandleZoom();
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        currentZoom -= scroll * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

        currentOffset = offset.normalized * currentZoom;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!Application.isPlaying)
            LateUpdate();
    }
#endif
}
