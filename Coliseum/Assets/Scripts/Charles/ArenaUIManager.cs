using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ArenaUIManager : MonoBehaviour
{
    public static ArenaUIManager Instance;
    public RectTransform upgradeSlot;
    public RectTransform timer;
    public bool isTimerOn;

    private void Awake()
    {
        // start of new code
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        isTimerOn = false;
    }
    private void Update() {
        
    }
    public Tween showUpgradeSlots(){
        Sequence sq = DOTween.Sequence();
        return sq
        .SetId("showUpgradeSlots")
        .OnStart(()=>{
            upgradeSlot.GetComponent<CanvasGroup>().interactable = false;
            upgradeSlot.localPosition = new Vector3(0, Screen.height / 2 + upgradeSlot.sizeDelta.y, 0);
        })
        .Append(
            upgradeSlot
            .DOAnchorPosY(0f, 1f)
            .SetEase(Ease.OutBack)
        )
        .OnComplete(()=>{
            upgradeSlot.GetComponent<CanvasGroup>().interactable = true;
        });

    }


    public void hideUpgradeSlots(){
        Sequence sq = DOTween.Sequence();
        sq
        .SetId("hideUpgradeSlots")
        .OnStart(()=>{
            upgradeSlot.GetComponent<CanvasGroup>().interactable = false;
        })
        .Append(
            upgradeSlot
            .DOAnchorPosY(Screen.height / 2 + upgradeSlot.sizeDelta.y, 1f)
            .SetEase(Ease.InBack)
        )
        .AppendInterval(1f)
        .OnComplete(()=>{
            // change phase
            ArenaManager.Instance.phase = Random.Range(0, 2);
        });
    }

    public Tween showTimer(){
        Sequence sq = DOTween.Sequence();
        return sq
        .SetId("showTimer")
        .OnStart(()=>{
            timer.localPosition = new Vector3(0, Screen.height / 2 + timer.sizeDelta.y, 0);
        })
        .Append(
            timer
            .DOAnchorPosY(Screen.height / 2 - timer.sizeDelta.y, 1f)
            .SetEase(Ease.OutBack)
        )
        .AppendInterval(0.5f)
        .OnComplete(()=>{
            isTimerOn = true;
            timer.GetComponent<SurvivalTimer>().startTimer();
        });
    }

    public void hideTimer(){
        Sequence sq = DOTween.Sequence();
        sq
        .SetId("hideTimer")
        .Append(
            timer
            .DOAnchorPosY(Screen.height / 2 + timer.sizeDelta.y, 1f)
            .SetEase(Ease.InBack)
        );
    }

    public void resetUI(){
        timer.localPosition = new Vector3(0, Screen.height / 2 + timer.sizeDelta.y, 0);
        upgradeSlot.localPosition = new Vector3(0, Screen.height / 2 + upgradeSlot.sizeDelta.y, 0);
    }
}
