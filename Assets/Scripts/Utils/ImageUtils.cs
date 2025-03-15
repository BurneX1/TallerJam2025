using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ImageUtils : MonoBehaviour
{
    public static void Opacity(Image box, float alpha, float uiReactSpd)
    {
        if (box.color.a != alpha)
        {
            box.color = new Color(box.color.r, box.color.g, box.color.b, Mathf.Lerp(box.color.a, alpha, Time.deltaTime * uiReactSpd));
        }
    }

    public static void BarRefresh(Image box, float act, float max, float uiReactSpd)
    {
        if (box.fillAmount != act / max)
        {
            box.fillAmount = Mathf.Lerp(box.fillAmount, act / max, Time.deltaTime * uiReactSpd);
        }
    }
    public static void BarRefresh(Image box, float act, float max, Text txt, string writeTxt, float uiReactSpd)
    {
        txt.text = writeTxt;
        if (box.fillAmount != act / max)
        {
            box.fillAmount = Mathf.Lerp(box.fillAmount, act / max, Time.deltaTime * uiReactSpd);
        }
    }
}
