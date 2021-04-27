using System;
using System.Text;
using UnityEngine;

namespace Akari
{
    /// <summary>
    /// Transform 扩展
    /// </summary>
    public static class TransformExtensions
    {
        /// <summary>
        /// 获取物体在场景中的位置
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static string GetScenePath(this Transform transform)
        {
            if (transform.gameObject == null || !transform.gameObject.scene.IsValid())
            {
                return string.Empty;
            }

            StringBuilder builder = new StringBuilder(transform.name);

            Transform parent = transform.parent;
            while (parent != null)
            {
                builder
                    .Insert(0, '/')
                    .Insert(0, parent.name);
                parent = parent.parent;
            }

            return builder.ToString();
        }

        /// <summary>
        /// 遍历每个子节点
        /// </summary>
        /// <param name="transform">实例</param>
        /// <param name="callback">返回子节点</param>
        public static void ForeachChild(this Transform transform, Action<Transform> callback)
        {
            foreach (Transform child in transform)
            {
                callback(child);

                child.ForeachChild(callback);
            }
        }

        /// <summary>
        /// 遍历每个子节点及自身
        /// </summary>
        /// <param name="transform">实例</param>
        /// <param name="callback">返回子节点和自身</param>
        public static void ForeachChildWithSelf(this Transform transform, Action<Transform> callback)
        {
            callback(transform);

            foreach (Transform child in transform)
            {
                child.ForeachChild(callback);
            }
        }
    }
}