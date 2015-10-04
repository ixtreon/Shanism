using IxSerializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.Objects.Serializers
{
    /// <summary>
    /// Registers extra modules with the IxSerializer. 
    /// 
    /// Should be called once at the start of each application. 
    /// </summary>
    public class SerializerModules
    {
        public static void Init()
        {
            Serializer.Initialize();
            Serializer.AddModule(new GameObjectSerializer());
        }
    }
}
