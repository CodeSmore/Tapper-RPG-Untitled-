﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyHealthBar : MonoBehaviour {

	private float currentHealth;
	private float maxHealth;
	private float healthRatio;
	
	private Image healthBarImage;
	
	private EnemyMonster enemyMonster;

	// Use this for initialization
	void Start () {
		enemyMonster = GameObject.FindObjectOfType<EnemyMonster>();
		maxHealth = enemyMonster.GetMaxHealth();

		healthBarImage = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		currentHealth = enemyMonster.GetCurrentHealth();
		
		HandleHealthBar ();
	}
	
	void HandleHealthBar () {
		healthRatio = currentHealth / maxHealth;

		healthBarImage.fillAmount = healthRatio;

		// adjust color of healthbar
		if (healthRatio > 0.50f) {
			// green
			healthBarImage.color = Color.green;
		} else if (healthRatio > 0.25f) {
			// yellow
			healthBarImage.color = Color.yellow;
		} else {
			//red
			healthBarImage.color = Color.red;
		}
	}
}
