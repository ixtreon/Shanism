using System;
using System.IO;
using System.Text;

namespace Shanism.Client.Models.Util
{
    /// <summary>
    /// A badly written duplex text writer.
    /// Redirects everything to an underlying collection of text writers.
    /// </summary>
    class DuplexWriter : TextWriter
    {
        readonly TextWriter[] writers;

        public DuplexWriter(params TextWriter[] writers)
        {
            this.writers = writers;
        }

        public override Encoding Encoding => Encoding.Default;

        public override void Write(char value) => ForEach(w => w.Write(value));
        public override void Write(string value) => ForEach(w => w.Write(value));
        public override void Write(char[] buffer) => ForEach(w => w.Write(buffer));

        public override void WriteLine() => ForEach(w => w.WriteLine());
        public override void WriteLine(char value) => ForEach(w => w.WriteLine(value));
        public override void WriteLine(string value) => ForEach(w => w.WriteLine(value));
        public override void WriteLine(char[] buffer) => ForEach(w => w.WriteLine(buffer));


        void ForEach(Action<TextWriter> watDo)
        {
            for(int i = 0; i < writers.Length; i++)
                watDo(writers[i]);
        }
    }
}
