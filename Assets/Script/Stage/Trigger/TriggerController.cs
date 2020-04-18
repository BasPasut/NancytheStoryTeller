﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class TriggerController : MonoBehaviour
{
    public TriggerSpawnController triggerSpawnController;
    public TriggerActionController triggerActionController;
    public TriggerTimelineController triggerTimelineController;

    public List<Trigger> triggerPointList;

    private static int currentTriggerIndex;

    private static int lastTriggerIndex;

    // Start is called before the first frame update
    public void Start()
    {
        lastTriggerIndex = triggerPointList.Count - 1;
        currentTriggerIndex = 0;
        int indexSpawn = 0;
        int indexTimeline = 0;
        for (int i = 0; i < triggerPointList.Count; i++)
        {
            triggerPointList[i].gameObject.SetActive(false);
            switch (triggerPointList[i].GetTriggerType())
            {
                case TriggerType.Action:
                    Item itemTrigger = triggerPointList[i].GetItemWithAction();
                    triggerActionController = new TriggerActionController();
                    triggerActionController.ReceivedTargetObject(itemTrigger);
                    triggerPointList[i].PerformActionWithoutObject(triggerActionController.StartTargetAction);
                    break;
                case TriggerType.Spawn:
                    triggerPointList[i].PerformActionWithoutObject(() => triggerSpawnController.Spawn(indexSpawn++));
                    break;
                case TriggerType.Timeline:
                    triggerPointList[i].PerformActionWithoutObject(() => triggerTimelineController.PlayTimeline(indexTimeline++));
                    break;
                default:
                    break;
            }
        }
    }

    private void Update()
    {
        try
        {
            if (currentTriggerIndex <= triggerPointList.Count)
            {
                triggerPointList[currentTriggerIndex].gameObject.SetActive(true);
                if (triggerPointList[currentTriggerIndex].GetIsTrigger())
                {
                    triggerPointList[currentTriggerIndex].gameObject.SetActive(false);
                    currentTriggerIndex++;
                }
            }
        }
        catch (Exception e)
        {
            //Debug.Log(e);        
        }
    }

    public static int GetCurrentTriggerIndex()
    {
        return currentTriggerIndex;
    }

    public static int GetLastTriggerIndex()
    {
        return lastTriggerIndex;
    }
}