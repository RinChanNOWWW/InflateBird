using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    private bool isDead = false;
    private Rigidbody2D rb;
    private Animator anim;

    // Params for velocity
    private readonly float gravityVelocity = -10;
    private readonly float baseVelocity = 22;
    private readonly float velocityRatio = 10;

    // How big is the object.
    private readonly float maxSize = 3;
    private readonly float growSpeed = 0.7f;
    private readonly float shrinkSpeed = 1;
    private bool isGrowing = false;


    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D> ();
        anim = GetComponent<Animator> ();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead == false)
        {
            UpdateSize();
            UpdateVelocity();
            anim.SetTrigger("Flap");
        }
    }

    // Update the object size.
    void UpdateSize()
    {
        // 如果鼠标左键被按住，物体将持续变大
        if (Input.GetMouseButton(0))
        {
            isGrowing = true;
            transform.localScale += Vector3.one * growSpeed * Time.deltaTime;
            if (transform.localScale.x >= maxSize)
            {
                transform.localScale = Vector3.one * maxSize;
            }
        }
        // 如果鼠标左键被松开，物体将持续缩小
        else if (isGrowing)
        {
            transform.localScale -= Vector3.one * shrinkSpeed * Time.deltaTime;

            // 缩小到原始大小
            if (transform.localScale.x <= 1.0f)
            {
                transform.localScale = Vector3.one;
                isGrowing = false;
            }
        }
    }

    // Update the vertical velocity.
    void UpdateVelocity()
    {
        rb.velocity = new Vector2 (0, gravityVelocity + baseVelocity - velocityRatio * GetSize());
    }

    float GetSize()
    {
        return transform.localScale.x;
    }

    void OnCollisionEnter2D()
    {
        if (isDead == false)
        {
            rb.velocity = Vector2.zero;
            isDead = true;
            anim.SetTrigger("Die");
            GameControl.instance.BirdDied();
        }
    }

    void OnBecameInvisible()
    {
        // Set to dead if the bird is out of camera.
        rb.velocity = Vector2.zero;
        isDead = true;
        anim.SetTrigger("Die");
        GameControl.instance.BirdDied();
    }

}

