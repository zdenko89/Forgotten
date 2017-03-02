using UnityEngine;
using System.Collections;

public class DamageTextControl : MonoBehaviour {

    private static DamageText DamageText;
    private static GameObject canvas;

    public static void Initialize()
    {
        canvas = GameObject.Find("Canvas");
        if (!DamageText)
        DamageText = Resources.Load<DamageText>("Prefabs/DamageTextControl");
    }

    public static void CreateDamageText(string text, Transform location)
    {
        DamageText instance = Instantiate(DamageText);
        Vector2 screenPos = Camera.main.WorldToScreenPoint(new Vector2(location.position.x + Random.Range(-5, 9), location.position.y));
        // Vector2 screenPosition = Camera.main.ScreenToWorldPoint(location.position);
        instance.transform.SetParent(canvas.transform, false);
        instance.transform.position = location.position;

        instance.SetText(text);

    }
}
