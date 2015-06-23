using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.Common
{
    /// <summary>
    /// Unused
    /// </summary>
    public static class UnitStateExt
    {

        static UnitStateAttribute GetInfo(this UnitState state)
        {
            return (UnitStateAttribute)typeof(UnitState)
                .GetMember(state.ToString())
                .First()    //should always have this member
                .GetCustomAttributes(typeof(UnitStateAttribute), false)
                .FirstOrDefault();
        }

        public static bool IsStacking(this UnitState state)
        {
            return state.GetInfo().IsStacking;
        }
    }


    /// <summary>
    /// Unused
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    sealed class UnitStateAttribute : Attribute
    {
        public readonly bool IsStacking;

        public UnitStateAttribute(bool isStacking)
        {
            this.IsStacking = isStacking;
        }

    }
}
