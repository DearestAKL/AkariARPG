using Akari;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[ActionConfig(typeof(Move))]
public class MoveConfig
{
    public float moveSpeed;
}

public class Move : IActionHandler
{
    public void Enter(ActionNode node)
    {
        Debug.Log("Move Enter");
        GameEntry.Input.IsProhibitMove = false;
    }

    public void Exit(ActionNode node)
    {
        Debug.Log("Move Exit");
    }

    public void Update(ActionNode node, float deltaTime)
    {
        Debug.Log("Move Update");

        MoveConfig config = (MoveConfig)node.config;
        //IActionMachine machine = node.actionMachine;
        TargetableObject controller = (TargetableObject)node.actionMachine.controller;

        var velocity = controller.CachedRigidbody.velocity;
        if (GameEntry.Input.HasEvent(InputEvents.Moving))
        {
            Vector2 desiredVelocity = GameEntry.Input.GetEffectiveCameraAxisValue()* config.moveSpeed;

            velocity.x = desiredVelocity.x;
            velocity.z = desiredVelocity.y;
        }
        else
        {
            velocity.x = 0;
            velocity.z = 0;
        }

        controller.CachedRigidbody.velocity = velocity;
    }
}