using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaBoundary : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController.p.TakeKnockback(new Vector2(0, 0), 10f);
            PlayerController.p.GetComponent<Animation>().Play();
        }
    }
}
