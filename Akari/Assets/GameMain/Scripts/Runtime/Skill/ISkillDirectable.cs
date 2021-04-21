using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akari
{
    public interface ISkillDirectable
    {
        string name { get; }
        float startTime { get; }
        float endTime { get; }

        bool isActive { get; }
    }


}
