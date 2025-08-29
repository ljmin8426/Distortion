using UnityEngine;

public class MouseTargeting : MonoBehaviour
{
    [SerializeField] private Camera mainCamera; 
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
                if (lastHighlighted != enemy) 
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
