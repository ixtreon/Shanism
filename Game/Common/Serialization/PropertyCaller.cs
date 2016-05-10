using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common.Serialization
{
    /// <summary>
    /// Provides a fast method of getting and setting object properties. 
    /// </summary>
    public static class PropertyCaller
    {
        static readonly Dictionary<Tuple<Type, string>, IPropertyCallAdapter> instances = new Dictionary<Tuple<Type, string>, IPropertyCallAdapter>();

        static IPropertyCallAdapter createAdapter(Type ty, PropertyInfo property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));

            // Getter
            var getMethod = property.GetGetMethod(true) ?? getTypeMethod(ty, "get_" + property.Name);
            Delegate getterInvocation = null;
            if (getMethod != null)
            {
                var getterType = typeof(Func<,>).MakeGenericType(ty, property.PropertyType);
                getterInvocation = Delegate.CreateDelegate(getterType, null, getMethod);
            }

            // Setter
            var setMethod = property.GetSetMethod(true) ?? getTypeMethod(ty, "set_" + property.Name);
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
            var adapter = (IPropertyCallAdapter)Activator.CreateInstance(concreteAdapterType, 
                property.Name, property.PropertyType,
                getterInvocation, setterInvocation);

            return adapter;
        }

        static MethodInfo getTypeMethod(Type declaringType, string name)
        {
            MethodInfo setMethod = null;
            while ((setMethod = declaringType.GetMethod(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)) == null
                && declaringType.BaseType != null)
                declaringType = declaringType.BaseType;
            return setMethod;
        }

        public static IPropertyCallAdapter GetInstance(Type declaringType, string propName)
        {
            var key = new Tuple<Type, string>(declaringType, propName);
            IPropertyCallAdapter instance;
            if (!instances.TryGetValue(key, out instance))
            {
                var property = declaringType.GetProperty(
                    propName,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                instance = createAdapter(declaringType, property);
                instances.Add(key, instance);
            }

            return instance;
        }

        public static IPropertyCallAdapter GetInstance(PropertyInfo pi)
        {
            var ty = pi.DeclaringType;
            var key = new Tuple<Type, string>(ty, pi.Name);

            IPropertyCallAdapter instance;
            if (!instances.TryGetValue(key, out instance))
            {
                instance = createAdapter(ty, pi);
                instances.Add(key, instance);
            }

            return (IPropertyCallAdapter)instance;
        }


        public static IPropertyCallAdapter<T> GetInstance<T>(string propName)
        {
            var ty = typeof(T);
            var key = new Tuple<Type, string>(ty, propName);
            IPropertyCallAdapter instance;
            if (!instances.TryGetValue(key, out instance))
            {
                var property = ty.GetProperty(
                    propName,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                if (property != null)
                    instance = createAdapter(ty, property);

                instances.Add(key, instance);
            }

            return (IPropertyCallAdapter<T>)instance;
        }
    }

    /// <summary>
    /// Allows calling a specified property of some type. 
    /// </summary>
    public interface IPropertyCallAdapter
    {
        object InvokeGet(object @this);

        void InvokeSet(object @this, object value);

        string PropertyName { get; }

        Type PropertyType { get; }

        bool CanRead { get; }

        bool CanWrite { get; }
    }

    /// <summary>
    /// Allows calling a specified property of the given type. 
    /// </summary>
    /// <typeparam name="TThis"></typeparam>
    public interface IPropertyCallAdapter<TThis> : IPropertyCallAdapter
    {
        object InvokeGet(TThis @this);

        void InvokeSet(TThis @this, object value);
    }

    public class PropertyCallAdapter<TThis, TResult> : IPropertyCallAdapter, IPropertyCallAdapter<TThis>
    {
        public string PropertyName { get; }
        public Type PropertyType { get; }

        readonly Func<TThis, TResult> _getterInvocation;
        readonly Action<TThis, TResult> _setterInvocation;

        public bool CanRead {  get { return _getterInvocation != null; } }
        public bool CanWrite {  get { return _setterInvocation != null; } }

        public PropertyCallAdapter(string propertyName,
            Type propertyType,
            Func<TThis, TResult> getterInvocation,
            Action<TThis, TResult> setterInvocation)
        {
            PropertyName = propertyName;
            PropertyType = propertyType;

            _getterInvocation = getterInvocation;
            _setterInvocation = setterInvocation;
        }

        public object InvokeGet(TThis @this)
        {
            return _getterInvocation.Invoke(@this);
        }

        public object InvokeGet(object @this)
        {
            return _getterInvocation.Invoke((TThis)@this);
        }

        public void InvokeSet(TThis @this, object value)
        {
            _setterInvocation.Invoke(@this, (TResult)value);
        }

        public void InvokeSet(object @this, object value)
        {
            _setterInvocation.Invoke((TThis)@this, (TResult)value);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => $"{typeof(TResult)} {PropertyName}";
    }
}
