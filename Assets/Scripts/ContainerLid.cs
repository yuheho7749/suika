using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerLid : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rb.velocity.y >= 0)
        {
            GameController.current.Lose();
        }

    }
}
