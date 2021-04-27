using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akari
{
    /// <summary>
    /// IActionHandler
    /// </summary>
    public interface IActionHandler
    {
        void Enter(ActionNode node);

        void Exit(ActionNode node);

        void Update(ActionNode node, float deltaTime);
    }
}
