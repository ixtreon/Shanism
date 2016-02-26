using IO.Common;
using IO.Objects;
using IO.Serialization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Network.Server
{
    /// <summary>
    /// Provides up-to-date, cached serialized versions of GameObjects. 
    /// </summary>
    class ObjectTracker
    {
        public static readonly ObjectTracker Default = new ObjectTracker();

        readonly ConcurrentDictionary<IEntity, ObjectData> objectDataDict = new ConcurrentDictionary<IEntity, ObjectData>();

        int _frame = 0;


        public void Update(int msElapsed)
        {
            _frame++;

            //send a game update frame
        }

        public byte[] GetBytes(IEntity obj)
        {

            using (var ms = new MemoryStream())
            {


            }
            var datas = objectDataDict.GetOrAdd(obj, new ObjectData());
            var curFrame = _frame;

            if(datas.Frame < curFrame)
            {
                //TODO: create the serialized datas
                using (var ms = new MemoryStream())
                {
                    //ProtoConverter.Default.Serialize(ms, obj);
                    datas.Data = ms.ToArray();
                }

                datas.Frame = curFrame;
            }

            return datas.Data;
        }


        struct ObjectData
        {
            public byte[] Data;
            public int Frame;
        }
    }
}
