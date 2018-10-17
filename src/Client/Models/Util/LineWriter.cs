using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Shanism.Client.Models.Util
{
    class LineWriter : TextWriter
    {
        const int BufferSize = 255;

        public LinkedList<string> Lines { get; } = new LinkedList<string>();
        public StringBuilder CurrentLine { get; } = new StringBuilder(80);

        public override Encoding Encoding => Encoding.Default;


        void newLine()
        {
            var l = CurrentLine.ToString();
            Lines.AddLast(l);

            CurrentLine.Clear();

            if(Lines.Count > BufferSize)
                Lines.RemoveFirst();
        }

        public override void Write(char value)
            => CurrentLine.Append(value);

        public override void Write(string value)
            => CurrentLine.Append(value);


        public override void WriteLine()
            => newLine();

        public override void WriteLine(char value)
        {
            Write(value);
            newLine();
        }

        public override void WriteLine(string value)
        {
            Write(value);
            newLine();
        }
    }
}
