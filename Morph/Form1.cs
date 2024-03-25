using Microsoft.FSharp.Collections;
using Morph.GameState;
using Morph.Logic;

namespace Morph
{
    public partial class Form1 : Form
    {
        private readonly Button[] _topCards = new Button[3];
        private readonly Button[][] _cells = new Button[8][];
        private readonly Button[] _bottomCards = new Button[3];

        private readonly GameStateManager Manager = new();

        public Form1()
        {
            InitializeComponent();

            for (int col = 0; col < 3; col++)
            {
                int y = col;
                var button = new Button
                {
                    Dock = DockStyle.Fill,
                    Text = $"T {y}"
                };
                button.Click += (_, __) =>
                    Manager.State = Interactive.GetHandButtons(Team.Light, Manager.State)[y].NextState.Value;
                _topCards[y] = button;
                tableLayoutPanel1.Controls.Add(button);
            }

            for (int row = 0; row < 8; row++)
            {
                int x = row;
                _cells[x] = new Button[8];
                for (int col = 0; col < 8; col++)
                {
                    int y = col;
                    var button = new Button
                    {
                        Dock = DockStyle.Fill,
                        Text = $"{x},{y}"
                    };
                    button.Click += (_, __) =>
                        Manager.State = Interactive.GetBoardButtons(Manager.State)[x][y].NextState.Value;
                    _cells[x][y] = button;
                    tableLayoutPanel3.Controls.Add(button);
                }
            }

            for (int col = 0; col < 3; col++)
            {
                int y = col;
                var button = new Button
                {
                    Dock = DockStyle.Fill,
                    Text = $"B {y}"
                };
                button.Click += (_, __) =>
                    Manager.State = Interactive.GetHandButtons(Team.Dark, Manager.State)[y].NextState.Value;
                _bottomCards[y] = button;
                tableLayoutPanel2.Controls.Add(button);
            }

            btnUndo.Click += (_, __) => Manager.State = Manager.PreviousState ?? Manager.State;
            btnRedo.Click += (_, __) => Manager.State = Manager.NextState ?? Manager.State;

            Manager = new GameStateManager();
            Manager.StateChanged += Manager_StateChanged;

            Manager.State = StateModule.CreateStartingState(Team.Dark);
        }

        private void Manager_StateChanged(object? sender, EventArgs e)
        {
            var state = new
            {
                TopCards = Interactive.GetHandButtons(Team.Light, Manager.State),
                Rows = Interactive.GetBoardButtons(Manager.State),
                BottomCards = Interactive.GetHandButtons(Team.Dark, Manager.State),
            };

            for (int x = 0; x < state.TopCards.Length; x++)
            {
                _topCards[x].Text = state.TopCards[x].Label;
                _topCards[x].Enabled = state.TopCards[x].Enabled;
            }

            for (int x = 0; x < state.Rows.Length; x++)
            {
                for (int y = 0; y < state.Rows[x].Length; y++)
                {
                    _cells[x][y].Text = state.Rows[x][y].Label;
                    _cells[x][y].Enabled = state.Rows[x][y].Enabled;
                }
            }

            for (int x = 0; x < state.BottomCards.Length; x++)
            {
                _bottomCards[x].Text = state.BottomCards[x].Label;
                _bottomCards[x].Enabled = state.BottomCards[x].Enabled;
            }

            btnUndo.Enabled = Manager.PreviousState != null;
            btnRedo.Enabled = Manager.NextState != null;
        }
    }
}
