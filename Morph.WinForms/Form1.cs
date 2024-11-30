using Microsoft.FSharp.Core;
using Morph.GameState;

namespace Morph.WinForms
{
    public partial class Form1 : Form
    {
        private readonly Button[] _topCards = new Button[5];
        private readonly Button[][] _cells = new Button[8][];
        private readonly Button[] _bottomCards = new Button[5];

        private readonly StateManager Manager = new();

        private void ApplyState(State state)
        {
            Manager.State = state;
        }

        private void PerformClick(InteractiveButton interactiveButton)
        {
            if (!interactiveButton.Enabled)
                return;

            ApplyState(interactiveButton.NextState.Value);
        }

        private void UpdateButton(Button button, InteractiveButton interactiveButton)
        {
            button.Enabled = interactiveButton.Enabled;
            button.ForeColor =
                FSharpOption<Suit>.Some(Suit.Heart).Equals(interactiveButton.ButtonSuit) ? Color.Red
                : FSharpOption<Suit>.Some(Suit.Club).Equals(interactiveButton.ButtonSuit) ? Color.Green
                : FSharpOption<Suit>.Some(Suit.Diamond).Equals(interactiveButton.ButtonSuit) ? Color.Blue
                : Color.Black;
            button.BackgroundImage = null;
            try
            {
                if (interactiveButton.Pieces.FirstOrDefault() is PiecePosition pp)
                {
                    string suit = pp.Piece.Suit.IsHeart ? "hearts"
                            : pp.Piece.Suit.IsClub ? "clubs"
                            : pp.Piece.Suit.IsDiamond ? "diamonds"
                            : "spades";

                    int type = pp.Type.IsRook ? 4
                        : pp.Type.IsBishop ? 3
                        : pp.Type.IsKnight ? 2
                        : 1;

                    if (pp.Piece.Team.IsLight)
                    {
                        type += 4;
                    }

                    button.BackgroundImage = Image.FromFile($"cards/{suit}/{type}.png");
                }
            }
            catch (FileNotFoundException) { }
            button.Text = button.BackgroundImage == null
                ? interactiveButton.Label ?? ""
                : "";
            button.BackgroundImageLayout = ImageLayout.Zoom;
        }

        public Form1()
        {
            InitializeComponent();

            for (int col = 0; col < 5; col++)
            {
                int y = col;
                var button = new Button
                {
                    Dock = DockStyle.Fill,
                    Text = $"T {y}",
                    Font = new Font(SystemFonts.DefaultFont.FontFamily, 16f)
                };
                button.Click += (_, __) =>
                    PerformClick(Interactive.GetFiveButtonRow(Team.Light, Manager.State)[y]);
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
                        Text = $"{x},{y}",
                        Margin = Padding.Empty
                    };
                    button.Click += (_, __) =>
                        PerformClick(Interactive.GetBoardButtons(Manager.State)[x][y]);
                    _cells[x][y] = button;
                    tableLayoutPanel3.Controls.Add(button);
                }
            }

            for (int col = 0; col < 5; col++)
            {
                int y = col;
                var button = new Button
                {
                    Dock = DockStyle.Fill,
                    Text = $"B {y}",
                    Font = new Font(SystemFonts.DefaultFont.FontFamily, 16f)
                };
                button.Click += (_, __) =>
                    PerformClick(Interactive.GetFiveButtonRow(Team.Dark, Manager.State)[y]);
                _bottomCards[y] = button;
                tableLayoutPanel2.Controls.Add(button);
            }

            btnUndo.Click += (_, __) => Manager.State = Manager.PreviousStates.First();
            btnRedo.Click += (_, __) => Manager.State = Manager.NextStates.First();

            Manager = new StateManager();
            Manager.StateChanged += Manager_StateChanged;

            ApplyState(StateModule.CreateStartingState(DeckType.Pinochle));
        }

        private async void Manager_StateChanged(object? sender, Unit _)
        {
            var state = new
            {
                TopCards = Interactive.GetFiveButtonRow(Team.Light, Manager.State),
                Rows = Interactive.GetBoardButtons(Manager.State),
                BottomCards = Interactive.GetFiveButtonRow(Team.Dark, Manager.State),
            };

            for (int x = 0; x < state.TopCards.Length; x++)
            {
                UpdateButton(_topCards[x], state.TopCards[x]);
            }

            for (int x = 0; x < state.Rows.Length; x++)
            {
                for (int y = 0; y < state.Rows[x].Length; y++)
                {
                    UpdateButton(_cells[x][y], state.Rows[x][y]);
                }
            }

            for (int x = 0; x < state.BottomCards.Length; x++)
            {
                UpdateButton(_bottomCards[x], state.BottomCards[x]);
            }

            scoreControlLight.ApplyState(Manager.State, Team.Light);
            scoreControlDark.ApplyState(Manager.State, Team.Dark);

            toolStripStatusLabel1.Text = StateModule.GetStatusText(Manager.State);

            btnUndo.Enabled = Manager.PreviousStates.Any();
            btnRedo.Enabled = Manager.NextStates.Any();

            if (Enabled && Manager.State.Team.IsLight && chkUseEngine.Checked)
            {
                DateTime dt = DateTime.Now;
                var stateChain = Engine.GetBestStateChain(Manager.State);
                System.Diagnostics.Debug.WriteLine(DateTime.Now - dt);
                Enabled = false;
                foreach (var x in stateChain.stack.Reverse())
                {
                    await Task.Delay(250);
                    ApplyState(x);
                }
                Enabled = true;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, @"Morph Card Chess for Windows Forms
https://lakora.us/morph-card-chess/

© 2024 Isaac Schemm
GNU APGL v3 or later");
        }

        private void euchreDeckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Manager.Clear();
            ApplyState(StateModule.CreateStartingState(DeckType.Euchre));
        }

        private void pinochleDeckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Manager.Clear();
            ApplyState(StateModule.CreateStartingState(DeckType.Pinochle));
        }

        private void pokerDeckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Manager.Clear();
            ApplyState(StateModule.CreateStartingState(DeckType.Poker));
        }
    }
}
