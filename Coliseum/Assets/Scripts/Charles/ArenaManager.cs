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
        phase = 0;
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
        ArenaUIManager.Instance.resetUI();
        while(true){
            yield return new WaitUntil(()=> (phase != 2));
            // normal battle/ survival
            switchPhase(phase);

            yield return new WaitUntil(()=> Input.GetKeyDown(KeyCode.K) || phase == 2);
            phase = 2;
            yield return new WaitUntil(()=> (phase == 2));
            // upgrade
            switchPhase(2);

            //
            round++;
        }
    }

    public void switchPhase(int p){
        switch(p){
            case 0:
            // normal battle
            break;
            case 1:
            // survival
            ArenaUIManager.Instance.showTimer();
            break;
            case 2:
            ArenaUIManager.Instance.showUpgradeSlots();
            break;
        }
    }
}

