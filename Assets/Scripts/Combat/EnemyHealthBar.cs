﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyHealthBar : MonoBehaviour {

	private float currentHealth;
	private float maxHealth;
	private float healthRatio;
	private float poisTimer = 0;
	
	private Image healthBarImage;
	
	private EnemyMonster enemyMonster;

	// Use this for initialization
	void Start () {
		enemyMonster = GameObject.FindObjectOfType<EnemyMonster>();


		healthBarImage = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!enemyMonster) {
			enemyMonster = GameObject.FindObjectOfType<EnemyMonster>();
		} else {
			maxHealth = enemyMonster.GetMaxHealth();
			currentHealth = enemyMonster.GetCurrentHealth();

			if (enemyMonster.GetCurrentStatus() == StatusEffect.Pois) {
				poisTimer += Time.deltaTime;

				if (poisTimer >= 2) {
					poisTimer = 0;

					int poisDamage = Mathf.CeilToInt(enemyMonster.GetMaxHealth() * .035f);
					enemyMonster.TakeDamage(poisDamage);
				}
			}

			GetComponent<RectTransform>().anchoredPosition = new Vector2 (transform.position.x , enemyMonster.GetHealthBarYPos());

			HandleHealthBar ();
		}
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
