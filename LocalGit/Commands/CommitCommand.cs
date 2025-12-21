using LocalGit.Commands.Model;
using System.Security.Cryptography;
using System.Text.Json;

namespace LocalGit.Commands
{
    public class CommitCommand
    {
        public static bool execute(string message)
        {
            try
            {
                int indexCount = 0;
                string CommitId = CreateCommitId();


                if (DoesCommitFileExists(CommitId))
                {
                    CommitId = CreateCommitId();
                }
                Dictionary<string, string> index = ClsCommon.ReadIndex();
                string head = ClsCommon.ReadHead();
                if(index.Count <= 0 )
                {
                    Console.WriteLine("Nothing to commit.");
                    return false;
                }
                indexCount = index.Count;
                if (!string.IsNullOrWhiteSpace(head))
                {
                    Dictionary<string, string> HeadCommitObjects = GetHeadCommitObjects(head);

                    //Console.WriteLine("Adding head commit");
                    foreach (string key in HeadCommitObjects.Keys)
                    {
                        if(!index.ContainsKey(key))
                            index.Add(key, HeadCommitObjects[key]);
                    }
                }
                
                CommitModel commit = new CommitModel();
                commit.CommitId = CommitId;
                commit.Timestamp = DateTime.Now;
                commit.Message = message;
                commit.ParentCommitId = head;
                commit.Snapshot = index;

                string commitInJson = JsonSerializer.Serialize(commit);

                File.WriteAllText($".LocalGit/commits/{CommitId}", commitInJson);
                File.WriteAllText(".LocalGit/HEAD", CommitId);

                bool IsIndexCleared = ClsCommon.ClearIndex();

                if (IsIndexCleared)
                {
                    Console.WriteLine($"{indexCount} files changed");
                    return true;
                }
                return false;
            }

            catch (Exception ex)
            {
                Console.WriteLine("Exception in CommitCommand execute");
                Console.WriteLine(ex.ToString());
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public static bool DoesCommitFileExists(string commitId)
        {
            return File.Exists($".LocalGit/Commits/{commitId}");
        }

        public static string CreateCommitId()
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] stringChars = new char[8];
            Random random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            string finalString = new String(stringChars);
            return finalString;
        }

        internal static bool ValidateCommand(string[] args)
        {
            if (args[1].Equals("-m"))
            {
                if (string.IsNullOrWhiteSpace(args[2]))
                {
                    Console.WriteLine("Enter a commit message in double quotes.");
                    return false; ;
                }
            }
            return true;
        }

        public static Dictionary<string, string> GetHeadCommitObjects(string head)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            try
            {
                string HeadCommitJson = File.ReadAllText($".LocalGit/commits/{head}");
                CommitModel model = JsonSerializer.Deserialize<CommitModel>(HeadCommitJson);
                return model.Snapshot;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in GetHeadCommitObjects");
                Console.WriteLine(ex.ToString());
                Console.WriteLine(ex.Message);
                return dict;
            }
        }
    }
}
