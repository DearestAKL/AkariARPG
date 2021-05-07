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
            Vector3 desiredVelocity;
            var input = Vector2.ClampMagnitude(GameEntry.Input.AxisValue, 1f);
            var cameraTrans = GameEntry.Camera.MainCamera.transform;
            if (cameraTrans)
            {
                Vector3 forward = cameraTrans.forward;
                forward.y = 0f;
                forward.Normalize();
                Vector3 right = cameraTrans.right;
                right.y = 0f;
                right.Normalize();
                desiredVelocity = (forward * input.y + right * input.x) * config.moveSpeed;
            }
            else
            {
                desiredVelocity = new Vector3(input.x, 0f, input.y) * config.moveSpeed;
            }

            velocity.x = desiredVelocity.x;
            velocity.z = desiredVelocity.z;
        }
        else
        {
            velocity.x = 0;
            velocity.z = 0;
        }

        controller.CachedRigidbody.velocity = velocity;
    }
}