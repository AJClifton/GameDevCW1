using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerController : MonoBehaviour
{

    CharacterController characterController;
    public float honeyHeld = 0;
    float maxHoneyHoldable = 90;
    float honeyMovementImpact = 0.4f; // see speed comment
    float speed = 6;

    GameManager gameManager = null;

    void Start()
    {

    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector3 movementVector = new(x, y, 0);
        // speed y=3\cdot\frac{90-ax}{90}\left\{1<x<90\right\} <- put in desmos
        transform.position += (maxHoneyHoldable - honeyMovementImpact*honeyHeld) / maxHoneyHoldable * speed * Time.deltaTime * movementVector.normalized;            

    }

    public void SetGameManager(GameManager manager) {
        gameManager = manager;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Flower")) {
            if (other.gameObject.TryGetComponent<FlowerController>(out var flowerController)) {
                float honeyGained = flowerController.TakeHoney();
                if (honeyGained > 0) {
                    honeyHeld += honeyGained;
                    if (honeyHeld > maxHoneyHoldable) {
                        honeyHeld = maxHoneyHoldable;
                    }
                    gameManager.CollectedFromFlower();
                }
            }
        }
        else if (other.CompareTag("Hive")) {
            if (gameManager != null) {
                gameManager.depositHoneyInHive(honeyHeld);
                honeyHeld = 0;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.CompareTag("Floor") || col.gameObject.CompareTag("Wasp")) {
            gameManager.playerDied();
        }
    }

    public void ResetPlayer() {
        transform.position = Vector3.zero;
        honeyHeld = 0;
    }


    public float getHoneyHeld() {
        return honeyHeld;
    }
}
