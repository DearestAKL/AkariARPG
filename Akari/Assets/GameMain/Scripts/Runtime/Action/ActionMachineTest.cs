using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Akari
{
    /// <summary>
    /// TestActionMachine
    /// </summary>
    [RequireComponent(typeof(AnimatorTest))]
    public class ActionMachineTest : MonoBehaviour
    {
        public TextAsset config;
        public UnityEngine.Matrix4x4 localToWorldMatrix => transform.localToWorldMatrix;
        public bool destroyOnPlay;

        private void Awake()
        {
            if (destroyOnPlay)
            {
                Destroy(gameObject);
            }
        }
    }
}
