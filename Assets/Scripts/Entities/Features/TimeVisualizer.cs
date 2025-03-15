using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TimeVisualizer : MonoBehaviour
{
    public TimeAction timer;
    public float speedRefresh = 1;
    public bool reverseFill;
    public Image targetImage;
    // Start is called before the first frame update
    void Start()
    {
        if (timer == null && gameObject.GetComponent<TimeAction>() != null) timer = gameObject.GetComponent<TimeAction>();
    }

    // Update is called once per frame
    void Update()
    {
        RefresImage();
    }

    public void RefresImage()
    {
        if (targetImage == null || timer == null) return;
        float value = timer.timer;
        if (reverseFill) value = timer.defaultTime - timer.timer;
        
        ImageUtils.BarRefresh(targetImage, value, timer.defaultTime, speedRefresh);
    }
}
