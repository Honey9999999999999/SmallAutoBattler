using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace AutoBattlers
{
    [Serializable]
    public class TimerToBattle : MonoBehaviour
    {
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                return;
            }

            Destroy(gameObject);
        }

        public static event Action OnStartBattle;

        [SerializeField] private int time;
        [SerializeField] private TextMeshProUGUI timerField;
        [SerializeField] private Animator timerAnimator;

        private static TimerToBattle instance;

        public static void StartBattle()
        {
            instance.StartCoroutine(instance.StartBattleAsync());
        }

        private IEnumerator StartBattleAsync()
        {
            while (time > 0)
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
            Destroy(gameObject);
        }
    }
}
