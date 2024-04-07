namespace final_work;

public class NewPage1 : ContentPage
{

        public class Player
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Firstperson { get; set; }
        public string Lastperson { get; set; }
        public string BirthYear { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Draws { get; set; }
        public TimeSpan TotalGameTime { get; set; }

      
    }

    public static void SetTotalGameTime(Player player, TimeSpan totalTime)
    {
        if (totalTime > player.TotalGameTime)
        {
            player.TotalGameTime = totalTime;
        }
    }

}
