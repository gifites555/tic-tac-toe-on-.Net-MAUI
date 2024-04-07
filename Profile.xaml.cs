using Microsoft.Maui.Controls;
using System;
using static final_work.NewPage1;
namespace final_work;

public partial class Pro : ContentPage
{

    private Player _player;
    public Pro(Player player)
    {
        InitializeComponent();
        _player = player;
        FirstNameLabel.Text = $"Name: {_player.FirstName}";
        SecondNameLabel.Text = $"Surname: {_player.LastName}";
        BirthNameLabel.Text = $"Birthday {_player.BirthYear}";
        WinsNameLabel.Text = $"Victorys {_player.Wins}";
        LossesNameLabel.Text = $"Defeats {_player.Losses}";
        DrawsNameLabel.Text = $"Draws {_player.Draws}";
        string formattedTime = $"{_player.TotalGameTime.Hours:D2}:{_player.TotalGameTime.Minutes:D2}:{_player.TotalGameTime.Seconds:D2}";
        TotalGameTime1NameLabel.Text = $"Maximum time in game {formattedTime}";
    }



    private async void gooutProfil(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ContentPage1());
    }

    private  void Out(object sender, EventArgs e)
    {
        Application.Current.Quit();
    }
}
