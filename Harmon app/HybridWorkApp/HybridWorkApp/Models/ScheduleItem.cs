namespace HybridWorkApp.Models
{
    public enum ScheduleType
    {
        Presential,
        Remote,
        Hybrid
    }

    public class ScheduleItem
    {
        public string Day { get; set; } = string.Empty;
        public ScheduleType Type { get; set; } = ScheduleType.Presential;
        public string Notes { get; set; } = string.Empty;
    }
}
