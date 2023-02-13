using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum phaseType{
    normal,
    survival,
    boss,
    upgrade,
    invalid
}
public enum upgradeType{
    hpUp,
    dashSpdUp,
    atkSpdUp,
    atkCDDown,
    DMGUp
    
}


public class ArenaManager : MonoBehaviour
{

    public static ArenaManager Instance;

    // statics
    public int round;
    public phaseType phase;   // 0: normal battle, 1: survival, 2: boss fight  3: after battle(upgrade/lose)

    public int loopPhase; // 0: first battle, 1: second battle, 2: survival, 3: boss

    // upgrade

    public List<int> curUpgrades = new List<int>{-1, -1, -1};
    // stats
    public float damageMultiplier = 1;
    public float healthMultiplier = 1;
    public float dashSpeedMultiplier = 1;
    public float attackCDMultiplier = 1;
    public float attackSpeedMultiplier = 1;

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

    public void adjustSingleStats(upgradeType i){
        switch(i){
            case upgradeType.hpUp:
                Debug.Log("hp up");
                healthMultiplier += 0.2f;
                break;
            case upgradeType.dashSpdUp:
                Debug.Log("dash spd up");
                dashSpeedMultiplier += 0.2f;
                break;
            case upgradeType.atkSpdUp:
                Debug.Log("atk spd up");
                attackSpeedMultiplier += 0.2f;
                break;
            case upgradeType.atkCDDown:
                Debug.Log("atk cd down");
                attackCDMultiplier -= 0.1f;
                break;
            case upgradeType.DMGUp:
                Debug.Log("dmgup");
                damageMultiplier += 0.3f;
                Debug.Log(damageMultiplier);
                break;
        }
        PlayerController.p.SetStats();
    }


    IEnumerator phaseLoop(){
        ArenaUIManager.Instance.resetUI();
        List<int> phaseIndex = new List<int>{0, 0, 1};
        while(true){
            yield return new WaitUntil(()=> (phase != phaseType.upgrade));

            // normal battle/ survival/ boss
            phase = (phaseType)phaseIndex[loopPhase];
            Debug.Log($"phase type: {phase}");
            switchPhase((int)phase);
            ArenaUIManager.Instance.showRoundTitle();
            

            yield return new WaitUntil(()=> Input.GetKeyDown(KeyCode.K) || phase == phaseType.upgrade);
            phase = phaseType.upgrade;
            // upgrade
            yield return new WaitUntil(()=> (phase == phaseType.upgrade));
            switchPhase(3);

            // increment round
            round++;
            loopPhase = round % (phaseIndex.Count);
        }
    }

    public void switchPhase(int p){
        switch(p){
            case 0:
            // normal battle
            StartCoroutine(_es.spawnEnemies());
            break;
            case 1:
            // survival
            ArenaUIManager.Instance.showTimer();
            StartCoroutine(_es.spawnObstacle());
            break;
            case 2:
            // boss
            break;
            case 3:
            // upgrade
            rollUpgrade();
            ArenaUIManager.Instance.updateUpgradeInfo();
            ArenaUIManager.Instance.showUpgradeSlots();
            break;
        }
    }


    public void rollUpgrade(){
        curUpgrades.Clear();
        int count = 0;
        while(count < 3){
            int temp = Random.Range(0, 5);
            if(!curUpgrades.Contains(temp)){
                curUpgrades.Add(temp);
                count++;
            }
        }
        Debug.Log(curUpgrades);
    }
}

