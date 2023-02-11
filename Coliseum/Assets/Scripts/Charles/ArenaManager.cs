using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : MonoBehaviour
{

    public static ArenaManager Instance;

    // statics
    public int round;
    public int phase;   // 0: normal battle, 1: survival, 2: boss fight  3: after battle(upgrade/lose)
    public int loopPhase; // 0: first battle, 1: second battle, 2: survival, 3: boss
    public float damageMultiplier = 1;
    public float healthMultiplier = 1;
    public float speedMultiplier = 1;
    public float dashCDmultiplier = 1;
    public float invincibilityTimeMultiplier = 1;
    public float enemyDamageMultiplier = 1;
    public float enemyHealthMultiplier = 1;


    // spawner
    public EnemySpawner _es;


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
        loopPhase = 0;
        StartCoroutine(phaseLoop());
    }

    public void adjustStats(){

    }
    public void adjustSingleStats(string ab){
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
        List<int> phaseIndex = new List<int>{0, 0, 1, 2};
        while(true){
            yield return new WaitUntil(()=> (phase != 3));

            // normal battle/ survival/ boss
            phase = phaseIndex[loopPhase];
            Debug.Log($"phase type: {phase}");
            switchPhase(phase);
            ArenaUIManager.Instance.showRoundTitle();
            

            yield return new WaitUntil(()=> Input.GetKeyDown(KeyCode.K) || phase == 3);
            phase = 3;
            // upgrade
            yield return new WaitUntil(()=> (phase == 3));
            switchPhase(3);

            // increment round
            round++;
            loopPhase = round % 4;
        }
    }

    public void switchPhase(int p){
        switch(p){
            case 0:
            // normal battle
            _es.spawnEnemies();
            break;
            case 1:
            // survival
            ArenaUIManager.Instance.showTimer();
            break;
            case 2:
            // boss
            break;
            case 3:
            // upgrade
            ArenaUIManager.Instance.showUpgradeSlots();
            break;
        }
    }
}

