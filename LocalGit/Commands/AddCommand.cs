using LocalGit.Core;

namespace LocalGit.Commands
{
    public class AddCommand
    {
        public static bool Execute(string[] args)
        {
            try
            {
                string fileName = string.Empty;
                for (int i = 1; i < args.Length; i++)
                {
                    fileName = args[i];

                    fileName = Path.GetRelativePath(Directory.GetCurrentDirectory(), Path.GetFullPath(fileName));

                    if (!File.Exists(fileName))
                    {
                        Console.WriteLine("File not found");
                    }

                    byte[] context = File.ReadAllBytes(fileName);
                    string hash = Hasher.Hash(context);

                    Dictionary<string, string> index = ClsCommon.ReadIndex();

                    if (!index.ContainsKey(fileName))
                    {
                        File.WriteAllBytes($".LocalGit/objects/{hash}", context);
                        File.AppendAllText(".LocalGit/index", $"{fileName}|{hash}\n");
                        Console.WriteLine($"Added {fileName}");
                    }
                    else if (index.ContainsKey(fileName) && index[fileName].Equals(hash))
                    {
                        Console.WriteLine("File already staged");
                    }
                    else if (index.ContainsKey(fileName) && !index[fileName].Equals(hash))
                    {
                        File.WriteAllBytes($".LocalGit/objects/{hash}", context);
                        File.AppendAllText(".LocalGit/index", $"{fileName}|{hash}\n");
                    }
                }
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
