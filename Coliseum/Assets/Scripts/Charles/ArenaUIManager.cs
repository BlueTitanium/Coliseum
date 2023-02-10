using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ArenaUIManager : MonoBehaviour
{
    public static ArenaUIManager Instance;
    public RectTransform upgradeSlot;
    public RectTransform timer;

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
    }

    public void showUpgradeSlots(){
        Sequence sq = DOTween.Sequence();
        sq
        .SetId("showUpgradeSlots")
        .OnStart(()=>{
            upgradeSlot.GetComponent<CanvasGroup>().interactable = false;

        })
        .Append(
            upgradeSlot
            .DOAnchorPosY(Screen.height + upgradeSlot.sizeDelta.y, 0.1f)
        )
        .Append(
            upgradeSlot
            .DOAnchorPosY(0, 1f)
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
            .DOAnchorPosY(Screen.height + upgradeSlot.sizeDelta.y, 1f)
            .SetEase(Ease.InBack)
        );
    }

    public void showTimer(){
        Sequence sq = DOTween.Sequence();
        sq
        .SetId("showTimer")
        .Append(
            timer
            .DOAnchorPosY(Screen.height + timer.sizeDelta.y, 0.1f)
        )
        .Append(
            timer
            .DOAnchorPosY(Screen.height - timer.sizeDelta.y, 1f)
            .SetEase(Ease.OutBack)
        );
    }

    public void hideTimer(){
        Sequence sq = DOTween.Sequence();
        sq
        .SetId("hideTimer")
        .Append(
            upgradeSlot
            .DOAnchorPosY(Screen.height + upgradeSlot.sizeDelta.y, 1f)
            .SetEase(Ease.InBack)
        );
    }

}
