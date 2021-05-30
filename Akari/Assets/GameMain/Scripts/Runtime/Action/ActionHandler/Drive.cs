using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Akari 
{
    public enum CarType
    {
        None = 0,
        Motorcycle = 1,
    }

    [System.Serializable]
    [ActionConfig(typeof(Drive))]
    public class DriveConfig
    {

    }

    public class Drive : IActionHandler
    {
        public void Enter(ActionNode node)
        {
            DriveConfig config = (DriveConfig)node.config;
            Hero controller = (Hero)node.actionMachine.controller;
            if(controller.car == null)
            {
                return;
            }
            GameEntry.Input.IsProhibitMove = false;

            controller.EnterDrive(controller.car.carType);
        }

        public void Exit(ActionNode node)
        {

        }

        public void Update(ActionNode node, float deltaTime)
        {
            if (GameEntry.Input.HasEvent(InputEvents.F))
            {
                Hero controller = (Hero)node.actionMachine.controller;
                controller.LeaveDrive();
            }
        }
    }
}
