using System;

namespace Akari
{
    /// <summary>
    /// ActionConfigAttribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ActionConfigAttribute : Attribute
    {
        public Type handlerType { get; protected set; }

        public ActionConfigAttribute(Type handlerType)
        {
            this.handlerType = handlerType;
        }
    }
}
