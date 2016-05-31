using Shanism.Common;
using Shanism.Common.Interfaces.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.ScenarioLib
{
    class Sandboxer : MarshalByRefObject
    {

        //TODO: gotta sign the exe...
        static readonly SecurityPermissionFlag[] scenarioPermissions =
        {
            SecurityPermissionFlag.Execution,
        };

        public const string OutputDirectory = ScenarioCompiler.OutputDirectory;

        /* static, 'cause only 1 scenario should ever be loaded */
        static AppDomain Sandbox;

        public static void Init(string outputDir = null)
        {
            outputDir = Path.GetFullPath(outputDir ?? OutputDirectory);

            //AppDomain setup
            var adSetup = new AppDomainSetup
            {
                ApplicationBase = outputDir,
                PrivateBinPath = "/",
            };

            //Permission set
            var permissions = new PermissionSet(PermissionState.None);
            foreach (var p in scenarioPermissions)
                permissions.AddPermission(new SecurityPermission(p));

            //Strong names of trusted assemblies
            var fullTrustAssembly = typeof(ScenarioCompiler).Assembly.Evidence.GetHostEvidence<StrongName>();

            //finally, the AppDomain
            Sandbox = AppDomain.CreateDomain("ShanoSandbox", null, adSetup, permissions, fullTrustAssembly);
        }

        public static Sandboxer Create()
        {
            // http://msdn.microsoft.com/en-us/library/dd413384(v=vs.110).aspx
            var handle = Activator.CreateInstanceFrom(Sandbox,
                typeof(Sandboxer).Assembly.ManifestModule.FullyQualifiedName,
                typeof(Sandboxer).FullName );

            var sandboxGuy = (Sandboxer)handle.Unwrap();

            //var sandboxGuy2 = (Sandboxer)Sandbox.CreateInstanceAndUnwrap(typeof(Sandboxer).Assembly.ManifestModule.FullyQualifiedName, typeof(Sandboxer).FullName, null);

            return sandboxGuy;
        }

        public IEnumerable<IGameObject> LoadAssembly(byte[] assemblyBytes, byte[] symbolBytes)
        {
            var dom = AppDomain.CurrentDomain;

            System.Diagnostics.Debug.Assert(!dom.IsFullyTrusted);
            var ass = dom.Load(assemblyBytes, symbolBytes);

            var brd = ass.GetType("DefaultScenario.Buffs.Haste", true, true);

            var dtys = ass.DefinedTypes;

            var tys = ass.GetTypesDescending<IGameObject>()
                .Select(t => (IGameObject)Activator.CreateInstance(t))
                .ToList();

            return tys;
        }

        //public static Assembly Load(byte[] assemblyBytes, byte[] symbolBytes)
        //{
        //    var sp = Sandbox.SetupInformation.PrivateBinPathProbe;
        //    var ap = AppDomain.CurrentDomain.RelativeSearchPath;

        //    Init();
        //    var a = Sandbox.Load(assemblyBytes, symbolBytes);

        //    return a;
        //}
    }
}
