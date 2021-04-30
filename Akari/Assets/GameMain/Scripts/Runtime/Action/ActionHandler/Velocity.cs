﻿using Akari;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[ActionConfig(typeof(Velocity))]
public class VelocityConfig : HoldFrames
{
    public Vector3 velocity;
}

public class Velocity : IActionHandler
{
    public void Enter(ActionNode node)
    {
        VelocityConfig config = (VelocityConfig)node.config;
        IActionMachine machine = node.actionMachine;
        TargetableObject controller = (TargetableObject)node.actionMachine.controller;

        controller.CachedRigidbody.velocity = controller.CachedTransform.rotation * config.velocity;
    }

    public void Exit(ActionNode node)
    {
    }

    public void Update(ActionNode node, float deltaTime)
    {
    }
}