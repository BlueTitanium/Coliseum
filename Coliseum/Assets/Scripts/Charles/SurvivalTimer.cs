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
    bool isOn = false;

    private void Update() {
        if(Input.GetKeyDown(KeyCode.R)){
            startTimer();
        }

        if(isOn){
            timer.text = $"{Mathf.Round(curTime)}:{Mathf.Round(curTime * 100f) % 100}";
        }
    }

    void startTimer(){


        Sequence sq = DOTween.Sequence();
        sq
        .SetId("timer")
        .OnStart(()=>{
            isOn = true;
            curTime = maxTime;
            DOTween
            .To(()=>curTime, x=>curTime = x, 0f, 30f)
            .SetEase(Ease.OutSine);
            

        })
        .AppendInterval(15f)
        .Append(
            timer
            .DOColor(Color.red, 1f)
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
        .OnComplete(()=>{ isOn = false; });
    
    }
}
