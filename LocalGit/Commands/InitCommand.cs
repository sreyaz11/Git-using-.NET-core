namespace LocalGit.Commands
{
    public class InitCommand
    {
        public static bool Execute()
        {
            try
            {
                if(Directory.Exists(".LocalGit"))
                {
                    Console.WriteLine("LocalGit already initialized.");
                    return false;
                }
                Directory.CreateDirectory(".LocalGit/objects");
                Directory.CreateDirectory(".LocalGit/commits");

                File.WriteAllText(".LocalGit/HEAD", "");
                File.WriteAllText(".LocalGit/index", "");

                File.SetAttributes(".LocalGit", FileAttributes.Hidden);

                Console.WriteLine("Initialized empty LocalGit repository");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public static bool IsGitInitialized()
        {
            return Directory.Exists(".LocalGit");
        }
    }
}
