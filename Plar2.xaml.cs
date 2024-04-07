using static final_work.NewPage1;

namespace final_work;

public partial class Plar2 : ContentPage
{

   
    public Plar2()
	{
		InitializeComponent();
       
    }

    private async void OnCounterClickedtogame(object sender, EventArgs e)
    {
        try
        {
            PlayerData.Instance.Player.Firstperson = FirstName2per.Text;
            PlayerData.Instance.Player.Lastperson = SecondName2per.Text;

            var plarPage = new Plar(PlayerData.Instance.Player, FirstName2per.Text, SecondName2per.Text);
            await Navigation.PushAsync(plarPage);

        }
        catch (Exception ex)
        {   
            _ = ex.Message;
            
        }
    }

    


}
