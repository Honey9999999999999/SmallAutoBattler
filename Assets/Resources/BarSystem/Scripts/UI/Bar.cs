using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    [SerializeField] private Image outFrame;
    [SerializeField] private Image slider;
    [SerializeField] private TextMeshProUGUI textField;

    private float maxSize;
    private float minSize;

    public void Awake()
    {
        maxSize = outFrame.rectTransform.sizeDelta.x - 5;
        minSize = outFrame.rectTransform.sizeDelta.y - 5;
    }

    public void SetRatio(float current, float max)
    {
        float ratio = current / max;
        slider.rectTransform.sizeDelta = new Vector2(Mathf.Lerp(minSize, maxSize, ratio), minSize);
        if(textField != null)
        {
            textField.text = $"{current:F0}/{max:F0}";
        }
    }
}
