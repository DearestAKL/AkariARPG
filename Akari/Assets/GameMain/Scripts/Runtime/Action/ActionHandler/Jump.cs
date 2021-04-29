﻿using Akari;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[ActionConfig(typeof(Jump))]
public class JumpConfig
{
    public string nextState;
    public float minHeight = 1;
    public float maxHeight = 3;
    public float moveSpeed = 2;
}

public class Jump : IActionHandler
{
    public void Enter(ActionNode node)
    {
        JumpConfig config = (JumpConfig)node.config;
        IActionMachine machine = node.actionMachine;
        ActionMachineController controller = (ActionMachineController)node.actionMachine.controller;

        float ySpeed = MathUtility.JumpSpeed(Physics.gravity.y, config.maxHeight);

        Vector3 velocity = controller.rigid.velocity;
        velocity.y = ySpeed;
        controller.rigid.velocity = velocity;
    }

    public void Exit(ActionNode node)
    {
    }

    public void Update(ActionNode node, float deltaTime)
    {
        JumpConfig config = (JumpConfig)node.config;
        IActionMachine machine = node.actionMachine;
        ActionMachineController controller = (ActionMachineController)node.actionMachine.controller;
        Vector3 velocity = controller.rigid.velocity;

        bool velocityChanged = false;

        if (!GameEntry.Input.HasEvent(InputEvents.Jumping))
        {
            float ySpeed = MathUtility.JumpSpeed(Physics.gravity.y, config.minHeight);
            if (velocity.y > ySpeed)
            {//限制到最小速度
                velocity.y = ySpeed;
                velocityChanged = true;
            }
        }

        if (GameEntry.Input.HasEvent(InputEvents.Moving))
        {//空中移动
            var move = GameEntry.Input.AxisValue.normalized * config.moveSpeed;
            velocity.x = move.x;
            velocity.z = move.y;
            velocityChanged = true;
        }

        if (velocityChanged)
        {
            controller.rigid.velocity = velocity;
        }

        if (controller.isGround)
        {//落地跳转
            machine.ChangeState(config.nextState);
        }
    }
}