using Akari;
using GameFramework.DataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[ActionConfig(typeof(PlayEffect))]
public class PlayEffectConfig : HoldFrames
{
    public EnumEntity effectType = EnumEntity.None;
    public Vector3 Offset = Vector3.zero;
    public Vector3 Angles = Vector3.zero;
    public Vector3 Scale = Vector3.one;
    public float lifeTime = 0f;
}

public class PlayEffect : IActionHandler
{
    private int m_EffectSerialId = 0;

    public void Enter(ActionNode node)
    {
        Debug.Log("PlayEffect Enter ====================================");

        PlayEffectConfig config = (PlayEffectConfig)node.config;
        TargetableObject controller = (TargetableObject)node.actionMachine.controller;

        m_EffectSerialId = GameEntry.Entity.GenerateSerialId();

        var data = new FollowerData(m_EffectSerialId, (int)config.effectType, controller.CachedTransform, config.Offset, config.Angles, config.Scale, config.lifeTime);

        GameEntry.Entity.ShowEntity<Particle>("Effect", Constant.AssetPriority.EffectAsset, data);

        //string assetName = AssetUtility.GetEffectAsset(drEffect.AssetName);
    }

    public void Exit(ActionNode node)
    {
        Debug.Log("PlayEffect Exit ====================================");
        //GameEntry.Entity.HideEntity(m_EffectSerialId);
    }

    public void Update(ActionNode node, float deltaTime)
    {

    }
}

