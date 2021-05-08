//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using UnityEngine;

namespace Akari
{
    public static partial class Constant
    {
        /// <summary>
        /// 层。
        /// </summary>
        public static class Layer
        {
            public const string DefaultLayerName = "Default";
            public static readonly int DefaultLayerId = LayerMask.NameToLayer(DefaultLayerName);

            public const string UILayerName = "UI";
            public static readonly int UILayerId = LayerMask.NameToLayer(UILayerName);

            public const string GroundLayerName = "Ground";
            public static readonly int GroundLayerId = LayerMask.NameToLayer(GroundLayerName);

            public const string HeroAttackBoxLayerName = "HeroAttackBox";
            public static readonly int HeroAttackBoxLayerId = LayerMask.NameToLayer(HeroAttackBoxLayerName);

            public const string MonsterAttackBoxLayerName = "MonsterAttackBox";
            public static readonly int MonsterAttackBoxLayerId = LayerMask.NameToLayer(MonsterAttackBoxLayerName);

            public const string HeroLayerName = "Hero";
            public static readonly int HeroLayerId = LayerMask.NameToLayer(HeroLayerName);

            public const string MonsterLayerName = "Monster";
            public static readonly int MonsterLayerId = LayerMask.NameToLayer(MonsterLayerName);
        }
    }
}
