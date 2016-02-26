using IO.Objects;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member


namespace IO.Objects
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    public class DoodadStub : EntityStub, IDoodad
    {
        public DoodadStub() { }

        public DoodadStub(uint guid)
            : base(guid)
        {

        }
    }
}
