using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemySpawner : MonoBehaviour
{
    public List<Transform> enemyList;
    // private Collider2D _col;
    // [SerializeField] private float areaWidth;
    // [SerializeField] private float areaHeight;
    // [SerializeField] private float screenWidth;
    // [SerializeField] private float screenHeight;
    Vector3 screenBounds;
    Vector3 screenOrigo;

    Vector3 origScreenBounds;
    Vector3 origScreenOrigo;

    

    private void Awake() {
        // _col = GetComponent<Collider2D>();
        // areaWidth = _col.bounds.extents.x;
        // areaHeight = _col.bounds.extents.y;
        // screenWidth = Screen.width;
        // screenHeight = Screen.height;
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        screenOrigo = Camera.main.ScreenToWorldPoint(Vector2.zero);
        origScreenBounds = screenBounds;
        origScreenOrigo = screenOrigo;        
        // Debug.Log(Camera.main.pixelHeight);
    }

    private void Update() {

    }

    public void spawnEnemies(){
        // initialize
        int round = ArenaManager.Instance.round;
        int bias = (Random.Range(0, 2) == 0)?
            (0):
            (round % 2);
        int enemyNum = round + bias + 1;
        int waveEnemyNum = Random.Range(2, 5);
        List<Transform> enemies = new List<Transform>{};


        // instantiate object
        for(int i = 0; i < enemyNum; i++){
            float randomAngle = Random.Range(0f, 2f * Mathf.PI);
            Transform obj = Instantiate(
                enemyList[Random.Range(0, enemyList.Count)],
                new Vector2(origScreenBounds.x * Mathf.Cos(randomAngle) * 0.4f, origScreenBounds.y * Mathf.Sin(randomAngle) * 0.4f),
                Quaternion.identity
            );
            //obj.gameObject.SetActive(false);
            enemies.Add(obj);
        }

        // DOJump wave by wave
        Vector2 landPos;
        int enemyIndex = 0;
        foreach(Transform i in enemies){
            landPos = i.position;   
            Sequence sq = DOTween.Sequence();
            sq
            .OnStart(()=>{
                screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
                screenOrigo = Camera.main.ScreenToWorldPoint(Vector2.zero);
                i.position /= 0.4f;
                i.position +=screenOrigo;
                i.gameObject.SetActive(true);
            })
            .Append(
                i
                .DOJump(
                    landPos,
                    jumpPower: 10f,
                    numJumps: 1,
                    duration: 0.6f
                )
                .OnComplete(()=>{
                    i
                    .DOScaleY(
                        0.7f,
                        0.2f
                    )
                    .SetLoops(2, LoopType.Yoyo);

                    i
                    .DOLocalMoveY(
                        -0.2f,
                        0.2f
                    )
                    .SetRelative()
                    .SetLoops(2, LoopType.Yoyo)
                    .OnComplete(()=>{
                        // activte attacking && player control
                        // TODO
                    });

                })
            )
            .SetDelay(enemyIndex * 3f);

            enemyIndex++;
        }
    }

    public void spawnBoss(){

    }
}
