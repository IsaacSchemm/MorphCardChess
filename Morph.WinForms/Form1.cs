using Microsoft.FSharp.Core;
using Morph.GameState;
using Morph.GameStateManagement;

namespace Morph.WinForms
{
    public partial class Form1 : Form
    {
        private readonly Button[] _topCards = new Button[5];
        private readonly Button[][] _cells = new Button[8][];
        private readonly Button[] _bottomCards = new Button[5];

        private readonly GameStateManager Manager = new();

        private void ApplyState(State state)
        {
            var buttons = Interactive.GetAllButtons(state)
                .Where(b => b.Enabled);

            if (buttons.All(b => b.Auto))
            {
                var states = buttons
                    .Select(b => b.NextState.Value)
                    .Distinct();
                if (states.Count() == 1)
                {
                    ApplyState(states.Single());
                    return;
                }
            }

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
            button.Text = interactiveButton.ImagePaths.Length == 0
                ? interactiveButton.Label ?? ""
                : "";
            button.Enabled = interactiveButton.Enabled;
            button.ForeColor =
                FSharpOption<Suit>.Some(Suit.Heart).Equals(interactiveButton.ButtonSuit) ? Color.Red
                : FSharpOption<Suit>.Some(Suit.Club).Equals(interactiveButton.ButtonSuit) ? Color.Green
                : FSharpOption<Suit>.Some(Suit.Diamond).Equals(interactiveButton.ButtonSuit) ? Color.Blue
                : Color.Black;
            try
            {
                button.BackgroundImage = interactiveButton.ImagePaths.FirstOrDefault() is string path
                    ? Image.FromFile(path)
                    : null;
            }
            catch (FileNotFoundException)
            {
                button.BackgroundImage = null;
            }
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
                    PerformClick(Interactive.GetHandAndPromotionButtons(Team.Light, Manager.State)[y]);
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
                    PerformClick(Interactive.GetHandAndPromotionButtons(Team.Dark, Manager.State)[y]);
                _bottomCards[y] = button;
                tableLayoutPanel2.Controls.Add(button);
            }

            btnUndo.Click += (_, __) => Manager.State = Manager.PreviousState ?? Manager.State;
            btnRedo.Click += (_, __) => Manager.State = Manager.NextState ?? Manager.State;

            Manager = new GameStateManager();
            Manager.StateChanged += Manager_StateChanged;

            ApplyState(StateModule.CreateStartingState(DeckType.Euchre));
        }

        private void Manager_StateChanged(object? sender, EventArgs e)
        {
            var state = new
            {
                TopCards = Interactive.GetHandAndPromotionButtons(Team.Light, Manager.State),
                Rows = Interactive.GetBoardButtons(Manager.State),
                BottomCards = Interactive.GetHandAndPromotionButtons(Team.Dark, Manager.State),
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

            toolStripStatusLabel1.Text = StateModule.Describe(Manager.State);

            btnUndo.Enabled = Manager.PreviousState != null;
            btnRedo.Enabled = Manager.NextState != null;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, @"Morph Card Chess v1.1
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
