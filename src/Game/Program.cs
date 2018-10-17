using System;
using System.Diagnostics;
using System.Numerics;

namespace Shanism.Client.Game
{
    class Program
    {
        static void Main(string[] args)
        {

            using (var game = new ShanismGame())
            {
                game.StartupMap = GetArg("-sc");

                // SetStartUpMap(game);

                game.Run();
            }


            string GetArg(string name)
            {
                var id = Array.IndexOf(args, name) + 1;
                if (0 < id && id < args.Length)
                    return args[id];
                return null;
            }
        }

        //[Conditional("DEBUG")]
        //static void SetStartUpMap(ShanismGame game) => game.StartupMap = "Mechanics Tests";

    }

    static class ProgramExt
    {
        public static string GetArgVal(this string[] args, string name)
        {
            var id = Array.IndexOf(args, name) + 1;
            if (0 < id && id < args.Length)
                return args[id];
            return null;
        }
    }
}

