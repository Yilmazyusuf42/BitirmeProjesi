using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damage : MonoBehaviour
{
    public int damage1 = 2;
    private PlayerHealth playerHealth;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(playerHealth == null)
            {
                 playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

            }
            Debug.Log("feefefefefefef");
            playerHealth.TakeDamage(damage1);
        }

           
    }
}
