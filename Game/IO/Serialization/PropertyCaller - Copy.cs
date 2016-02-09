//using IO;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;

//namespace IO.Serialization
//{
//    /// <summary>
//    /// Provides a fast method of getting and setting object properties. 
//    /// </summary>
//    /// <typeparam name="TThis">The type of the this.</typeparam>
//    class PropertyCaller<TThis>
//    {
//        static readonly Dictionary<string, IPropertyCallAdapter<TThis>> instances = new Dictionary<string, IPropertyCallAdapter<TThis>>();

//        static IPropertyCallAdapter<TThis> createAdapter(PropertyInfo property)
//        {
//            if (property == null) throw new ArgumentNullException(nameof(property));

//            // Getter
//            var getMethod = property.GetGetMethod(true);
//            if (getMethod == null)
//                throw new Exception("Unable to find a getter for property `{0}` of type `{1}`".F(property?.Name, typeof(TThis).Name));

//            var openGetterType = typeof(Func<,>);
//            var concreteGetterType = openGetterType.MakeGenericType(typeof(TThis), property.PropertyType);
//            var getterInvocation =
//                Delegate.CreateDelegate(concreteGetterType, null, getMethod);

//            // Setter
//            var setMethod = property.GetSetMethod(true);
//            if (setMethod == null)
//                throw new Exception("Unable to find a setter for property `{0}` of type `{1}`".F(property?.Name, typeof(TThis).Name));

//            var openSetterType = typeof(Action<,>);
//            var concreteSetterType = openSetterType.MakeGenericType(typeof(TThis), property.PropertyType);
//            var setterInvocation =
//                Delegate.CreateDelegate(concreteSetterType, null, setMethod);

//            // Adapter
//            var openAdapterType = typeof(PropertyCallAdapter<,>);
//            var concreteAdapterType = openAdapterType
//                .MakeGenericType(typeof(TThis), property.PropertyType);
//            var adapter = (IPropertyCallAdapter<TThis>)Activator.CreateInstance(concreteAdapterType, getterInvocation, setterInvocation);

//            return adapter;
//        }

//        public static IPropertyCallAdapter<TThis> GetInstance(string forPropertyName)
//        {
//            IPropertyCallAdapter<TThis> instance;
//            if (!instances.TryGetValue(forPropertyName, out instance))
//            {
//                var property = typeof(TThis).GetProperty(
//                    forPropertyName,
//                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

//                instance = createAdapter(property);
//                instances.Add(forPropertyName, instance);
//            }

//            return instance;
//        }
//    }
//    interface IPropertyCallAdapter<TThis>
//    {
//        object InvokeGet(TThis @this);

//        void InvokeSet(TThis @this, object value);
//    }

//    public class PropertyCallAdapter<TThis, TResult> : IPropertyCallAdapter<TThis>
//    {
//        readonly Func<TThis, TResult> _getterInvocation;
//        readonly Action<TThis, TResult> _setterInvocation;

//        public PropertyCallAdapter(
//            Func<TThis, TResult> getterInvocation,
//            Action<TThis, TResult> setterInvocation)
//        {
//            _getterInvocation = getterInvocation;
//            _setterInvocation = setterInvocation;
//        }

//        public object InvokeGet(TThis @this)
//        {
//            return _getterInvocation.Invoke(@this);
//        }

//        public void InvokeSet(TThis @this, object value)
//        {
//            _setterInvocation.Invoke(@this, (TResult)value);
//        }
//    }
//}
