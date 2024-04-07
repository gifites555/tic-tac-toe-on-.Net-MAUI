using static final_work.NewPage1;

namespace final_work;

public class PlayerData
{
    private static PlayerData _instance;

    public Player Player { get; set; }

    private PlayerData()
    {
    }

    public static PlayerData Instance
    {
        get
        {
            if (_instance == null)
                _instance = new PlayerData();
            return _instance;
        }
    }
}

