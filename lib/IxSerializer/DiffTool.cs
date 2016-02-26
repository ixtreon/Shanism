using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IxSerializer
{
    /// <summary>
    /// Does binary diffs on items. 
    /// Max 255 diffs per object. 
    /// Max 65536 distance between diffs. 
    /// Max 65536 deletions and/or adds. 
    /// </summary>
    public class DiffTool
    {


        public static bool Diff(BinaryWriter writer, byte[] oldObject, byte[] newObject)
        {
            var diffs = IxSerializer.Diff.DiffBytes(oldObject, newObject);

            //write # changes
            writer.Write((byte)diffs.Length);

            var curPos = 0;
            foreach(var i in Enumerable.Range(0, diffs.Length))
            {
                //get the current diff
                var diff = diffs[i];

                //calc how much we moved in b
                var toMove = diff.StartB - curPos;

                //write it down, move to that place
                writer.Write((ushort)toMove);
                curPos += toMove;

                //write the # deletions
                writer.Write((ushort)(diff.deletedA));

                // write # insertions, the insertions
                // pretend we are in b
                writer.Write((ushort)(diff.insertedB));
                writer.Write(newObject, curPos, diff.insertedB);
                curPos += diff.insertedB;
            }

            return diffs.Any();
        }

        static byte[] patchBuffer = new byte[65536];

        public static byte[] Patch(BinaryReader reader, byte[] obj)
        {
            var srcPos = 0;

            using (var ms = new MemoryStream())
            {
                var nChanges = reader.ReadByte();
                using (var writer = new BinaryWriter(ms))
                {
                    foreach (var i in Enumerable.Range(0, nChanges))
                    {
                        var nForward = reader.ReadUInt16();
                        var nSkip = reader.ReadUInt16();
                        var nAdd = reader.ReadUInt16();

                        //copy from ye olde src
                        writer.Write(obj, srcPos, nForward);
                        srcPos += nForward;

                        //skip in zhe src
                        srcPos += nSkip;

                        //append to zhe new
                        reader.Read(patchBuffer, 0, nAdd);
                        writer.Write(patchBuffer, 0, nAdd);
                    }
                }

                return ms.ToArray();
            }
        }

    }
}
