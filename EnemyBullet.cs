using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
   private void OnCollisionEnter(Collision collision)
   {
        if (collision.gameObject.CompareTag("Player"))
        {
            print("hit the Player");
            Destroy(gameObject);
        }
   }
}
