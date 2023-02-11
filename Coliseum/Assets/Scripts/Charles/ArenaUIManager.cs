using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class ArenaUIManager : MonoBehaviour
{
    public static ArenaUIManager Instance;
    public RectTransform upgradeSlot;
    public RectTransform timer;
    public RectTransform title;
    public CanvasGroup titleCanvasGroup;
    public TextMeshProUGUI titleText;
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
        // titleCanvasGroup = title.GetComponent<CanvasGroup>();
        // titleText = title.GetComponent<TextMeshProUGUI>();
    }
    private void Update() {

    }
    public void showRoundTitle(){
        //Sequence sq = DOTween.Sequence();
        // return sq
        // .SetId("showRoundTitle")
        // .Append(
        List<string> titles = new List<string>{"Death Fight", "Survival", "The Boss"};
        titleText.text = titles[ArenaManager.Instance.phase];
        titleCanvasGroup
        .DOFade(1, 1f)
        .SetEase(Ease.InQuad)
        .SetLoops(2, LoopType.Yoyo);
        // );
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
            ArenaManager.Instance.phase = -1;
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

        // battle title
        title.localPosition = Vector3.zero;
        titleText.color = Color.red;
        titleCanvasGroup.alpha = 0f;
    }
}
