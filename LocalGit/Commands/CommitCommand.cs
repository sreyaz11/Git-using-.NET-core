using LocalGit.Commands.Model;
using System.Net.Http.Headers;
using System.Text.Json;

namespace LocalGit.Commands
{
    public class CommitCommand
    {
        public static bool execute(string message)
        {
            try
            {
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
                    Console.WriteLine($"{index.Count} files changed");
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
    }
}
