using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballControiler : MonoBehaviour
{
    public QuestMachine questMachine;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(2);
            // 如果碰到的是玩家，呼叫 CollectItem 方法
            if (questMachine != null)
            {
                questMachine.CollectItem();

                if (questMachine.isQuestStarted)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
