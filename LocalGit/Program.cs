using LocalGit.Commands;

class Program
{
    public static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("LocalGit - commands: init, add, commit, log");
            return;
        }
        if (!InitCommand.IsGitInitialized() && args[0] != "init")
        {
            Console.WriteLine("LocalGit is not initialized, please initialize first");
            return;
        }
        //bool result = callCommandExecute(args[0], args[1]);
        string command = args[0];

        bool result = false;
        switch (command)
        {
            case "init":
                result = InitCommand.Execute();
                break;
            case "status":
                StatusCommand.Execute();
                break;
            case "add":
                string fileName = args[1];
                AddCommand.Execute(fileName);
                break;
            default:
                Console.WriteLine("command not recognized");
                break;
        }
    }

    private static bool callCommandExecute(string command, string fileName)
    {
        bool result = false;
        switch (command)
        {
            case "init":
                result = InitCommand.Execute();
                break;
            case "status" :
                StatusCommand.Execute();
                break;
            case "add":
                AddCommand.Execute(fileName);
                break;
            default :
                Console.WriteLine("command not recognized");
                break;
        }
        return result;
    }
}