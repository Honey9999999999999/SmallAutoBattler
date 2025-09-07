using TMPro;
using UnityEngine;

namespace Autobattlers
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
            battler.StatusSystem.OnStatusChanged += (StatusSystem.StatusType status) => CheckStatuses();
            stunCounter = stunImage.GetComponent<TextMeshProUGUI>();
            fireCounter = fireImage.GetComponent<TextMeshProUGUI>();
        }

        private void CheckStatuses()
        {
            stunCounter.text = "x" + (battler.StatusSystem.Count<StunEffect>().ToString());
            stunImage.SetActive(stunCounter.text != "x0");
            fireCounter.text = "x" + (battler.StatusSystem.Count<FireStatus>().ToString());
            fireImage.SetActive(fireCounter.text != "x0");
        }
    }
}