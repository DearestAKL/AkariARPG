using UnityEngine;
using GameFramework;
using GameFramework.Fsm;
using ActionOwner = GameFramework.Fsm.IFsm<Akari.Hero>;

namespace Akari
{
    [System.Serializable]
    [ActionConfig(typeof(Move))]
    public class MoveConfig
    {
        public float moveSpeed;
    }

    public class Move : FsmState<Hero>, IReference
    {
        private ActionNode node;
        private MoveConfig config;
        private IActionMachine machine;

        private PlayerComponent player;
        private InputComponent input;
        private Rigidbody rigidbody;

        protected override void OnInit(ActionOwner actionOwner)
        {
            base.OnInit(actionOwner);

            player = GameEntry.Player;
            input = GameEntry.Input;
        }

        protected override void OnEnter(ActionOwner actionOwner)
        {
            base.OnEnter(actionOwner);

            node = actionOwner as ActionNode;
            config = node.config as MoveConfig;
            machine = node.actionMachine;

            rigidbody = player.HeroRigidbody;
        }

        protected override void OnUpdate(ActionOwner actionOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(actionOwner, elapseSeconds, realElapseSeconds);

            if (input.HasEvent(InputEvents.Moving))
            {
                var velocity = rigidbody.velocity;
                var move = input.AxisValue.normalized * config.moveSpeed;

                velocity.x = move.x;
                velocity.z = move.y;

                rigidbody.velocity = velocity;
            }
        }

        protected override void OnLeave(ActionOwner actionOwner, bool isShutdown)
        {
            base.OnLeave(actionOwner, isShutdown);
        }

        protected override void OnDestroy(ActionOwner actionOwner)
        {
            base.OnDestroy(actionOwner);
        }

        public void Clear()
        {
            throw new System.NotImplementedException();
        }
    }
}
