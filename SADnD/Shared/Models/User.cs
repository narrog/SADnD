namespace SADnD.Shared.Models
{
    public class User
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public ICollection<Character>? Characters { get; set; }
        public User(ApplicationUser appUser) 
        {
            Id = appUser.Id;
            UserName = appUser.UserName;
            Characters = appUser.Characters;
        }
    }
}