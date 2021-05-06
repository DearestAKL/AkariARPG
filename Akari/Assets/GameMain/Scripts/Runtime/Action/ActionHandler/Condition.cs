using Akari;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[ActionConfig(typeof(Condition))]
public class ConditionConfig : HoldFrames
{
    public string stateName;
    public int priority;

    [SerializeReference]
    [Conditions.ConditionTypes]
    public List<Conditions.IChecker> checker;

    public override string ToString()
    {
        return $"{GetType().Name} > {stateName} - {priority}";
    }
}

public class Condition : IActionHandler
{
    public void Enter(ActionNode node)
    {
        Debug.Log("Condition Enter");
    }

    public void Exit(ActionNode node)
    {
        Debug.Log("Condition Exit");
    }

    public void Update(ActionNode node, float deltaTime)
    {
        Debug.Log("Condition Update");

        ConditionConfig config = (ConditionConfig)node.config;
        IActionMachine machine = node.actionMachine;
        //ActionMachineController controller = (ActionMachineController)node.actionMachine.controller;

        if (Checker(config.checker, node))
        {
            machine.ChangeState(config.stateName, config.priority);
        }
    }

    public static bool Checker(List<Conditions.IChecker> checkers, ActionNode node)
    {
        if (checkers == null || checkers.Count == 0)
        {
            return true;
        }

        foreach (var checker in checkers)
        {
            if (!checker.Execute(node))
            {
                return false;
            }
        }

        return true;
    }
}

namespace Conditions
{
    public class ConditionTypesAttribute : ObjectTypesAttribute
    {
        public override Type baseType => typeof(IChecker);
    }

    public interface IChecker
    {
        bool Execute(ActionNode node);
    }

    [Serializable]
    public class KeyCodeChecker : IChecker
    {
        public InputEvents events;
        public bool isNot;
        public bool fullMatch;

        public bool Execute(ActionNode node)
        {
            IActionMachine machine = node.actionMachine;
            //ActionMachineController controller = (ActionMachineController)node.actionMachine.controller;
            bool result = GameEntry.Input.HasEvent(events, fullMatch);
            return isNot ? !result : result;
        }
    }

    [Serializable]
    public class GroundChecker : IChecker
    {
        public bool isNot;

        public bool Execute(ActionNode node)
        {
            IActionMachine machine = node.actionMachine;
            TargetableObject controller = (TargetableObject)node.actionMachine.controller;
            return isNot ? !controller.IsGround : controller.IsGround;
        }
    }

    public class HitChecker : IChecker
    {

    }
}