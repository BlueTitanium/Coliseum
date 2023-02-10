using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomCursor : MonoBehaviour
{
    public Vector3 mDisplacement;
    public Animation a;
    public Image crosshair;
    void Start()
    {
        // this sets the base cursor as invisible
        Cursor.visible = false;
    }

    void Update()
    {
        if (Cursor.visible)
        {
            Cursor.visible = false;
        }

        transform.position = Input.mousePosition + mDisplacement;
        if(Input.GetMouseButtonDown(0))
            a.Play();
        
        crosshair.fillAmount = (PlayerController.p.attackCD - PlayerController.p.attackCDLeft) / PlayerController.p.attackCD;
    }
}