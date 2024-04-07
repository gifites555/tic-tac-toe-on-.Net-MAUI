using static final_work.NewPage1;

namespace final_work;

public partial class ContentPage1 : ContentPage
{

   
    public ContentPage1()
	{
		InitializeComponent();

       
    }
   
    private async void Profil(object sender, EventArgs e)
    {
        try
        {
            var player = PlayerData.Instance.Player;
            await Navigation.PushAsync(new Pro(player));

        }
        catch (Exception ex)
        {
            _ = ex.Message;

        }
    }
    private void Out(object sender, EventArgs e)
    {
        Application.Current.Quit();
    }
    private async void Computer1(object sender, EventArgs e)
    {
        try
        {
            var player = PlayerData.Instance.Player;
            await Navigation.PushAsync(new PC(player, "Player", ""));

        }
        catch (Exception ex)
        {
            _ = ex.Message;

        }
    }
    private async void Player1(object sender, EventArgs e)
    {
        try
        {
            var player = PlayerData.Instance.Player;
            await Navigation.PushAsync(new Plar2());

        }
        catch (Exception ex)
        {
            _ = ex.Message;

        }
       
    }
    
    
}
