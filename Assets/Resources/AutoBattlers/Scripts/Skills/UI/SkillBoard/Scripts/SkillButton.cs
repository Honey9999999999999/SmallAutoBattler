using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Image), typeof(Button))]
public class SkillButton : MonoBehaviour
{
    public float RequeredMP
    {
        get => requeredMP;
        set
        {
            requeredMP = value;
            SetText($"{requeredMP}MP");
        }
    }
    private float requeredMP;
    private Image image;
    private Button button;

    private void SetText(string value)
    {
        if (textMesh != null)
        {
            textMesh.text = value;
        }
    }

    [SerializeField] private TextMeshProUGUI textMesh;

    public void Awake()
    {        
        image = GetComponent<Image>();
        button = GetComponent<Button>();

        image.type = Image.Type.Filled;
    }

    public void SetAction(UnityAction action)
    {
        button.onClick.AddListener(action);
    }

    public void SetReloadState(float currentTime, float maxTime)
    {
        image.fillAmount = currentTime / maxTime;
        SetText($"{currentTime:F1}");
    }

    public void ClearReloadState()
    {
        image.fillAmount = 1;
        SetText($"{requeredMP}MP");
    }
}
