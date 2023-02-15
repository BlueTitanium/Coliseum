using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class SurvivalTimer : MonoBehaviour
{
    [SerializeField] private float maxTime = 15f;
    [SerializeField] private float curTime = 15f;
    public TextMeshProUGUI timer;

    private void Update() {
        if(ArenaUIManager.Instance.isTimerOn){
            timer.text = $"{Mathf.Floor(curTime):00}:{Mathf.Floor(curTime * 100f) % 100:00}";
        }
    }

    public void startTimer(){

        timer.text = $"{maxTime}:00";
        Sequence sq = DOTween.Sequence();
        sq
        .SetId("timer")
        .OnStart(()=>{
            // initialize data
            ArenaUIManager.Instance.isTimerOn = true;
            timer.color = Color.white;
            timer.text = $"{maxTime}:00";
            curTime = maxTime;
            // start timer
            DOTween
            .To(()=>curTime, x=>curTime = x, 0f, 15f)
            .SetEase(Ease.Linear);
            

        })
        .AppendInterval(10f)
        .Append(
            timer
            .DOColor(Color.red, 3f)
            .SetEase(Ease.InCubic)
        )
        .Join(
            timer.rectTransform
            .DOShakeAnchorPos(
                duration: 5f,
                strength: 10f,
                vibrato: 30
            )
            .SetEase(Ease.Linear)
        )
        .AppendInterval(1.5f)
        .OnComplete(()=>{
            if (gameObject != null)
            {
                ArenaUIManager.Instance.isTimerOn = false;
                ArenaUIManager.Instance.hideTimer();
                // change phase
                ArenaManager.Instance.phase = phaseType.upgrade;
            }
        });
    
    }
}
