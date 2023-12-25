using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class FruitPhysics : MonoBehaviour
{
    public Fruit type;

    private Rigidbody2D rb;
    private Collider2D col;

    public bool valid = true;

    private Dropper dropper;
    private bool isFirstCol = true;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        col = gameObject.GetComponent<CircleCollider2D>();
        rb.sharedMaterial = GameController.instance.gameSettings.physicsMaterial;
        valid = true;
    }

    public void Drop(Dropper source)
    {
        dropper = source; 
        transform.parent = null;
        col.enabled = true;
        rb.constraints = RigidbodyConstraints2D.None;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;

        // Tell Dropper to make new fruit
        if (isFirstCol && dropper)
        {
            isFirstCol = !dropper.SpawnNewFruitAfterPreviousCollide(this);
        }

        if (!other.CompareTag("Fruit"))
        {
            return;
        }

        

        // Call merger
        FruitPhysics otherFruitPhysics = other.GetComponent<FruitPhysics>();
        if (valid && otherFruitPhysics.valid && otherFruitPhysics.type.size == this.type.size)
        {
            GameController.instance.HandleFruitMerge(this, otherFruitPhysics, this.type.size);
        }
    }
}
