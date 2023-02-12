using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class ArenaUIManager : MonoBehaviour
{
    public static ArenaUIManager Instance;
    public RectTransform upgradeSlot;
    private List<RectTransform> upgradeSlotChildren = new List<RectTransform>{};
    [SerializeField] private List<TextMeshProUGUI> threeText = new List<TextMeshProUGUI>{};
    
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
        for(int i = 0; i < 3; i++){
            // get three rect transform
            upgradeSlotChildren.Add(upgradeSlot.GetChild(i).GetComponent<RectTransform>());
            // get three text
            threeText.Add(upgradeSlotChildren[i].GetChild(0).GetComponent<TextMeshProUGUI>());
            // get three image
        }

    }
    private void Update() {

    }
    public void showRoundTitle(){
        //Sequence sq = DOTween.Sequence();
        // return sq
        // .SetId("showRoundTitle")
        // .Append(
        List<string> titles = new List<string>{"Death Fight", "Survival", "The Boss"};
        titleText.text = titles[(int)ArenaManager.Instance.phase];
        titleCanvasGroup
        .DOFade(1, 1f)
        .SetEase(Ease.InQuad)
        .SetLoops(2, LoopType.Yoyo);
        // );
    }

    public void updateUpgradeInfo(){
        string[] textToShows = {"hp up", "dmg up", "atk cd down", "dash spd up", "atk spd up"};
        for(int i = 0; i < 3; i++){
            threeText[i].text = textToShows[ArenaManager.Instance.curUpgrades[i]];
        }
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


    public void hideUpgradeSlots(int i){
        Sequence sq = DOTween.Sequence();
        sq
        .SetId("hideUpgradeSlots")
        .OnStart(()=>{
            //upgrade the stats
            ArenaManager.Instance.adjustSingleStats((upgradeType)ArenaManager.Instance.curUpgrades[i]);
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
            ArenaManager.Instance.phase = phaseType.invalid;
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
            .DOAnchorPosY(430, 1f)
            // .DOAnchorPosY(Screen.height / 2 - timer.sizeDelta.y, 1f)
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
        )
        .OnComplete(()=>{
            timer.GetComponent<TextMeshProUGUI>().color = Color.white;
        });
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
