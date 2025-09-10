using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace AutoBattlers
{
    [Serializable]
    public class TimerToBattle : MonoBehaviour
    {
        public event Action OnStartBattle;

        [SerializeField] private int time;
        [SerializeField] private TextMeshProUGUI timerField;
        [SerializeField] private Animator timerAnimator;

        public void StartBattle()
        {
            StartCoroutine(StartBattleAsync());
        }

        private IEnumerator StartBattleAsync()
        {
            while(time > 0)
            {
                if (timerField != null)
                {
                    timerField.text = time--.ToString();
                }
                if (timerAnimator != null)
                {
                    timerAnimator.Play("TimerTB");
                }
                
                yield return new WaitForSeconds(1);
            }

            OnStartBattle?.Invoke();
        }
    }
}
