namespace Tests.Models
{
    public enum TargetEnum
    {
        Default = 0,
        First = 1
    }

    public class InvalidCaseItem
    {
        public int Id { get; set; }

        public TargetEnum TargetEnum { get; set; }
    }
}
