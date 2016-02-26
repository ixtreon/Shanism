using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IxSerializer
{

    public class TypeStore
    {
        static readonly Dictionary<Type, Dictionary<string, IPropertyCallAdapter>> typeAdapterDict = new Dictionary<Type, Dictionary<string, IPropertyCallAdapter>>();

        static readonly Dictionary<Type, PropertyDescriptor[]> typeProperties = new Dictionary<Type, PropertyDescriptor[]>();

        static IPropertyCallAdapter getCallAdapter(Type baseType, string forPropertyName)
        {
            //get the dict for this type
            Dictionary<string, IPropertyCallAdapter> callAdapters;
            if (!typeAdapterDict.TryGetValue(baseType, out callAdapters))
                typeAdapterDict[baseType] = callAdapters = new Dictionary<string, IPropertyCallAdapter>();

            //get the instance for this type
            IPropertyCallAdapter instance;
            if (!callAdapters.TryGetValue(forPropertyName, out instance))
                callAdapters[forPropertyName] = instance = getAdapter(baseType, forPropertyName);

            return instance;
        }

        public static object GetValue(object obj, string propertyName)
        {
            var adapter = getCallAdapter(obj.GetType(), propertyName);
            return adapter.InvokeGet(obj);
        }

        public static void SetValue(object obj, string propertyName, object val)
        {
            var adapter = getCallAdapter(obj.GetType(), propertyName);
            adapter.InvokeSet(obj, val);
        }

        /// <summary>
        /// Gets all properties of the provided class or interface. 
        /// Automatically recurses into the interface tree.  
        /// </summary>
        public static PropertyDescriptor[] GetProperties(Type ty)
        {
            PropertyDescriptor[] val;
            if (!typeProperties.TryGetValue(ty, out val))
            {
                typeProperties[ty] =
                val =
                    recurseProperties(ty)       // fetch all properties
                    .GroupBy(n => n.Name)
                    .Select(g => g.First())     // get distinct ones
                    .ToArray();
            }
            return val;
        }

        //recursively gets all public properties defined by ty and its interfaces
        static IEnumerable<PropertyDescriptor> recurseProperties(Type ty)
        {
            return ty.GetProperties()
                .Select(pi => new PropertyDescriptor(pi))
                .Concat(ty.GetInterfaces().SelectMany(tyInt => recurseProperties(tyInt)));
        }

        static IPropertyCallAdapter getAdapter(Type baseType, string propertyName)
        {
            var property = baseType.GetProperties()
                .Concat(baseType.BaseType.GetProperties())
                .Concat(baseType.GetInterfaces().SelectMany(i => i.GetProperties()))
                .Where(p => p.Name == propertyName)
                .First();

            //obtain Func for the getter
            var getMethod = property?.GetGetMethod(true);
            //if (getMethod == null) throw new ArgumentException(nameof(propertyName), "Property does not have a getter!");

            var concreteGetterType = typeof(Func<,>)
                .MakeGenericType(baseType, property.PropertyType);
            var getterInvocation = (getMethod != null) ? Delegate.CreateDelegate(concreteGetterType, null, getMethod) : null;

            //obtain Action for the setter
            var setMethod = property?.GetSetMethod(true);
            //if (setMethod == null) throw new ArgumentException(nameof(propertyName), "Property does not have a setter!");

            var concreteSetterType = typeof(Action<,>)
                .MakeGenericType(baseType, property.PropertyType);
            var setterInvocation = (setMethod != null) ? Delegate.CreateDelegate(concreteSetterType, null, setMethod) : null;

            //create the adapter
            var concreteAdapterType = typeof(PropertyCallAdapter<,>)
                .MakeGenericType(baseType, property.PropertyType);
            return (IPropertyCallAdapter)Activator.CreateInstance(concreteAdapterType, new[] { getterInvocation, setterInvocation });
        }
    }

    public struct PropertyDescriptor
    {
        public readonly string Name;
        public readonly Type Type;

        public PropertyDescriptor(string name, Type type) { Name = name;  Type = type; }
        public PropertyDescriptor(PropertyInfo pi) { Name = pi.Name;  Type = pi.PropertyType; }
    }

    public interface IPropertyCallAdapter
    {
        object InvokeGet(object obj);
        void InvokeSet(object obj, object val);
    }

    public class PropertyCallAdapter<TCall, TResult> : IPropertyCallAdapter
    {
        private readonly Func<TCall, TResult> getterFunc;
        private readonly Action<TCall, TResult> setterFunc;

        public PropertyCallAdapter(Func<TCall, TResult> getterFunc, Action<TCall, TResult> setterFunc)
        {
            this.getterFunc = getterFunc;
            this.setterFunc = setterFunc;
        }

        public object InvokeGet(object obj)
        {
            return getterFunc.Invoke((TCall)obj);
        }

        public void InvokeSet(object obj, object val)
        {
            setterFunc.Invoke((TCall)obj, (TResult)val);
        }
    }

}
