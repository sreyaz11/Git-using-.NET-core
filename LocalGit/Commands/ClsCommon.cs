
namespace LocalGit.Commands
{
    public class ClsCommon
    {
        public static Dictionary<string, string> ReadIndex()
        {
            var dict = new Dictionary<string, string>();

            if (!File.Exists(".LocalGit/index"))
            {
                Console.WriteLine("Index file not found");
                return dict;
            }

            foreach (var line in File.ReadAllLines(".LocalGit/index"))
            {
                string[] parts = line.Split('|');
                if (parts.Length == 2)
                    dict[parts[0]] = parts[1];
            }
            return dict;
        }

        public static bool ClearIndex()
        {
            try
            {                                                                                         //add later
                var dict = new Dictionary<string, string>();                                          //file locking                      
                                                                                                      //concurrent commands
                if (!File.Exists(".LocalGit/index"))                                                  //transactional safety
                {
                    Console.WriteLine("Index file not found");
                }

                File.WriteAllText(".LocalGit/index", "");
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception in ClearIndex");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public static string ReadHead()
        {
            try
            {
                if (!File.Exists(".LocalGit/HEAD"))
                {
                    Console.WriteLine("HEAD not found.");
                    return null;
                }
                string head = File.ReadAllText(".LocalGit/HEAD");
                return head;

            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception in ReadHead");
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
