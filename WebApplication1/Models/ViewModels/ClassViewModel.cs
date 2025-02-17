using NuGet.DependencyResolver;
using WebApplication1.Models;

namespace WebApplication1.Models.ViewModels
{
    public class ClassViewModel
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        // Other class properties

        // For displaying and selecting members and trainers
        public List<Member> AllMembers { get; set; }
        public List<int> SelectedMembers { get; set; }
        public List<Employee> AllTrainers { get; set; }
        public List<int> SelectedTrainers { get; set; }
    }

}