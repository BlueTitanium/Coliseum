using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : MonoBehaviour
{

    public static ArenaManager Instance;

    // statics
    public int round;
    public int phase;   // 0: normal battle, 1: survival,  2: after battle(upgrade/lose)
    public float damageMultiplier = 1;
    public float healthMultiplier = 1;
    public float speedMultiplier = 1;
    public float dashCDmultiplier = 1;
    public float invincibilityTimeMultiplier = 1;


    // playerController
    private PlayerController _pc;
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
        _pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        StartCoroutine(phaseLoop());
    }


    public void adjustStats(string ab){
        switch(ab){
            case "damage":
            //
            break;
            case "health":
            //
            break;
            case "speed":
            //
            break;
            case "dash":
            //
            break;
            case "invincibility":
            //
            break;
        }
    }


    IEnumerator phaseLoop(){
        ArenaUIManager.Instance.hideTimer();
        ArenaUIManager.Instance.hideUpgradeSlots();
        while(true){
            yield return new WaitUntil(()=> Input.GetKeyDown(KeyCode.K));
            // normal battle/ survival
            switchPhase(Random.Range(0, 2));


            yield return new WaitUntil(()=> Input.GetKeyDown(KeyCode.K));
            // upgrade
            switchPhase(2);
        }
    }

    public void switchPhase(int p){
        switch(p){
            case 0:
            ArenaUIManager.Instance.showUpgradeSlots();
            ArenaUIManager.Instance.hideTimer();
            break;
            case 1:
            ArenaUIManager.Instance.hideUpgradeSlots();
            ArenaUIManager.Instance.showTimer();
            break;
            case 2:
            ArenaUIManager.Instance.hideUpgradeSlots();
            ArenaUIManager.Instance.hideTimer();
            break;
        }
    }
}

