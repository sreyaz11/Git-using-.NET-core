using System.Security.Cryptography;

namespace LocalGit.Core
{
    public class Hasher
    {
        public static string Hash(byte[] content)
        {
            using var sha = SHA256.Create();
            return Convert.ToHexString(sha.ComputeHash(content));
        }
    }
}
