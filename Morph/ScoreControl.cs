using Morph.GameState;
using System.Data;

namespace Morph
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

        public IEnumerable<Suit> Points
        {
            set
            {
                var hearts = value.Where(x => x.IsHeart);
                var clubs = value.Where(x => x.IsClub);
                var diamonds = value.Where(x => x.IsDiamond);

                lblHearts.Text = $"{hearts.Count()}";
                chkDiamonds.Checked = diamonds.Any();

                lblClubs.Text = $"{clubs.Count()}";
                chkHearts.Checked = hearts.Any();

                lblDiamonds.Text = $"{diamonds.Count()}";
                chkClubs.Checked = clubs.Any();

                lblScore.Text = hearts.Any() && clubs.Any() && diamonds.Any()
                    ? $"{value.Count()}"
                    : "";
            }
        }
    }
}
