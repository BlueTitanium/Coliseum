using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : MonoBehaviour
{

    public static ArenaManager Instance;

    // statics
    public int round;
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
}

