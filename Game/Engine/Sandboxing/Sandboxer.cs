using Shanism.Common;
using Shanism.Common.Interfaces.Objects;
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

namespace Shanism.Engine
{
    class Sandboxer : MarshalByRefObject
    {
        public const string DefaultOutputDirectory = "/";

        static readonly SecurityPermissionFlag[] scenarioPermissions =
        {
            SecurityPermissionFlag.Execution,
        };


        /* static, 'cause only 1 scenario should ever be loaded */
        static AppDomain newDomain;

        public static void Init(string outputDir = DefaultOutputDirectory)
        {
            outputDir = Path.GetFullPath(outputDir);

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
            var fullTrustAssembly = typeof(Sandboxer).Assembly.Evidence.GetHostEvidence<StrongName>();

            //finally, the AppDomain
            newDomain = AppDomain.CreateDomain("ShanoSandbox", null, adSetup, permissions, fullTrustAssembly);
        }

        public static Sandboxer Create()
        {
            // http://msdn.microsoft.com/en-us/library/dd413384(v=vs.110).aspx
            var handle = Activator.CreateInstanceFrom(newDomain,
                typeof(Sandboxer).Assembly.ManifestModule.FullyQualifiedName,
                typeof(Sandboxer).FullName );

            var sandboxGuy = (Sandboxer)handle.Unwrap();
            return sandboxGuy;
        }

        public IEnumerable<IGameObject> LoadAssembly(AssemblyName name)
        {
            var dom = AppDomain.CurrentDomain;

            System.Diagnostics.Debug.Assert(!dom.IsFullyTrusted);
            var ass = dom.Load(name);


            var engine = new ShanoEngine();
            string compileErrors;
            if (!engine.TryLoadScenario("sad", 123, out compileErrors))
                throw new Exception(compileErrors);

            var dtys = ass.DefinedTypes;
            var tys = ass.GetTypesDescending<IGameObject>()
                .Select(t => (IGameObject)Activator.CreateInstance(t))
                .ToList();

            return tys;
        }
    }
}
