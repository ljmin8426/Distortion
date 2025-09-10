using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MyOutline))]
public class EnemyOutlineHighlighter : MonoBehaviour
{
    private MyOutline outline;

    void Awake()
    {
        outline = GetComponent<MyOutline>();
        outline.enabled = false;
    }

    public void SetHighlight(bool value)
    {
        outline.OutlineColor = Color.red;
        outline.enabled = value;
    }
}
