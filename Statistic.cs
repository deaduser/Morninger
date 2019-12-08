namespace morninger
{
    public class Statistic
    {
        public Statistic()
        {
            UserId = 0;
            UserName = "userName";
        }
        public Statistic(int userId, string userName)
        {
            UserId = userId;
            UserName = userName;
        }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string LastSeen { get; set; } = "10/01/01";
        public int Done { get; set; } = 0;
        public int Undone { get; set; } = 0;
        public int Ill { get; set; } = 0;
    }
}