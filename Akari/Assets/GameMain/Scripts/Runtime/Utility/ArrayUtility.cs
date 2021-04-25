using GameFramework;
using System;

namespace Akari
{
    /// <summary>
    /// 数组工具
    /// </summary>
    public class ArrayUtility
    {
        /// <summary>
        /// 移除指定下标元素，可获取移除元素 与 移除元素后的数组
        /// </summary>
        public static T RemoveAt<T>(ref T[] source, int index, T defaultValue = default)
        {
            if (null == source || 0 >= source.Length || source.Length <= index)
            {
                return defaultValue;
            }

            T[] splites = Splite(ref source, index, 1);
            return 0 < splites.Length ? splites[0] : defaultValue;
        }

        /// <summary>
        /// 分割，得到新的数组
        /// </summary>
        public static T[] Splite<T>(ref T[] source, int startIndex, int length)
        {
            int beginIndex = startIndex + length;
            if (null == source || source.Length < beginIndex)
            {
                throw new GameFrameworkException($"参数不在有效范围内>数组长度:{source?.Length},起始下标:{startIndex},长度:{length}");
            }

            T[] results = new T[length];
            Array.Copy(source, startIndex, results, 0, length);
            Array.Copy(source, beginIndex, source, startIndex, source.Length - beginIndex);
            Array.Resize(ref source, source.Length - length);
            return results;
        }

        /// <summary>
        /// 合并，将数组按顺序合并，得到新数组
        /// </summary>
        public static T[] Combine<T>(params T[][] sources)
        {
            int length = 0;
            int cnt = sources.Length;

            for (int i = 0; i < cnt; i++)
            {
                length += sources[i].Length;
            }

            T[] results = new T[length];

            int startIndex = 0;
            for (int i = 0; i < cnt; i++)
            {
                int subLength = sources[i].Length;
                Array.Copy(sources[i], 0, results, startIndex, subLength);
                startIndex += subLength;
            }

            return results;
        }

        /// <summary>
        /// 附加，附加到目标数组末尾，得到新数组
        /// </summary>
        public static T[] Append<T>(T[] source, params T[] items)
        {
            return Combine(source, items);
        }
    }
}
