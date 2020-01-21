using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    Animator explosion;
    GameObject background;

    private void Start()
    {
        explosion = this.GetComponent<Animator>();
        explosion.enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "guard")
        {
            explosion.enabled = true;
            Destroy(this.gameObject, 0.2f);
        }
    }

}
