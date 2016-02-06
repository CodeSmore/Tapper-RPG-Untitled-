﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class BattleResultsController : MonoBehaviour {

	public Text expGainedText;
	public Text totalExpText;

	private PlayerClass playerClass;
	private EnemyMonster enemyMonster;
	private GameController gameController;
	private GameObject pauseButton;
	private EnemyActionBar enemyActionBar;
	private SkillButtonController[] skillButtonControllers;

	// Level Up Battle Results
	public GameObject levelUpHeader;
	public Text levelOld;
	public Text levelNew;
	public Text HPOld;
	public Text HPNew;
	public Text EPOld;
	public Text EPNew;
	public Text attackStatOld;
	public Text attackStatNew;
	public GameObject tapToContinueObject;


	private float resultsTimer;
	public float enableTransitionTime;

	void Awake () {
		enemyActionBar = GameObject.FindObjectOfType<EnemyActionBar>();
		playerClass = GameObject.FindObjectOfType<PlayerClass>();
		skillButtonControllers = GameObject.FindObjectsOfType<SkillButtonController>();
		enemyMonster = GameObject.FindObjectOfType<EnemyMonster>();
		levelUpHeader.SetActive(false);
	}

	// Use this for initialization
	void Start () {
		gameController = GameObject.FindObjectOfType<GameController>();
		pauseButton = GameObject.Find("Pause Button");
		tapToContinueObject.SetActive(false);

		resultsTimer = 0;
	}

	void Update () {
		if (pauseButton.activeSelf) {
			pauseButton.SetActive(false);
		}

		if (!enemyMonster) {
			enemyMonster = GameObject.FindObjectOfType<EnemyMonster>();
		}

		resultsTimer += Time.deltaTime;

		if (resultsTimer > enableTransitionTime) {
			tapToContinueObject.SetActive(true);
		}
	}

	public void EndOfBattlePreparations () {
		enemyMonster.ResetHealth();

		tapToContinueObject.SetActive(false);
		enemyActionBar.gameObject.SetActive(true);

		enemyActionBar.ResetActionBar();
		Destroy(enemyMonster.gameObject);

		pauseButton.SetActive(true);
		gameObject.SetActive(false);
	}

	void OnMouseUp () {
		if (resultsTimer > enableTransitionTime) {
			// trigger
			gameController.TransitionToWorld();
		}
	}

	void OnDisable () {
		foreach (SkillButtonController controller in skillButtonControllers) {
			if (controller.Unlocked()) {
				controller.SetCooldownActive(true);
			}	
		}

		levelUpHeader.SetActive(false);
	}

	void OnEnable () {
		foreach (SkillButtonController controller in skillButtonControllers) {
			if (controller.Unlocked()) {
				controller.SetCooldownActive(false);
			}	
		}
	}

	public void UpdateBattleResults (float expGained, float totalExp, float expForNextLevel) {
		expGainedText.text = expGained / expForNextLevel * 100 + "%";
		totalExpText.text = totalExp / expForNextLevel * 100 + "%";

		if (totalExp >= expForNextLevel) {
			levelUpHeader.SetActive(true);

			// original stats set before level-up
			levelOld.text = playerClass.GetPlayerLevel().ToString(); 
			HPOld.text = playerClass.GetMaxHealth().ToString();
			EPOld.text = playerClass.GetMaxEnergy().ToString();
			attackStatOld.text = playerClass.GetAttackStat().ToString();

			playerClass.LevelUp();

			// ends the cooldown phase of each earned skill button upon LevelUp
			foreach (SkillButtonController controller in skillButtonControllers) {
				if (controller.Unlocked()) {
					controller.ActivateButton();
					controller.EndCooldown();
				}	
			}
			// TODO create method that resets all relevant variables when a level-up occurs 
			// in order to avoid having to update them every update()


			// display level up and stat increases

			// post-level-up stats
			levelNew.text = playerClass.GetPlayerLevel().ToString(); 
			HPNew.text = playerClass.GetMaxHealth().ToString();
			EPNew.text = playerClass.GetMaxEnergy().ToString();
			attackStatNew.text = playerClass.GetAttackStat().ToString();
		} 

		// start timer to keep screen up for a second
		resultsTimer = 0;
	}
}
