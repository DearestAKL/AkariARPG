using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akari
{
    /// <summary>
    /// GameObjectExtensions
    /// </summary>
    public static class GameObjectExtensions
    {
        /// <summary>
        /// 获取物体在场景中的位置
        /// </summary>
        /// <param name="GameObject"></param>
        /// <returns></returns>
        public static string GetScenePath(this GameObject go)
        {
            if (go != null && go.transform != null)
            {
                return go.transform.GetScenePath();
            }
            return string.Empty;
        }
    }
}
