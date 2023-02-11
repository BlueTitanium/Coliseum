using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class SurvivalTimer : MonoBehaviour
{
    [SerializeField] private float maxTime = 30;
    [SerializeField] private float curTime;
    public TextMeshProUGUI timer;

    private void Update() {

        if(ArenaUIManager.Instance.isTimerOn){
            timer.text = $"{Mathf.Floor(curTime):00}:{Mathf.Floor(curTime * 100f) % 100:00}";
        }
    }

    public void startTimer(){


        Sequence sq = DOTween.Sequence();
        sq
        .SetId("timer")
        .OnStart(()=>{
            // initialize data
            ArenaUIManager.Instance.isTimerOn = true;
            timer.color = Color.white;
            timer.text = "30:00";
            curTime = maxTime;
            // start timer
            DOTween
            .To(()=>curTime, x=>curTime = x, 0f, 30f)
            .SetEase(Ease.OutSine);
            

        })
        .AppendInterval(13f)
        .Append(
            timer
            .DOColor(Color.red, 3f)
            .SetEase(Ease.InCubic)
        )
        .Join(
            timer.rectTransform
            .DOShakeAnchorPos(
                duration: 16f,
                strength: 10f,
                vibrato: 30
            )
            .SetEase(Ease.InOutCubic)
        )
        .AppendInterval(1.5f)
        .OnComplete(()=>{
            ArenaUIManager.Instance.isTimerOn = false;
            ArenaUIManager.Instance.hideTimer();
            // change phase
            ArenaManager.Instance.phase = 2;
        });
    
    }
}
