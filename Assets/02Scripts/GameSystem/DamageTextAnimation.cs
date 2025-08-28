using TMPro;
using UnityEngine;

public class DamageTextAnimation : PoolObject
{
    [SerializeField] private AnimationCurve opacityCurve;
    [SerializeField] private AnimationCurve scaleCurve;
    [SerializeField] private AnimationCurve heightCurve;

    private TextMeshProUGUI tmp;
    private float time;
    private Vector3 origin;

    private void Awake()
    {
        tmp = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        tmp.color = new Color(1, 1, 1, opacityCurve.Evaluate(time));
        transform.localScale = Vector3.one * scaleCurve.Evaluate(time);
        transform.position = origin + new Vector3(0, 1 + heightCurve.Evaluate(time), 0);

        time += Time.deltaTime;

        // 애니메이션 끝나면 풀로 반환
        if (time >= 1f)
        {
            ReturnToPool();
        }
    }

    public void SetText(string text, Color color, Vector3 position)
    {
        tmp.text = text;
        tmp.faceColor = color;
        origin = position;
    }

    public override void OnSpawn()
    {
        time = 0f;
    }

    public override void OnDespawn()
    {
    }
}
