using Morph.GameState;
using System.Data;

namespace Morph.WinForms
{
    public partial class ScoreControl : UserControl
    {
        public ScoreControl()
        {
            InitializeComponent();
        }

        public string TeamName
        {
            get
            {
                return lblName.Text;
            }
            set
            {
                lblName.Text = value;
            }
        }

        public void ApplyState(State state, Team team)
        {
            int hearts = StateModule.GetPointsBySuit(team, Suit.Heart, state);
            int clubs = StateModule.GetPointsBySuit(team, Suit.Club, state);
            int diamonds = StateModule.GetPointsBySuit(team, Suit.Diamond, state);

            lblHearts.Text = $"{hearts}";
            chkHearts.Checked = hearts > 0;

            lblClubs.Text = $"{clubs}";
            chkClubs.Checked = clubs > 0;

            lblDiamonds.Text = $"{diamonds}";
            chkDiamonds.Checked = diamonds > 0;

            lblScore.Text = $"{StateModule.GetTotalPoints(team, state)}";
        }
    }
}
