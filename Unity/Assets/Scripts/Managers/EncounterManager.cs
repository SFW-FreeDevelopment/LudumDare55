using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EncounterManager : MonoBehaviour
{
    [SerializeField] public LayerMask encounterZone;
    [SerializeField] public Transform player;

    private int count;

    private void OnEnable()
    {
        EventManager.OnPlayerMoved += EventManagerOnPlayerMoved;
    }

    private void OnDisable()
    {
        EventManager.OnPlayerMoved -= EventManagerOnPlayerMoved;
    }

    private void EventManagerOnPlayerMoved()
    {
        var inEncounterZone = InEncounterZone();
        if (inEncounterZone)
        {
            var encounterCheck = DoEncounterRoll();
            if (encounterCheck)
            {
                DoEncounter();
            }
        }
    }

    private bool DoEncounterRoll()
    {
        if (Random.Range(1, 100) <= 100)
        {
            return true;
        }
        return false;
    }

    private void DoEncounter()
    {
        Debug.Log($"Encountered pokemon {count}");
        count++;
    }

    private bool InEncounterZone()
    {
        if (Physics2D.OverlapCircle(player.position, 0.2f) != null)
        {
            return true;
        }
        return false;
    }
}
