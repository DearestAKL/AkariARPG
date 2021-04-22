using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Akari
{
    [Conditional("UNITY_EDITOR")]
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class ObjectTypesAttribute : Attribute
    {
        public Type[] types
        {
            get
            {
                HashSet<Type> list = new HashSet<Type>();
                if (baseType != null)
                {
#if UNITY_EDITOR
                    foreach (var item in UnityEditor.TypeCache.GetTypesDerivedFrom(baseType))
                    {
                        list.Add(item);
                    }
#endif
                }
                if (extraTypes != null)
                {
                    foreach (var item in extraTypes)
                    {
                        list.Add(item);
                    }
                }
                return list.ToArray();
            }
        }

        public virtual Type baseType { get; set; }
        public Type[] extraTypes { get; protected set; }

        public ObjectTypesAttribute(params Type[] extraTypes)
        {
            this.extraTypes = extraTypes;
        }
    }
}
