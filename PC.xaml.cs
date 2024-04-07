using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using static final_work.NewPage1;

namespace final_work
{
    public partial class PC : ContentPage
    {
        private Player _player;
        private bool _isPlayer1Turn;
        private Button[,] _cells;
        private int _lastMoveRow;
        private int _lastMoveColumn;
        private Stopwatch _gameStopwatch;
        private bool _isComputerThinking = false;
        
        public PC(Player player, string extraFirstName, string extraLastName)
        {
            _player = player;
          
            InitializeComponent();
            _gameStopwatch = new Stopwatch();
            InitializeGame();  
            First.Text = $"{_player.FirstName}";
            Second.Text = $"{_player.LastName}";
        }

        private void InitializeGame()
        {
            Random random = new Random();
            _isPlayer1Turn = random.Next(2) == 0; 

            TurnLabel.Text = _isPlayer1Turn ? _player.FirstName : "Computer";
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
            if (!_isPlayer1Turn)
            {   
                ComputerMove();
            }
        }

        private async void OnCellClicked(object sender, EventArgs e)
        {
           
            if (_isComputerThinking)
            {
                return;
            }

            var cell = (Button)sender;
            if (string.IsNullOrEmpty(cell.Text))
            {
                cell.Text = "O";
                _lastMoveRow = Grid.GetRow(cell);
                _lastMoveColumn = Grid.GetColumn(cell);
      


                if (!_gameStopwatch.IsRunning)
                {
                    StartGameTimer();
                }

                if (CheckForWinner())
                {
                    DisplayWinner(_isPlayer1Turn ? _player.FirstName : "Computer");
                }
                else
                {
                    _isPlayer1Turn = !_isPlayer1Turn;
                    TurnLabel.Text = _isPlayer1Turn ? _player.FirstName : "Computer";

                    if (!_isPlayer1Turn)
                    {
                      
                        _isComputerThinking = true;
                        await ComputerMove();
                        _isComputerThinking = false;

                     
                    }
                }
                if (!CheckForWinner() && !_cells.Cast<Button>().Any(c => string.IsNullOrEmpty(c.Text)))
                {                

                    DisplayWinner("Draw");

                }
            }
        }

        private async Task ComputerMove()
        {
            if (_cells.Cast<Button>().Any(c => string.IsNullOrEmpty(c.Text)))
            {
                ComputerLoadingIndicator.IsRunning = true;
                ComputerLoadingIndicator.IsVisible = true;
                await Task.Delay(2000);

                var winningMove = FindWinningMove("X");
                var blockingMove = FindWinningMove("O");

                if (winningMove != null)
                {
                    
                    MakeComputerMove(winningMove);
                }
                else if (blockingMove != null)
                {
                  
                    MakeComputerMove(blockingMove);
                }
                else
                {
                    
                    MakeRandomComputerMove();
                }

                if (!_gameStopwatch.IsRunning)
                {
                    StartGameTimer();
                }

                ComputerLoadingIndicator.IsRunning = false;
                ComputerLoadingIndicator.IsVisible = false;

                if (!CheckForWinner() && !_cells.Cast<Button>().Any(c => string.IsNullOrEmpty(c.Text)))
                {
                    
                }
            }
        }

        private Button FindWinningMove(string symbol)
        {
            // Check rows
            for (int i = 0; i < 3; i++)
            {
                if (CountSymbolsInLine(_cells[i, 0].Text, _cells[i, 1].Text, _cells[i, 2].Text, symbol) == 2)
                {
                    return GetEmptyCellInLine(_cells[i, 0], _cells[i, 1], _cells[i, 2]);
                }
            }

            // Check columns
            for (int i = 0; i < 3; i++)
            {
                if (CountSymbolsInLine(_cells[0, i].Text, _cells[1, i].Text, _cells[2, i].Text, symbol) == 2)
                {
                    return GetEmptyCellInLine(_cells[0, i], _cells[1, i], _cells[2, i]);
                }
            }

            // Check diagonals
            if (CountSymbolsInLine(_cells[0, 0].Text, _cells[1, 1].Text, _cells[2, 2].Text, symbol) == 2)
            {
                return GetEmptyCellInLine(_cells[0, 0], _cells[1, 1], _cells[2, 2]);
            }

            if (CountSymbolsInLine(_cells[0, 2].Text, _cells[1, 1].Text, _cells[2, 0].Text, symbol) == 2)
            {
                return GetEmptyCellInLine(_cells[0, 2], _cells[1, 1], _cells[2, 0]);
            }

            return null;
        }

        private int CountSymbolsInLine(string symbol1, string symbol2, string symbol3, string targetSymbol)
        {
            int count = 0;

            if (symbol1 == targetSymbol)
                count++;

            if (symbol2 == targetSymbol)
                count++;

            if (symbol3 == targetSymbol)
                count++;

            return count;
        }

        private Button GetEmptyCellInLine(Button cell1, Button cell2, Button cell3)
        {
            if (string.IsNullOrEmpty(cell1.Text))
                return cell1;

            if (string.IsNullOrEmpty(cell2.Text))
                return cell2;

            if (string.IsNullOrEmpty(cell3.Text))
                return cell3;

            return null;
        }

        private void MakeComputerMove(Button cell)
        {
            cell.Text = "X";

            _lastMoveRow = Grid.GetRow(cell);
            _lastMoveColumn = Grid.GetColumn(cell);

            if (CheckForWinner())
            {
                DisplayWinner("Computer");
            }
            else
            {
                _isPlayer1Turn = !_isPlayer1Turn;
                TurnLabel.Text = _isPlayer1Turn ? _player.FirstName : "Computer";
            }
        }

        private void MakeRandomComputerMove()
        {
            Random random = new Random();

            var emptyCells = _cells.Cast<Button>().Where(c => string.IsNullOrEmpty(c.Text)).ToArray();
            if (emptyCells.Length > 0)
            {
                var randomCell = emptyCells[random.Next(emptyCells.Length)];
                randomCell.Text = "X";

                _lastMoveRow = Grid.GetRow(randomCell);
                _lastMoveColumn = Grid.GetColumn(randomCell);

                if (CheckForWinner())
                {
                    DisplayWinner("Computer");
                }
                else
                {
                    _isPlayer1Turn = !_isPlayer1Turn;
                    TurnLabel.Text = _isPlayer1Turn ? _player.FirstName : "Computer";
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
                _player.Draws++;
                outcomeMessage = "It's a draw\nWant to play again?";       
            }
            else if (winner == player1Name)
            {
                _player.Wins++;
                outcomeMessage = $"{player1Name} won!\nWant to play again?";
            }
            else
            {
                _player.Losses++;
                outcomeMessage = $"The computer won!\n{player1Name} lost!\nWant to play again? ";
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

        private async void Ongout()
        {
            await Navigation.PushAsync(new ContentPage1());

        }

        private void RestartGame()
        {
            InitializeGame();

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