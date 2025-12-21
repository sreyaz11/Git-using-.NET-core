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
    static List<string> Staged = new List<string>();

    static List<string> Modified = new List<string>();

    static List<string> Untracked = new List<string>();

    public static void Execute()
    {
        try
        {
            EnsureRepo();

            Dictionary<string, string> index = ClsCommon.ReadIndex();
            CommitModel lastCommit = ReadLastCommit();

            var files = Directory.GetFiles(Directory.GetCurrentDirectory())
                                 .Select(Path.GetFileName);

            if (index.Count == 0 && lastCommit.Snapshot.Count == 0)
            {
                foreach (string file in files)
                    Console.WriteLine($"Untracked : {file}");
                Console.WriteLine("\n");
                return;
            }

            foreach (var file in files)
            {
                var content = File.ReadAllBytes(file);
                var hash = Hasher.Hash(content);

                if (!lastCommit.Snapshot.ContainsKey(file) && !index.ContainsKey(file))
                {
                    Untracked.Add(file);
                }
                //else if (lastCommit.Snapshot.ContainsKey(file) && lastCommit.Snapshot[file].Equals(hash))                                            //I want to add working directory clean Console when no changes are there in the directory
                //{
                //    // Console.WriteLine($"Clean: {file}");
                //}
                else if (index.ContainsKey(file) && index[file].Equals(hash))
                {
                    Staged.Add(file);
                }
                else if (lastCommit.Snapshot.ContainsKey(file) && !index.ContainsKey(file) && !lastCommit.Snapshot[file].Equals(hash))
                {
                    Modified.Add(file);
                }
                else if (lastCommit.Snapshot.ContainsKey(file) && !index.ContainsKey(file) && !lastCommit.Snapshot[file].Equals(hash))
                {
                    Modified.Add(file);
                }
            }

            if (Staged.Count > 0 || Modified.Count > 0 || Untracked.Count > 0)
            {
                Console.WriteLine("File Status:\n");

                if(Staged.Count > 0)
                {
                    Console.WriteLine("Staged:");
                    foreach (string str in Staged)
                    {
                        Console.WriteLine($"\t{str}");
                    }
                    Console.WriteLine();
                }
                
                if(Modified.Count > 0)
                {
                    Console.WriteLine("Modified:");
                    foreach (string str in Modified)
                    {
                        Console.WriteLine($"\t{str}");
                    }
                    Console.WriteLine();
                }
                
                if(Untracked.Count > 0)
                {
                    Console.WriteLine("Untracked:");
                    foreach (string str in Untracked)
                    {
                        Console.WriteLine($"\t{str}");
                    }
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("Nothing to commit, working directory clean."); 
            }
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
            CommitModel lastCommit = new CommitModel();
            lastCommit.Snapshot = new Dictionary<string, string>();
            string head = File.ReadAllText(".LocalGit/HEAD");

            if(!string.IsNullOrWhiteSpace(head)){
                string lastCommitJson = File.ReadAllText($".LocalGit/commits/{head}");

                lastCommit = JsonSerializer.Deserialize<CommitModel>(lastCommitJson);

                return lastCommit;
            }
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
