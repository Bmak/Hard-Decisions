using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class TextCountAnim
{
    public enum Direction
    {
        Up,
        Down
    }

    private static float Y_OFFSET = 50.0f;
    private static float Y_MOVE = 100.0f;

    private static Canvas _canvas;

    public static Text CreateTextAnim(Text text, int from, int to, Action callBack, Direction direction = Direction.Up, bool opposite = false)
    {
        if (_canvas == null)
        {
            _canvas = GameObject.FindObjectOfType<Canvas>();
        }

        Text result = Object.Instantiate(text, _canvas.transform);
        RectTransform rect = result.GetComponent<RectTransform>();
        var originalRect = text.GetComponent<RectTransform>();
        result.transform.localScale = Vector3.one * 2;
        result.transform.position = text.transform.position;
        rect.sizeDelta = new Vector2(originalRect.rect.width, originalRect.rect.height);
        Vector3 pos = result.transform.localPosition;
        pos.y = pos.y + (direction == Direction.Down ? -Y_OFFSET : Y_OFFSET);
        result.transform.localPosition = pos;
        result.text = string.Format("{0}{1}", from < to ? "+" : "", to - from);
        result.color = from < to ? opposite ? Color.green : Color.red : opposite ? Color.red : Color.green;
        result.transform.DOLocalMove(GetMovePos(result.transform.localPosition, direction), 0.5f).OnComplete(() =>
        {
            Object.Destroy(result.gameObject);
            callBack();
        });

        return result;
    }

    private static Vector2 GetMovePos(Vector3 position, Direction direction)
    {
        position.y += direction == Direction.Down ? -Y_MOVE : Y_MOVE;
        return position;
    }

    public static void CreateStatAnim(List<ProfStatView> before, Action callBack)
    {
        if (_canvas == null)
        {
            _canvas = GameObject.FindObjectOfType<Canvas>();
        }

        GameObject go = new GameObject();
        go.name = "StatAnim";
        go.AddComponent<RectTransform>();
        go.transform.SetParent(_canvas.transform);
        go.transform.localScale = Vector3.one;

        foreach (ProfStatView stat in before)
        {
            ProfStatView v = Object.Instantiate(stat, go.transform);
            RectTransform rect = v.GetComponent<RectTransform>();
            var originalRect = stat.GetComponent<RectTransform>();
            v.transform.localScale = Vector3.one * 2;
            v.transform.position = stat.transform.position;
            rect.sizeDelta = new Vector2(originalRect.rect.width, originalRect.rect.height);
        }

        go.transform.DOLocalMove(GetMovePos(go.transform.localPosition, Direction.Up), 0.3f).OnComplete(() =>
        {
            Object.Destroy(go.gameObject);
            callBack();
        });
    }
}
