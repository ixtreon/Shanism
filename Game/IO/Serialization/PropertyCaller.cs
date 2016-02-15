using IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IO.Serialization
{
    /// <summary>
    /// Provides a fast method of getting and setting object properties. 
    /// </summary>
    /// <typeparam name="TThis">The type of the this.</typeparam>
    public static class PropertyCaller
    {
        static readonly Dictionary<Tuple<Type, string>, IPropertyCallAdapter> instances = new Dictionary<Tuple<Type, string>, IPropertyCallAdapter>();

        static IPropertyCallAdapter createAdapter(Type ty, PropertyInfo property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));

            // Getter
            var getMethod = getTypeMethod(ty, "get_" + property.Name);
            Delegate getterInvocation = null;
            if (getMethod != null)
            {
                var getterType = typeof(Func<,>).MakeGenericType(ty, property.PropertyType);
                getterInvocation = Delegate.CreateDelegate(getterType, null, getMethod);
            }

            // Setter
            var setMethod = getTypeMethod(ty, "set_" + property.Name);
            Delegate setterInvocation = null;
            if (setMethod != null)
            {
                var setterType = typeof(Action<,>).MakeGenericType(ty, property.PropertyType);
                setterInvocation = Delegate.CreateDelegate(setterType, null, setMethod);
            }

            // Adapter
            var openAdapterType = typeof(PropertyCallAdapter<,>);
            var concreteAdapterType = openAdapterType
                .MakeGenericType(ty, property.PropertyType);
            var adapter = (IPropertyCallAdapter)Activator.CreateInstance(concreteAdapterType, getterInvocation, setterInvocation);

            return adapter;
        }

        static MethodInfo getTypeMethod(Type ty, string name)
        {
            MethodInfo setMethod = null;
            while ((setMethod = ty.GetMethod(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)) == null
                && ty.BaseType != null)
                ty = ty.BaseType;
            return setMethod;
        }

        public static IPropertyCallAdapter GetInstance(Type ty, string propName)
        {
            var key = new Tuple<Type, string>(ty, propName);
            IPropertyCallAdapter instance;
            if (!instances.TryGetValue(key, out instance))
            {
                var property = ty.GetProperty(
                    propName,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                instance = createAdapter(ty, property);
                instances.Add(key, instance);
            }

            return instance;
        }
    }


    public interface IPropertyCallAdapter
    {
        object InvokeGet(object @this);

        void InvokeSet(object @this, object value);
    }

    public class PropertyCallAdapter<TThis, TResult> : IPropertyCallAdapter
    {
        readonly Func<TThis, TResult> _getterInvocation;
        readonly Action<TThis, TResult> _setterInvocation;

        public PropertyCallAdapter(
            Func<TThis, TResult> getterInvocation,
            Action<TThis, TResult> setterInvocation)
        {
            _getterInvocation = getterInvocation;
            _setterInvocation = setterInvocation;
        }

        public object InvokeGet(object @this)
        {
            return _getterInvocation.Invoke((TThis)@this);
        }

        public void InvokeSet(object @this, object value)
        {
            _setterInvocation.Invoke((TThis)@this, (TResult)value);
        }
    }
}
