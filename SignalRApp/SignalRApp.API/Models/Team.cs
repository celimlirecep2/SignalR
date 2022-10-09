namespace SignalRApp.API.Models
{
    public class Team
    {
        public Team()
        {
            //team.Users.add() gibi bir yapı kullanılmak istenildiği için kullandık
            Users= new List<User>(); 
        }
        public int Id { get; set; }
        public string Name { get; set; }
        //bire çok ilişkilerde dinleme yapsın diye virtual kullandık
        //Icollection a tıklanıldığında ise kendinden fonksiyonlarla beraber geliyor onun için kullanmış olduk
        public virtual ICollection<User> Users { get; set; }
    }
}
