using System.ComponentModel.DataAnnotations;

namespace BugTracker.Entity
{
    public class Person
    {
//JUST LINKED 1to1 people and applicationuser, go on from there
        public int? Id { get; set; }
        public string? Name { get; set; }
        [EmailAddress]
        public string? Email{ get; set; }
        public PersonRole Role { get; set; }
        public DateTime? CreatedDate  { get; set; }
        public List<PersonProject>? PersonnelProjects { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public string? ApplicationUserId { get; set; }
    }
    public enum PersonRole
    {
        User,
        Developer,
        Project_manager,
        Admin
    }
}
