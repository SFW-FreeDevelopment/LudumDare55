using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager
{
    public static event UnityAction OnPlayerMoved;
    public static void PlayerMoved() => OnPlayerMoved?.Invoke();
}
