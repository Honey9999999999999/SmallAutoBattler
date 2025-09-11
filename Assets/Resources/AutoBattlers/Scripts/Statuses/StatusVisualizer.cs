using TMPro;
using UnityEngine;

namespace AutoBattlers
{
    public class StatusVisualizer : MonoBehaviour
    {
        [SerializeField] private AutoBattler battler;
        [SerializeField] private GameObject stunImage;
        [SerializeField] private GameObject fireImage;

        private TextMeshProUGUI stunCounter;
        private TextMeshProUGUI fireCounter;

        public void Start()
        {
            battler.Statuses.OnStatusChanged += (StatusSystem.StatusType status) => CheckStatuses();
            stunCounter = stunImage.GetComponent<TextMeshProUGUI>();
            fireCounter = fireImage.GetComponent<TextMeshProUGUI>();
        }

        private void CheckStatuses()
        {
            stunCounter.text = "x" + (battler.Statuses.Count<StunEffect>().ToString());
            stunImage.SetActive(stunCounter.text != "x0");
            fireCounter.text = "x" + (battler.Statuses.Count<FireStatus>().ToString());
            fireImage.SetActive(fireCounter.text != "x0");
        }
    }
}