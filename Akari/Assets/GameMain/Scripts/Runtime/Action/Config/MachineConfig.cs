using System;
using System.Collections.Generic;
using UnityEngine;

namespace Akari
{
    /// <summary>
    /// MachineConfig
    /// </summary>
    [Serializable]
    public class MachineConfig
    {
        public string firstStateName;
        public List<StateConfig> states = new List<StateConfig>();

        [SerializeReference]
        public List<object> globalActions = new List<object>();
    }
}
