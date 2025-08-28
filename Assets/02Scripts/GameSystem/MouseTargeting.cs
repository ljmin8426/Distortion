using UnityEngine;

public class MouseTargeting : MonoBehaviour
{
    [SerializeField] private Camera mainCamera; // 할당 안하면 자동으로 Camera.main 사용
    private EnemyOutlineHighlighter lastHighlighted;

    void Update()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            EnemyOutlineHighlighter enemy = hit.collider.GetComponent<EnemyOutlineHighlighter>();

            if (enemy != null)
            {
                if (lastHighlighted != enemy) // 새로운 적에 닿았을 때만
                {
                    if (lastHighlighted != null)
                        lastHighlighted.SetHighlight(false);

                    enemy.SetHighlight(true);
                    lastHighlighted = enemy;
                }
            }
            else
            {
                ClearHighlight();
            }
        }
        else
        {
            ClearHighlight();
        }
    }

    private void ClearHighlight()
    {
        if (lastHighlighted != null)
        {
            lastHighlighted.SetHighlight(false);
            lastHighlighted = null;
        }
    }
}
