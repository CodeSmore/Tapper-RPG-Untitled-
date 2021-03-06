﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerInteractableController : MonoBehaviour {

	private EnemySpawnerController enemySpawnerController;
	private Button interactButton;
	private GameObject textBox;
	private InteractionTextController textController;
	private Animator transitionPanelAnimator;
	private UnityAdsExample unityAdsExample;
	private GameController gameController;

	private GameObject triggerHit;
	private GameObject interactTrigger;
	private Vector3 doorwayDestinationVector3;

	private bool timeToAnnoy = false;

	private int numOfCurrentInteractableCollisions = 0;

	// Use this for initialization
	void Start () {
		enemySpawnerController = GameObject.FindObjectOfType<EnemySpawnerController>();
		interactButton = GameObject.Find("Interact Button").GetComponent<Button>();
		interactButton.interactable = false;
		textBox = GameObject.Find("Text Box");
		textBox.SetActive(false);
		textController = GameObject.FindObjectOfType<InteractionTextController>();
		transitionPanelAnimator = GameObject.Find("Transition Panel").GetComponent<Animator>();
		unityAdsExample = GameObject.FindObjectOfType<UnityAdsExample>();
		gameController = GameObject.FindObjectOfType<GameController>();
	}

	void OnEnable () {
		// reset these values due to errors caused by going into/ out of battle and increasing interactable count
		ResetInteratableCount();
	}

	void OnTriggerEnter2D (Collider2D collider) {
		triggerHit = collider.gameObject;

		if (triggerHit.tag == "Interactable" || triggerHit.tag == "Destructable" || triggerHit.tag == "Message Interactable" || triggerHit.tag == "Grind Portal") {
			numOfCurrentInteractableCollisions++;
		}

		// If player collides with SpawnArea collider, the SpawnType in enemySpawnerController is set
		// This is in OnTriggerEnter b/c it only needs updated when venue is changed
		if (triggerHit.tag == "SpawnArea") {
			enemySpawnerController.SetSpawnType(triggerHit.name);
		}

		if (triggerHit.tag == "BattleTrigger" || triggerHit.tag == "Boss") {
			if (triggerHit.tag == "Boss") {
				enemySpawnerController.SetSpawnType(triggerHit.name);
			}
			collider.gameObject.SetActive(false);
			gameController.BeginBattle();
		} 

		if (triggerHit.tag == "Doorway") {
			if (triggerHit.GetComponent<Doorway>().GetIsAdPortal()) {
				// player ad
				unityAdsExample.ShowAd();
			}

			// deactivate interact button AND set interactions to zero
			interactButton.interactable = false;
			numOfCurrentInteractableCollisions = 0;
			// trigger animation
			gameObject.GetComponent<PlayerMovement>().SetMovementIsEnabled(false);

			transitionPanelAnimator.SetTrigger("DoorwayTransitionTrigger");
			// TODO animation then triggers this portion
			doorwayDestinationVector3 = collider.GetComponent<Doorway>().GetDestinationVector();

			// if entering Tabuz Maze, increase spawn rate
			// This info is grabbed by GameController.cs
			if (triggerHit.name == "Bad Portal") {
				timeToAnnoy = true;
			} else {
				timeToAnnoy = false;
			}
		}
	}

	void OnTriggerStay2D (Collider2D collider) {
		interactTrigger = collider.gameObject;

		if (interactTrigger.tag == "Interactable" || interactTrigger.tag == "Destructable" || interactTrigger.tag == "Message Interactable" || interactTrigger.tag == "Grind Portal") {
			interactButton.interactable = true;
		} 
	}

	void OnTriggerExit2D (Collider2D collider) {
		if (collider.gameObject.tag == "Interactable" || collider.gameObject.tag == "Destructable" || collider.gameObject.tag == "Message Interactable" || collider.gameObject.tag == "Grind Portal") {
			numOfCurrentInteractableCollisions--;
			if (numOfCurrentInteractableCollisions == 0) {
				interactButton.interactable = false;
			}
		}
	}

	public void Interact () {
		if (interactTrigger.GetComponent<CombinationBlock>()) {
			interactTrigger.GetComponent<CombinationBlock>().IncrementBlock();
		} else if (interactTrigger.tag == "Message Interactable") {
			textBox.SetActive(true);
			textController.UpdateText(interactTrigger.name);
		} else if (interactTrigger.tag == "Destructable") {
			Destroy(interactTrigger);
			interactButton.interactable = false;
		} else if (interactTrigger.tag == "Grind Portal") {
			// spawn monster
			gameController.BeginBattle();
			Debug.Log("Begin Grinding");
		}
	}

	public void TeleportPlayer () {
		transform.position = doorwayDestinationVector3;
	}

	public void DeactivateTextBox () {
		textBox.SetActive(false);
	}

	public bool IsTimeToAnnoy () {
		return timeToAnnoy;
	}

	public int GetNumberOfInteractableCollisions () {
		return numOfCurrentInteractableCollisions;
	}

	public void DisableInteractButton () {
		interactButton.interactable = false;
	}

	public void ResetInteratableCount () {
		numOfCurrentInteractableCollisions = 0;
	}
}
