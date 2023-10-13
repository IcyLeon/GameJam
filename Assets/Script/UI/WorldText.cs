using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WorldText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI WorldTextREF;
    [SerializeField] CanvasGroup canvasGroup;
    private float DestroyTimer;
    private Vector3 Offset;

    private void Start()
    {
        transform.position += Offset;
    }

    public void Init(string text, float timer, float delay, Color32 color, Vector3 offset)
    {
        SetText(text);
        SetColor(color);
        SetDestroyTimer(timer);
        SetOffset(offset);
        StartCoroutine(FadeOut(delay));
    }

    public void SetColor(Color32 color)
    {
        WorldTextREF.color = color;
    }

    public void SetOffset(Vector3 offset)
    {
        Offset = offset;
    }

    public void SetDestroyTimer(float timer)
    {
        DestroyTimer = timer;
    }

    public void SetText(string text)
    {
        WorldTextREF.text = text;
    }

    public void MovingText(Vector3 velocity)
    {
        StartCoroutine(Moving(velocity));
    }

    private IEnumerator Moving(Vector3 velocity)
    {
        while (true)
        {
            transform.position += velocity * Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator FadeOut(float delay)
    {
        canvasGroup.alpha = 1;
        yield return new WaitForSeconds(delay);
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime / DestroyTimer;
            yield return null;
        }
        Destroy(gameObject);
    }
}
