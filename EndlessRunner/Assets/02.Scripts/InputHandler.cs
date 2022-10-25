using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public const KeyCode SHORTCUT_PLAYER_JUMP = KeyCode.Space;
    public const KeyCode SHORTCUT_PLAYER_MOVE_LEFT = KeyCode.LeftArrow;
    public const KeyCode SHORTCUT_PLAYER_MOVE_RIGHT = KeyCode.RightArrow;
    public const KeyCode SHORTCUT_PLAYER_SLIDE = KeyCode.DownArrow;

    private static Dictionary<KeyCode, Action> keyDownActions = new Dictionary<KeyCode, Action>();

    public static void RegisterKeyDownAction(KeyCode keyCode, Action keyDownAction)
    {
        if (keyDownActions.ContainsKey(keyCode))
        {
            keyDownActions[keyCode] += keyDownAction;
        }
        else
        {
            keyDownActions.Add(keyCode, keyDownAction);
        }
    }

    public static void ClearKeyDownAction(KeyCode keyCode) => keyDownActions.Remove(keyCode);

    private void Update()
    {
        foreach (var pair in keyDownActions)
        {
            if (Input.GetKeyDown(pair.Key))
                pair.Value?.Invoke();
        }
    }
}
