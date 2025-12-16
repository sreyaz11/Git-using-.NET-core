using LocalGit.Core;

namespace LocalGit.Commands
{
    public class AddCommand
    {
        public static bool Execute(string fileName)
        {
            try
            {
                fileName = Path.GetRelativePath(Directory.GetCurrentDirectory(), Path.GetFullPath(fileName));

                if (!File.Exists(fileName))
                {
                    Console.WriteLine("File not found");
                }

                byte[] context = File.ReadAllBytes(fileName);
                string hash = Hasher.Hash(context);

                File.WriteAllBytes($".LocalGit/objects/{hash}", context);
                File.AppendAllText(".LocalGit/index", $"{fileName}|{hash}\n");

                Console.WriteLine($"Added {fileName}");
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine("AddCommand Execute exception");
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
