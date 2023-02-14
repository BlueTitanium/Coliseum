using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemySpawner : MonoBehaviour
{
    public List<Transform> enemyList;
    public List<Transform> obstacleList; // 0: spike 1: barrel
    public Transform landMark;


    Vector3 origScreenBounds;
    Vector3 origScreenOrigo;

    public Collider2D gridCol;
    public Transform water;

    Vector2 fieldBounds;

    // round enemy detail
    int enemyNum;
    int enemyCount;
    int activeEnemyCount;



    private void Awake()
    {

        origScreenBounds = new Vector3(8, 14);
        origScreenOrigo = Vector3.zero;
        fieldBounds = gridCol.bounds.extents;
        // Debug.Log(Camera.main.pixelHeight);
    }

    private void Update()
    {

    }

    public IEnumerator spawnEnemies()
    {
        // initialize
        int[] fib = { 1, 1, 2, 3, 5, 8, 13, 21, 34, 55 };
        int round = ArenaManager.Instance.round;

        enemyNum = fib[round - (round/3)];
        enemyCount = 0;
        activeEnemyCount = 0;
        //int waveEnemyNum = Random.Range(2, 5);
        List<Transform> enemies = new List<Transform> { };

        StartCoroutine(traceDeaths());
        while (enemyCount < enemyNum)
        {
            // wait until less or equal to 3 enemies
            yield return new WaitUntil(() => activeEnemyCount < 3);

            // instantiate ONE enemy at certain angle
            float randomAngle = Random.Range(0f, 2f * Mathf.PI);
            water.gameObject.layer = LayerMask.NameToLayer("Default");
            Transform obj = Instantiate(
                enemyList[(round < 2) ? 0 : Random.Range(0, enemyList.Count)],
                new Vector2(origScreenBounds.x * Mathf.Cos(randomAngle) * 2, origScreenBounds.y * Mathf.Sin(randomAngle) * 2),
                Quaternion.identity
            );
            // increment
            activeEnemyCount++;
            enemyCount++;
            Debug.Log($"active: {activeEnemyCount} total: {enemyCount}");

            StartCoroutine(trackSingleDeath(obj.gameObject));
            if (obj.GetComponent<MeleeEnemy>())
            {
                obj.GetComponent<MeleeEnemy>().disabled = true;
            }

            //
            Vector2 landPos = obj.position * 0.16f + Vector3.up * 0.1f;
            obj
            .DOJump(
                landPos,
                jumpPower: 10f,
                numJumps: 1,
                duration: 0.6f
            )
            .OnComplete(() =>
            {
                water.gameObject.layer = LayerMask.NameToLayer("Water");
                obj
                .DOScaleY(
                    0.7f,
                    0.2f
                )
                .SetLoops(2, LoopType.Yoyo);

                obj
                .DOLocalMoveY(
                    -0.2f,
                    0.2f
                )
                .SetRelative()
                .SetLoops(2, LoopType.Yoyo)
                .OnComplete(() =>
                {
                    // activte attacking && player control
                    // TODO
                    if (obj.GetComponent<MeleeEnemy>())
                    {
                        obj.GetComponent<MeleeEnemy>().disabled = false;
                    }
                });
            });
            // increment
            yield return new WaitForSeconds(3f);


        }
    }


    public IEnumerator spawnObstacle(){
        float a;
        float b; 
        float theta;
        float x; 
        float y; 
        // generate spike
        for(int i = 0; i < ArenaManager.Instance.round; i++){
            a = fieldBounds.y * Mathf.Sqrt(Random.Range(0f, 1f));
            b = fieldBounds.y * Mathf.Sqrt(Random.Range(0f, 1f));
            theta = Random.Range(0f, 1f) * 2 * Mathf.PI;
            x = a * Mathf.Cos(theta);
            y = b * Mathf.Sin(theta);

            StartCoroutine(generateSingleSpike(new Vector2(x, y)));
        }

        yield return new WaitForSeconds(2f);
        while(ArenaUIManager.Instance.isTimerOn){
            Vector2 pos;
            a = fieldBounds.y * Mathf.Sqrt(Random.Range(0f, 1f));
            b = fieldBounds.y * Mathf.Sqrt(Random.Range(0f, 1f));
            theta = Random.Range(0f, 1f) * 2 * Mathf.PI;
            x = a * Mathf.Cos(theta);
            y = b * Mathf.Sin(theta);
            pos = new Vector2(x, y);
            if(Random.Range(0, 5) == 0){
                pos = PlayerController.p.transform.position;
            }
            StartCoroutine(generateSingleBarrel(pos));
            yield return new WaitForSeconds(0.5f - 0.03f * ArenaManager.Instance.round);
          
        }


    }

    IEnumerator generateSingleSpike(Vector3 pos){
        Transform obj = Instantiate(obstacleList[0], pos, Quaternion.identity);
        SpriteRenderer _sr = obj.GetChild(0).GetComponent<SpriteRenderer>();
        _sr.color = new Color(1, 1, 1, 0);


        _sr
        .DOFade(
            1f,
            2f
        );
        obj
        .DOLocalRotate(
            new Vector3(0, 0, 360),
            2f,
            RotateMode.FastBeyond360
        )
        .SetRelative();

        yield return new WaitForSeconds(30f);
        _sr
        .DOFade(
            0f,
            0.3f
        );
        obj.DOLocalMoveY(
            -0.3f,
            0.3f
        )
        .SetEase(
            Ease.InBack
        )
        .OnComplete(()=>{
            Destroy(obj.gameObject);
        });
        

    }

    IEnumerator generateSingleBarrel(Vector3 pos){
        Transform obj = Instantiate(obstacleList[1], pos + origScreenBounds.y * Vector3.up * 2f, Quaternion.identity);
        var mark = Instantiate(landMark, pos, Quaternion.identity);
        var _sr = obj.GetComponent<SpriteRenderer>();
  

        obj
        .DOMove(
            pos,
            1f
        )
        .SetEase(
            Ease.InSine
        )
        .OnKill(()=>{   // hit player
            // add effect
            Destroy(mark.gameObject);
        })
        .OnComplete(()=>{ // hit ground
            //
            if(obj.transform.GetChild(0).GetComponent<ExplodingBomb>() != null)
            {
                obj.transform.GetChild(0).GetComponent<ExplodingBomb>().explodeBomb();
                Destroy(mark.gameObject);
            } else
            {
                obj.GetComponent<Collider2D>().enabled = false;
                Destroy(mark.gameObject);
                _sr.DOFade(
                    0f,
                    0.3f
                )
                .OnComplete(() => {

                    Destroy(obj.gameObject);
                });
            }
        });
        yield return new WaitForSeconds(1f);


    }


    IEnumerator trackSingleDeath(GameObject obj)
    {
        yield return new WaitUntil(() => (obj == null));
        activeEnemyCount--;
    }

    IEnumerator traceDeaths()
    {
        yield return new WaitUntil(() => (enemyCount == enemyNum && activeEnemyCount == 0));
        ArenaManager.Instance.phase = phaseType.upgrade;
    }

}
