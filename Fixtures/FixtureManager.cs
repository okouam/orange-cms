using System.IO;
using System.Linq;
using System.Reflection;

namespace Fixtures
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
            using (var input = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
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
