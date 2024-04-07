using Morph.GameState;

namespace Morph
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

            ApplyState(StateModule.CreateStartingState(Team.Dark));
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
                _topCards[x].Text = state.TopCards[x].Label;
                _topCards[x].Enabled = state.TopCards[x].Enabled;
                _topCards[x].ForeColor = state.TopCards[x].ForeColor;
            }

            for (int x = 0; x < state.Rows.Length; x++)
            {
                for (int y = 0; y < state.Rows[x].Length; y++)
                {
                    _cells[x][y].Text = state.Rows[x][y].Label.EndsWith(".png")
                        ? ""
                        : state.Rows[x][y].Label;
                    _cells[x][y].Enabled = state.Rows[x][y].Enabled;
                    _cells[x][y].ForeColor = state.Rows[x][y].ForeColor;
                    _cells[x][y].BackgroundImage = state.Rows[x][y].Label.EndsWith(".png")
                        ? Image.FromFile(state.Rows[x][y].Label)
                        : null;
                    _cells[x][y].BackgroundImageLayout = ImageLayout.Zoom;
                }
            }

            for (int x = 0; x < state.BottomCards.Length; x++)
            {
                _bottomCards[x].Text = state.BottomCards[x].Label;
                _bottomCards[x].Enabled = state.BottomCards[x].Enabled;
                _bottomCards[x].ForeColor = state.BottomCards[x].ForeColor;
            }

            scoreControlLight.Points = Manager.State.Points
                .Where(x => x.Item1 == Team.Light)
                .Select(x => x.Item2);
            scoreControlDark.Points = Manager.State.Points
                .Where(x => x.Item1 == Team.Dark)
                .Select(x => x.Item2);

            toolStripStatusLabel1.Text = StateModule.Describe(Manager.State);

            btnUndo.Enabled = Manager.PreviousState != null;
            btnRedo.Enabled = Manager.NextState != null;
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplyState(StateModule.CreateStartingState(Team.Dark));
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, @"Morph Card Chess v1.0
https://lakora.us/morph-card-chess/

© 2024 Isaac Schemm
GNU APGL v3 or later");
        }
    }
}
