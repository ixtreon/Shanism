using Engine.Objects;
using Engine.Objects.Game;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ShanoRpgWin
{
    static class LocalHeroes
    {
        const string HeroDir = "Heroes";

        readonly static Dictionary<Hero, string> heroes = new Dictionary<Hero, string>();

        public static IEnumerable<Hero> Heroes
        {
            get { return heroes.Keys; }
        }

        static LocalHeroes()
        {
            if (!Directory.Exists(HeroDir))
                Directory.CreateDirectory(HeroDir);
        }

        public static void LoadHeroes()
        {
            heroes.Clear();
            foreach(var fn in Directory.EnumerateFiles(HeroDir, "*.hero"))
            {
                try
                {
                    var h = Hero.Load(fn);
                    heroes.Add(h, fn);
                }
                catch
                {
                    //Console.WriteLine("Error reading hero {1}", Path.GetFileNameWithoutExtension(fn));
                }
            }
        }

        public static void Save(this Hero h)
        {
            var saveDir = Path.Combine(HeroDir, h.Name + ".hero");
            h.Save(saveDir);
            if(!heroes.ContainsKey(h))
            {
                Console.WriteLine("Warning: Saving a newly created hero!");
                heroes.Add(h, saveDir);
            }
        }

        public static string GetDirectory(this Hero h)
        {
            return heroes[h];
        }

    }
}
