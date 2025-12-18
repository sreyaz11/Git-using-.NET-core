namespace LocalGit.Commands.Model
{
    public class CommitModel
    {
        public string CommitId { get; set; }
        public string ParentCommitId { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        public Dictionary<string, string> Snapshot {  get; set; }
    }
}
