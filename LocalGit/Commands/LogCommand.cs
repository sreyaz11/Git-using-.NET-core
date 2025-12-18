using LocalGit.Commands.Model;
using System.Text.Json;

namespace LocalGit.Commands
{
    public class LogCommand
    {
        public static bool CallRespectiveLogCommand(string[] args)
        {
            if(args.Length == 1)
            {
                return Execute();
            }
            else if(args.Length == 3 && args[1].Equals("-n"))
            {
                return Execute(Convert.ToInt32(args[2]));
            }
            return false;
        }

        public static bool Execute()
        {
            try
            {
                List<string> visited = new List<string>();
                string currentCommit = ClsCommon.ReadHead();

                if (string.IsNullOrWhiteSpace(currentCommit))
                {
                    Console.WriteLine("No commits yet");
                    return true;
                }

                while (!string.IsNullOrWhiteSpace(currentCommit) && !visited.Contains(currentCommit))
                {
                    CommitModel commitObj = JsonSerializer.Deserialize<CommitModel>(File.ReadAllText($".LocalGit/commits/{currentCommit}"));

                    Console.WriteLine("----------------------------------------------------");
                    Console.WriteLine(
                        $"commit : {currentCommit}\n" +
                        $"Date   : {commitObj.Timestamp}\n" +
                        $"Message: {commitObj.Message}"
                    );

                    visited.Add(currentCommit);
                    currentCommit = commitObj.ParentCommitId;
                }

                Console.WriteLine("----------------------------------------------------");
                Console.WriteLine();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in CommitCommand execute");
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public static bool Execute(int n)
        {
            try
            {
                int count = 0;
                List<string> visited = new List<string>();
                string currentCommit = ClsCommon.ReadHead();

                if (string.IsNullOrWhiteSpace(currentCommit))
                {
                    Console.WriteLine("No commits yet");
                    return true;
                }

                while (!string.IsNullOrWhiteSpace(currentCommit) && !visited.Contains(currentCommit) && count < n)
                {
                    CommitModel commitObj = JsonSerializer.Deserialize<CommitModel>(File.ReadAllText($".LocalGit/commits/{currentCommit}"));

                    Console.WriteLine("----------------------------------------------------");
                    Console.WriteLine(
                        $"commit : {currentCommit}\n" +
                        $"Date   : {commitObj.Timestamp}\n" +
                        $"Message: {commitObj.Message}"
                    );

                    visited.Add(currentCommit);
                    currentCommit = commitObj.ParentCommitId;
                }

                Console.WriteLine("----------------------------------------------------");
                Console.WriteLine();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in CommitCommand execute");
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
