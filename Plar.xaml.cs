using System;
using System.Diagnostics;
using Microsoft.Maui.Controls;
using static final_work.NewPage1;

namespace final_work
{
    public partial class Plar : ContentPage
    {
        private Player _player;
        private string _extraFirstName;
        private string _extraLastName;
        private bool _isPlayer1Turn;
        private Button[,] _cells;
        private int _lastMoveRow;
        private int _lastMoveColumn;
        private Stopwatch _gameStopwatch;




        public Plar(Player player, string extraFirstName, string extraLastName)
        {
            _player = player;
            _extraFirstName = extraFirstName;
            _extraLastName = extraLastName;

            InitializeComponent();
            _gameStopwatch = new Stopwatch();
            InitializeGame();
            FirstNameSecond.Text = $" {_player.Firstperson}  ";
            SecondNameSecond.Text = $" {_player.Lastperson} ";
            First.Text = $"{_player.FirstName}";
            Second.Text = $"{_player.LastName}";
          


        }

        private string GetRandomName()
        {
            Random random = new Random();
            int randomIndex = random.Next(2);

            return randomIndex == 0 ? $" {_player.Firstperson}  " : $"{_player.FirstName}";
        }
       
       

        private void InitializeGame()
        {
            _isPlayer1Turn = GetRandomName() == _player.FirstName; 

            TurnLabel.Text = _isPlayer1Turn ? _player.FirstName : _player.Firstperson;
            _cells = new Button[,]
            {
                { Cell00, Cell01, Cell02 },
                { Cell10, Cell11, Cell12 },
                { Cell20, Cell21, Cell22 }
            };

            foreach (var cell in _cells)
            {
                cell.Text = "";
                cell.Clicked += OnCellClicked;
            }
            _gameStopwatch.Reset();
        }

        private void OnCellClicked(object sender, EventArgs e)
        {
            var cell = (Button)sender;
            if (string.IsNullOrEmpty(cell.Text))
            {
                cell.Text = _isPlayer1Turn ? "X" : "O";

                
                _lastMoveRow = Grid.GetRow(cell);
                _lastMoveColumn = Grid.GetColumn(cell);


                if (!_gameStopwatch.IsRunning)
                {
                    StartGameTimer();
                }

                if (CheckForWinner())
                {
                    DisplayWinner(_isPlayer1Turn ? _player.FirstName : _player.Firstperson);
                }
                else
                {
                    _isPlayer1Turn = !_isPlayer1Turn;
                    TurnLabel.Text = _isPlayer1Turn ? _player.FirstName : _player.Firstperson;

                   
                    if (_cells.Cast<Button>().All(c => !string.IsNullOrEmpty(c.Text)))
                    {
                        DisplayWinner("Draw");
                    }
                }
            }
        }

        private bool CheckForWinner()
        {
            for (int i = 0; i < 3; i++)
            {
                if (_cells[i, 0].Text == _cells[i, 1].Text && _cells[i, 1].Text == _cells[i, 2].Text && !string.IsNullOrEmpty(_cells[i, 0].Text))
                {
                    return true;
                }
            }

            for (int i = 0; i < 3; i++)
            {
                if (_cells[0, i].Text == _cells[1, i].Text && _cells[1, i].Text == _cells[2, i].Text && !string.IsNullOrEmpty(_cells[0, i].Text))
                {
                    return true;
                }
            }

            if (_cells[0, 0].Text == _cells[1, 1].Text && _cells[1, 1].Text == _cells[2, 2].Text && !string.IsNullOrEmpty(_cells[0, 0].Text))
            {
                return true;
            }

            if (_cells[0, 2].Text == _cells[1, 1].Text && _cells[1, 1].Text == _cells[2, 0].Text && !string.IsNullOrEmpty(_cells[0, 2].Text))
            {
                return true;
            }

            
            return false;
        }

        private async void DisplayWinner(string winner)
        {
            string player1Name = _player.FirstName;
            string player2Name = _player.Firstperson;

            string outcomeMessage;

            if (winner == "Draw")
            {
                outcomeMessage = "It's a draw\nWant to play again?";
                _player.Draws++;
            }
            else if (_isPlayer1Turn)
            {

                _player.Wins++;
                outcomeMessage = $"{player1Name} won!\nWant to play again?";
            }
            else
            {
                _player.Losses++;   
                outcomeMessage = $"{player2Name} won!\n{player1Name} lost!\nWant to play again?";
            }
            StopGameTimer();
            var result = await DisplayAlert("Game over", outcomeMessage, "Yes", "No");

            if (result)
            {
                RestartGame();
            }
            else
            {
                Ongout();
            }

        }

        private void RestartGame()
        {
            InitializeGame();
           
        }

        private async void Ongout()
        {
            await Navigation.PushAsync(new ContentPage1());

        }

        private void OnRestartClicked(object sender, EventArgs e)
        {
           
            InitializeGame();
            
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {


            await Navigation.PushAsync(new ContentPage1());
        }

        private void StartGameTimer()
        {
            _gameStopwatch.Start();
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                UpdateTimerLabel();
                return true; 
            });
        }


        private void StopGameTimer()
        {
            _gameStopwatch.Stop();
            UpdateTimerLabel();
            TimeSpan elapsedTime = _gameStopwatch.Elapsed;
            SetTotalGameTime(_player, elapsedTime);
        }
        private void UpdateTimerLabel()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                TimeSpan elapsed = _gameStopwatch.Elapsed;
                TimerLabel.Text = $"Time: {elapsed.Hours:D2}:{elapsed.Minutes:D2}:{elapsed.Seconds:D2}";
            });
        }


    }

}