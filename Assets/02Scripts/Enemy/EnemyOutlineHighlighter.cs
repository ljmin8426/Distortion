using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Outline))]
public class EnemyOutlineHighlighter : MonoBehaviour
{
    private Outline outline;

    void Awake()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false; // ±âº»Àº ²¨µÒ
    }

    public void SetHighlight(bool value)
    {
        outline.OutlineColor = Color.red; // ¸¶¿ì½º ¿À¹ö½Ã »¡°£»ö
        outline.enabled = value;
    }
}
