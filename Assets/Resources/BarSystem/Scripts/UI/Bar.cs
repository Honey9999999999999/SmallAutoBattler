using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    [SerializeField] private Image outFrame;
    [SerializeField] private Image slider;

    private float maxSize;
    private float minSize;

    public void Awake()
    {
        maxSize = outFrame.rectTransform.sizeDelta.x - 5;
        minSize = outFrame.rectTransform.sizeDelta.y - 5;
    }

    public void SetRatio(float ratio)
    {
        ratio = Mathf.Clamp01(ratio);

        slider.rectTransform.sizeDelta = new Vector2(Mathf.Lerp(minSize, maxSize, ratio), minSize);
    }
}
