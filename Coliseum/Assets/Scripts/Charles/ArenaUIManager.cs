using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class ArenaUIManager : MonoBehaviour
{
    public static ArenaUIManager Instance;
    public RectTransform upgradeSlot;
    private List<RectTransform> upgradeSlotChildren = new List<RectTransform>{};
    [SerializeField] private List<TextMeshProUGUI> threeText = new List<TextMeshProUGUI>{};
    string[] textToShows = {"hp up", "move spd up", "atk spd up", "atk cd down", "atk dmg up"};
    [SerializeField] private List<Image> threeIcon = new List<Image>{};
    public List<Sprite> spriteToShow = new List<Sprite>{};


    public RectTransform timer;
    public RectTransform title;
    public CanvasGroup titleCanvasGroup;
    public TextMeshProUGUI titleText;
    public bool isTimerOn;

    public AudioSource source;

    private void Awake()
    {
        // // start of new code
        // if (Instance != null)
        // {
        //     Destroy(gameObject);
        //     return;
        // }
        // // end of new code

        Instance = this;
        // DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        isTimerOn = false;
        for(int i = 0; i < 3; i++){
            // get three rect transform
            upgradeSlotChildren.Add(upgradeSlot.GetChild(i).GetComponent<RectTransform>());
            // get three image
            threeIcon.Add(upgradeSlotChildren[i].GetChild(0).GetComponent<Image>());
            // get three text
            threeText.Add(upgradeSlotChildren[i].GetChild(1).GetComponent<TextMeshProUGUI>());
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
        for(int i = 0; i < 3; i++){
            threeText[i].text = textToShows[ArenaManager.Instance.curUpgrades[i]];

            threeIcon[i].sprite = spriteToShow[ArenaManager.Instance.curUpgrades[i]];
            threeIcon[i].preserveAspect = true;
            threeIcon[i].SetNativeSize();
            threeIcon[i].rectTransform.sizeDelta = new Vector2(300, 300);
        }
    }
    public Tween showUpgradeSlots(){
        Sequence sq = DOTween.Sequence();
        return sq
        .SetId("showUpgradeSlots")
        .OnStart(()=>{
            upgradeSlot.GetComponent<CanvasGroup>().interactable = false;
            upgradeSlot.localPosition = new Vector3(0, Screen.height + upgradeSlot.sizeDelta.y, 0);
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
            source.Play();
            upgradeSlot.GetComponent<CanvasGroup>().interactable = false;
        })
        .Append(
            upgradeSlot
            .DOAnchorPosY(Screen.height + upgradeSlot.sizeDelta.y, 1f)
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
            timer.localPosition = new Vector3(0, Screen.height + timer.sizeDelta.y, 0);
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
            .DOAnchorPosY(Screen.height + timer.sizeDelta.y, 1f)
            .SetEase(Ease.InBack)
        )
        .OnComplete(()=>{
            timer.GetComponent<TextMeshProUGUI>().color = Color.white;
        });
    }

    public void resetUI(){
        timer.localPosition = new Vector3(0, Screen.height + timer.sizeDelta.y, 0);
        upgradeSlot.localPosition = new Vector3(0, Screen.height + upgradeSlot.sizeDelta.y, 0);

        // battle title
        title.localPosition = Vector3.zero;
        titleText.color = Color.red;
        titleCanvasGroup.alpha = 0f;
    }
}
