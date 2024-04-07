using Microsoft.Maui.Controls;
using static final_work.NewPage1;
namespace final_work;

public partial class MainPage : ContentPage
{


    private DateTime _dateOfBirth;

    public DateTime DateOfBirth
    {
        get { return _dateOfBirth; }
        set
        {
            if (_dateOfBirth != value)
            {
                _dateOfBirth = value;
                OnPropertyChanged(nameof(DateOfBirth));
                OnPropertyChanged(nameof(DateOfBirthString));
            }
        }
    }

    public string DateOfBirthString => DateOfBirth.ToString("dd/MM/yyyy");

    public MainPage()
	{
		InitializeComponent();
        BindingContext = this;
    }

    private void OnDateSelected(object sender, DateChangedEventArgs e)
    {
        DateOfBirth = e.NewDate;
    }


    private async void OnCounterClicked(object sender, EventArgs e)
	{   
        try
        {
            PlayerData.Instance.Player = new Player 
            { FirstName = FirstName.Text,
              LastName = SecondName.Text, 
              BirthYear = DateOfBirthString };
            await Navigation.PushAsync(new ContentPage1());
        }
        catch (Exception ex)
        {
            _ = ex.Message;
           
        }
    

    }


}
