using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestMachine : MonoBehaviour
{
    public float interactionRadius = 2f; // 互動範圍
    public int collectedItemgoal; //蒐集數量
    private bool isPlayerInRange = false; //是否在機器範圍
    public Transform playerTransform;  //追蹤玩家
    private int collectedItems = 0; //一開始蒐集數量為0
    public Text interactionText; //UI
    private bool isCheckingDistance = true; //是否測量玩家與機器距離
    public bool isQuestStarted = false;
    void Update()
    {
        CheckPlayerInput(); //檢查玩家輸入
    }
    void CheckPlayerInput()
    {
        if (isCheckingDistance && isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            StartQuest();
            isCheckingDistance = false;
        }
        else if(isCheckingDistance)
        {
            CheckPlayerDistance();
        }
    }

    void CheckPlayerDistance()
    {
        Vector3 playerPosition = playerTransform.position;
        float distanceToPlayer = Vector3.Distance(transform.position, playerPosition);

        if (distanceToPlayer <= interactionRadius)
        {
            // 玩家在互動範圍內
            isPlayerInRange = true;
            Debug.Log("1");
            // 在這裡你可以顯示提示字樣
            interactionText.text = "I'm hungry can you please find me some food? (Press E to start the mission)";
        }
        else
        {
            // 玩家在互動範圍外
            isPlayerInRange = false;
            Debug.Log("0");
            // 在這裡你可以隱藏提示字樣
            interactionText.text = "";
        }
    }

    void StartQuest()
    {
        isQuestStarted = true;
        // 在這裡觸發任務的開始，你可以執行一些初始化工作
        Debug.Log("Quest Started!");
        interactionText.text = "Collected:" + collectedItems + "/" + collectedItemgoal;
        // 初始化
        collectedItems = 0;
    }
    public void CollectItem()
    {
        if (isQuestStarted)
        {
            collectedItems++;
            Debug.Log("Item Collected! Total: " + collectedItems);

            // 在這裡更新 UI 顯示
            if (collectedItems < collectedItemgoal)
            {
                interactionText.text = "Collected:" + collectedItems + "/" + collectedItemgoal; // 更新 UI 顯示
            }
            else if (collectedItems == collectedItemgoal)
            {
                CompleteQuest();
            }
        }
    }
    void CompleteQuest()
    {
        Debug.Log("Quest Completed!");
        interactionText.text = "Mission accomplished!"; // 任務完成的提示文字
        // 在這裡處理任務完成後的相應操作
    }
}
