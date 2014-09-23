using System.IO;
using System.Reflection;
using CodeKinden.OrangeCMS.Domain;

namespace CodeKinden.OrangeCMS.Fixtures
{
    public class FixtureManager
    {
        public static string Extract(string resourceName)
        {
            var tempFileName = Path.GetTempFileName();
            WriteResourceToFile(resourceName, tempFileName);
            return tempFileName;
        }

        public static void WriteResourceToFile(string resourceName, string fileName)
        {
            const int bufferSize = 4096;
            var buffer = new byte[bufferSize];
            var assembly = Assembly.GetExecutingAssembly();
            using (var input = assembly.GetManifestResourceStream(resourceName))
            {
                if (input == null)
                {
                    throw new RuntimeException("The resource named '{0}' could not be found in the assembly '{1}.", resourceName, Assembly.GetExecutingAssembly().FullName);
                }

                using (Stream output = new FileStream(fileName, FileMode.Create))
                {
                    var byteCount = input.Read(buffer, 0, bufferSize);
                    while (byteCount > 0)
                    {
                        output.Write(buffer, 0, byteCount);
                        byteCount = input.Read(buffer, 0, bufferSize);
                    }
                }
            }
        }
    }
}
