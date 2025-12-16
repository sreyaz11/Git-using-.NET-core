using LocalGit.Core;

public static class StatusCommand
{
    public static void Execute()
    {
        EnsureRepo();

        Dictionary<string, string> index = ReadIndex();
        Dictionary<string, string> lastCommit = ReadLastCommit();

        var files = Directory.GetFiles(Directory.GetCurrentDirectory())
                             .Select(Path.GetFileName);

        Console.WriteLine("File Status:\n");

        foreach (var file in files)
        {
            var content = File.ReadAllBytes(file);
            var hash = Hasher.Hash(content);

            if (!index.ContainsKey(file))
            {
                Console.WriteLine($"Untracked: {file}");
            }
            else if (!lastCommit.ContainsKey(file))
            {
                Console.WriteLine($"Staged: {file}");
            }
            else if (lastCommit[file] != hash)
            {
                Console.WriteLine($"Modified: {file}");
            }
            else
            {
                Console.WriteLine($"Clean: {file}");
            }
        }
    }

    static Dictionary<string, string> ReadIndex()
    {
        var dict = new Dictionary<string, string>();

        if (!File.Exists(".LocalGit/index")) return dict;

        foreach (var line in File.ReadAllLines(".LocalGit/index"))
        {
            string[] parts = line.Split('|');
            if (parts.Length == 2)
                dict[parts[0]] = parts[1];
        }
        return dict;
    }

    static Dictionary<string, string> ReadLastCommit()
    {
        var dict = new Dictionary<string, string>();
        var head = File.ReadAllText(".LocalGit/HEAD");

        if (string.IsNullOrEmpty(head)) return dict;

        var commitFile = $".LocalGit/commits/{head}.json";
        var json = File.ReadAllText(commitFile);

        var filesSection = json.Split("\"files\":")[1]
                               .Split('}')[0]
                               .Trim('"');

        foreach (var line in filesSection.Split("\\n"))
        {
            var parts = line.Split('|');
            if (parts.Length == 2)
                dict[parts[0]] = parts[1];
        }

        return dict;
    }

    static void EnsureRepo()
    {
        if (!Directory.Exists(".LocalGit"))
            throw new Exception("Not a LocalGit repository");
    }
}
