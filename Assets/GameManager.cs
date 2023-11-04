using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using Unity.Mathematics;
using UnityEditor.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    float hiveHoneyLevel = 90;
    float levelLength = 20;
    int numberOfFlowers = 3;
    int flowersCollectedFrom = 0;
    float difficulty = 0.2f;

    public GameObject playerPrefab;
    public GameObject flowerPrefab;
    public GameObject hivePrefab;
    public GameObject floorPrefab;
    public GameObject waspPrefab;

    GameObject player;
    ArrayList levelObjects = new();
    PlayerController playerController;

    public GameObject hiveHoneyLevelText;
    TextMeshProUGUI hiveHoneyLevelTextComponent;
    public GameObject beeHoneyLevelText;
    TextMeshProUGUI beeHoneyLevelTextComponent;

    void Start()
    {
        hiveHoneyLevelTextComponent = hiveHoneyLevelText.GetComponent<TextMeshProUGUI>();
        beeHoneyLevelTextComponent = beeHoneyLevelText.GetComponent<TextMeshProUGUI>();

        InstantiatePlayer();
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        CreateLevel();
    }

    void Update()
    {
        hiveHoneyLevel -= Time.deltaTime;
        hiveHoneyLevelTextComponent.SetText("Hive Honey Level: " + hiveHoneyLevel.ToString("F2"));
    }


    void CreateLevel() {
        flowersCollectedFrom = 0;
        //levelLength = 50 * difficulty + 40;
        float levelSegment = levelLength / numberOfFlowers;
        // Create the hive at the start then the flowers evenly spread over the length of the level
        levelObjects.Add(Instantiate(hivePrefab, new Vector3(0, 0, 0), Quaternion.identity));
        for (int i = 1; i <= numberOfFlowers; i++) {
            levelObjects.Add(Instantiate(flowerPrefab, new Vector3(levelSegment * i, 0, 0), Quaternion.identity));
        }
        // Create the floors as a child of a new empty (cleans up the inspector)
        GameObject floorParent = new GameObject();
        floorParent.name = "Floors";
        for (int i = 0; i <= levelLength; i++) {
            Instantiate(floorPrefab, new Vector3(i, -5, 0), Quaternion.identity, floorParent.transform);
        }
        levelObjects.Add(floorParent);
        for (int i = 0; i < 4; i++) {
            GameObject wasp = Instantiate(waspPrefab, new Vector3(UnityEngine.Random.value * (levelLength - 10) + 10, 3, 0), quaternion.identity);
            levelObjects.Add(wasp);
            wasp.GetComponent<WaspController>().SetTarget(player);
        }
        UpdateBeeHoneyHeldText();
    }

    public void depositHoneyInHive(float honey) {
        hiveHoneyLevel += honey;
        UpdateBeeHoneyHeldText();
        if (flowersCollectedFrom >= numberOfFlowers) {
            CreateNewLevel();
        }
    }

    public void playerDied() {
        if (flowersCollectedFrom >= numberOfFlowers) {
            CreateNewLevel();
            return;
        }
        playerController.ResetPlayer();
        UpdateBeeHoneyHeldText();
    }

    public void InstantiatePlayer() {
        player = Instantiate(playerPrefab);
        playerController = player.GetComponent<PlayerController>();
        playerController.SetGameManager(this);
    }

    public void CollectedFromFlower() {
        flowersCollectedFrom += 1;
        Debug.Log("FlowersCollectedFrom = " + flowersCollectedFrom);
        UpdateBeeHoneyHeldText();
    }

    void CreateNewLevel() {
        Destroy(player);
        for (int i = levelObjects.Count - 1; i >= 0; i--) {
            Destroy((Object)levelObjects[i]);
        }
        levelObjects = new ArrayList();
        InstantiatePlayer();
        CreateLevel();
    }

    void UpdateBeeHoneyHeldText() {
        beeHoneyLevelTextComponent.SetText("Honey Held: " + playerController.getHoneyHeld());
    }
}
