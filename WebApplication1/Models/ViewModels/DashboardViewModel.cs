namespace WebApplication1.Models.ViewModels
{
    public class EntryCount
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }
    public class EntriesGraphViewModel
    {
        public List<EntryCount> EntriesPerDay { get; set; }
    }

    public class DashboardViewModel
    {
        public decimal TotalFeesAmount { get; set; }
        public int NewMembersCount { get; set; }
        public int ActiveAdmissionMembersCount { get; set; }

        // Additional Variable
        public List<EntryLog> EntryLogs { get; set; }

        // Editing Part
        public Dictionary<DateTime, EntryCount> MembersJoinedPerMonth { get; set; }
        public Dictionary<DateTime, EntryCount> MembersLeftPerMonth { get; set; }

        public List<Member> Members { get; set; }
        public Dictionary<DateTime, int> EntryLogsPerDay { get; set; }
    }

    
}
