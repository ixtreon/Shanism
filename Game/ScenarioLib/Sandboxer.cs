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
        /* static, 'cause only 1 scenario should ever be loaded */
        static AppDomain Sandbox;

        //TODO: gotta sign the exe...
        static readonly SecurityPermissionFlag[] scenarioPermissions =
        {
            SecurityPermissionFlag.Execution,
        };

        public const string OutputDirectory = ScenarioCompiler.OutputDirectory;


        public static void Init(string outputDir = null)
        {
            outputDir = outputDir ?? OutputDirectory;

            //AppDomain setup
            var adSetup = new AppDomainSetup();
            adSetup.ApplicationBase = Path.GetFullPath(outputDir);

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

            Sandboxer newDomainInstance = (Sandboxer)handle.Unwrap();

            return newDomainInstance;
        }

        public Assembly LoadAssembly(byte[] assemblyBytes, byte[] symbolBytes)
        {
            return Assembly.Load(assemblyBytes, symbolBytes);
        }
    }
}
