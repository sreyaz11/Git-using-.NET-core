using LocalGit.Commands;
using LocalGit.Commands.Model;
using LocalGit.Core;
using System.Text.Json;


//🚀 Natural next steps(choose one)
//Design deleted file detection
//Handle staged new files
//Implement log(very easy now)
//Design checkout(rebuild working directory)
public static class StatusCommand
{
    public static void Execute()
    {
        try
        {
            EnsureRepo();

            Dictionary<string, string> index = ClsCommon.ReadIndex();
            CommitModel lastCommit = ReadLastCommit();

            var files = Directory.GetFiles(Directory.GetCurrentDirectory())
                                 .Select(Path.GetFileName);

            Console.WriteLine("File Status:\n");

            if (index.Count == 0 && lastCommit.Snapshot.Count == 0)
            {
                foreach(string file in files)
                    Console.WriteLine($"Untracked : {file}");
            }

            foreach (var file in files)
            {
                var content = File.ReadAllBytes(file);
                var hash = Hasher.Hash(content);

                if (!lastCommit.Snapshot.ContainsKey(file))
                {
                    Console.WriteLine($"Untracked : {file}");
                }
                else if (lastCommit.Snapshot[file].Equals(hash))                                            //I want to add working directory clean Console when no changes are there in the directory
                {
                    Console.WriteLine($"Clean: {file}");
                }
                else if (index.ContainsKey(file) && index[file].Equals(hash))
                {
                    Console.WriteLine($"Staged: {file}");
                }
                else
                {
                    Console.WriteLine($"Modified: {file}");
                }
            }
            Console.WriteLine("\n");
        }
        catch(Exception ex)
        {
            Console.WriteLine("Exception in StatusCommand execute");
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.ToString());
        }
        
    }

    static CommitModel ReadLastCommit()
    {
        try
        {
            var head = File.ReadAllText(".LocalGit/HEAD");

            string lastCommitJson = File.ReadAllText($".LocalGit/commits/{head}");

            CommitModel lastCommit = JsonSerializer.Deserialize<CommitModel>(lastCommitJson);

            return lastCommit;
        }
        catch(Exception ex)
        {
            Console.WriteLine("Exception in ReadLastCommit");
            Console.WriteLine(ex.ToString());
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    static void EnsureRepo()
    {
        if (!Directory.Exists(".LocalGit"))
            throw new Exception("Not a LocalGit repository");
    }
}
