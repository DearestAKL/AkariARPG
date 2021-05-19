using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Akari
{
    public class Particle : EntityLogic, IPause
    {
        private ParticleSystem ps;
        private FollowerData followerData;

        protected bool pause = false;
        private float pauseTime;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            ps = GetComponentInChildren<ParticleSystem>();

        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            followerData = userData as FollowerData;
            if (followerData == null)
            {
                return;
            }

            ps.Play(true);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (pause)
                return;

            if (followerData != null && followerData.Follow != null)
            {
                transform.position = followerData.Follow.position + followerData.Offset;
                transform.rotation = Quaternion.Euler(followerData.Follow.eulerAngles + followerData.Angles);
            }

            if(followerData.LifeTime > 0 && ps.time > followerData.LifeTime)
            {
                GameEntry.Entity.HideEntity(this.Entity);
            }
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            followerData = null;
            ps.Stop(true);
        }

        public void Pause()
        {
            pause = true;
            ps.Pause(true);
            pauseTime = ps.time;
        }

        public void Resume()
        {
            pause = false;
            ps.Play();
            ps.time = pauseTime;
        }
    }
}
