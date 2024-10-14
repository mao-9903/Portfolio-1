using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SetAnchorPresets
{
    internal static void SetAnchorToCenter(GameObject obj){
        RectTransform rectTransform = obj.GetComponent<RectTransform>();
        Vector2 pos = rectTransform.position;
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.position = pos;
    }
}
