﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
	public int totalHealth;
	private int health;

	public CharacterDeathEvent characterDeathEvent;
	public AllyType characterType;

    public int rewardUponDeath;

    public GameObject healthBar;
    public GameObject healthBarGreen;
    public float healthBarLength;

    public float invulnerabilityTime;
    private float invulnCounter;

    private GameManager gameManager;

	void Awake()
	{
		setup();
	}

    public void setup() {
        healthBar.SetActive(false);
        health = totalHealth;
        if (characterDeathEvent == null) {
            characterDeathEvent = new CharacterDeathEvent();
        }
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        characterDeathEvent.AddListener(KillCharacter);
    }

    public void Update() {
        if (invulnCounter > 0) {
            invulnCounter -= Time.deltaTime;
        }
    }

    void applyKnockBack(Vector3 impulse) {
        this.GetComponent<Rigidbody2D>().AddForce(impulse, ForceMode2D.Impulse);
        //this.GetComponent<Rigidbody2D>().velocity = (impulse);
        this.GetComponent<MovementController>().MovementEnabled = false;
    }

	void takeDamage(int i) {
		health -= i;

        if(i > 0) {
            SoundController.theController.playSound(SoundController.theController.damage);
        }

		if (health <= 0) {
            this.KillAndDestroy();
		} else if(health > totalHealth) {
            health = totalHealth;
        }

        healthBar.SetActive(true);
        Vector3 temp = healthBarGreen.transform.localScale;
        temp.x = (((float) health)/totalHealth);
        healthBarGreen.transform.localScale = temp;

        temp = healthBarGreen.transform.localPosition;
        temp.x = healthBarLength * (1 - (((float) health)/totalHealth)) * -0.5f;
        healthBarGreen.transform.localPosition = temp;
	}

    public void KillAndDestroy()
    {
        characterDeathEvent.Invoke(this);
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        Damager damageComponent = collision.gameObject.GetComponent<Damager>();
        if (damageComponent != null) {
            onCollision(damageComponent);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Damager damageComponent = collision.gameObject.GetComponent<Damager>();
        if (damageComponent != null) {
            onCollision(damageComponent);
        }
    }

    private void onCollision(Damager damageComponent) {
        if (damageComponent.damageSource != characterType && !isInvulnerable()) {
            Vector3 deltaX = this.transform.position - damageComponent.transform.position;
            Vector3 impulse = damageComponent.forceDirection * damageComponent.force;
            //impulse.x *= (deltaX.x >= 0 ? 1 : -1);
            impulse.x *= (damageComponent.transform.root.transform.localScale.x);
            this.applyKnockBack(impulse);
            this.takeDamage(damageComponent.damageAmount);
            this.GetComponent<CharacterAnimator>().Hurt();
            damageComponent.triggerContact();
            invulnCounter = invulnerabilityTime;
        }
    }

    public bool isInvulnerable() {
        return invulnCounter > 0;
    }

    void KillCharacter(Character character)
    {
        if(character.gameObject.tag == "Player")
        {
            gameManager.playerCharacterDies();
        }
        else 
        {
            gameManager.enemyDies(rewardUponDeath);
        }
    }
}
