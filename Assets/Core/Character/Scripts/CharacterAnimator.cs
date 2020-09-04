﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GDG;

public class CharacterAnimator : MonoBehaviour
{
    protected bool facingDirection = true; //true means spirte is facing right
    protected Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void Move(bool isMoving = true)
    {
        animator.SetBool("Moving", isMoving);
    }

    public void Jump()
    {
        SoundManager.Instance.PlayJump();
        animator.SetTrigger("Jump");
    }

    public void Ground(bool isGrounded = true)
    {
        animator.SetBool("Grounded", isGrounded);
    }

    public void Hurt()
    {
        animator.SetTrigger("Hurt");
    }

    public void faceLeft()
    {
        if (facingDirection)
        {
            horizontalDisplayFlip();
            facingDirection = false;
        }
    }

    public void faceRight()
    {
        if (!facingDirection)
        {
            horizontalDisplayFlip();
            facingDirection = true;
        }
    }

    private void horizontalDisplayFlip()
    {
        transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
        //Flips the sprite along the x-axis
    }

    public bool getFacingDirection()
    {
        return facingDirection;
    }
}
