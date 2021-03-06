﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealthBar : MonoBehaviour {
	
	private float currentHealth;
	private float maxHealth;
	private float healthRatio;
	private float poisTimer;
	
	private Image healthBarImage;
	private Text playerHealthText;
	
	private PlayerClass playerClass;

	// Use this for initialization
	void Start () {
		playerClass = GameObject.FindObjectOfType<PlayerClass>();
		maxHealth = playerClass.GetMaxHealth();

		healthBarImage = GetComponent<Image>();
		playerHealthText = GetComponentInChildren<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		currentHealth = playerClass.GetCurrentHealth();
		maxHealth = playerClass.GetMaxHealth();

		if (playerClass.GetCurrentStatus() == StatusEffect.Pois) {
			poisTimer += Time.deltaTime;

			if (poisTimer >= 2) {
				poisTimer = 0;

				int poisDamage = Mathf.CeilToInt(playerClass.GetMaxHealth() * .035f);
				playerClass.TakeDamage(poisDamage);
			}
		}
	
		HandleHealthBar ();
	}
	
	void HandleHealthBar () {
		healthRatio = currentHealth / maxHealth;
		playerHealthText.text = currentHealth + " / " + maxHealth;

		healthBarImage.fillAmount = healthRatio;

		// Adjust color of healthbar
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
