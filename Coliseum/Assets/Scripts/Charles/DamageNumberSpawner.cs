using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class DamageNumberSpawner : MonoBehaviour
{
    public static DamageNumberSpawner Instance;
    public Transform dmgTextPrefab;
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

    private void Update() {
        if(Input.GetMouseButtonDown(0)){
            Vector3 a = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            spawnDamageNumberOnce(a, 99);
        }
    }

    public void spawnDamageNumberOnce(Vector3 pos, int dmg){
        Transform obj = Instantiate(dmgTextPrefab, pos + 1f * Vector3.up, Quaternion.identity);
        TextMeshPro tm = obj.GetComponent<TextMeshPro>();
        tm.alpha = 0f;
        var _rt = obj.GetComponent<RectTransform>();
        _rt.anchoredPosition3D = new Vector3(_rt.anchoredPosition.x, _rt.anchoredPosition.y, 10f);

        Sequence sq = DOTween.Sequence();
        sq
        .SetId("spawnDamageNumberOnce")
        .OnStart(()=>{
            tm.text = dmg.ToString();
        })
        .Append(
            obj
            .DOPunchPosition(
                punch: Vector3.down,
                duration: 0.3f
            )
        )
        .Join(
            tm
            .DOFade(
                1f,
                0.3f
            )
        )
        .Append(
            tm
            .DOFade(
                0f,
                1f
            )
        )
        .OnComplete(()=>{
            Destroy(obj.gameObject);
        });

    }
}
