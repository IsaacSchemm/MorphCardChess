using Morph.GameState;

namespace Morph
{
    public class GameStateManager
    {
        public event EventHandler? StateChanged;

        private List<State> states = [];
        private int index = -1;

        public State State
        {
            get => states[index];
            set
            {
                if (states.Contains(value))
                {
                    index = states.IndexOf(value);
                }
                else
                {
                    states = states.Take(index + 1).Concat([value]).ToList();
                    index++;
                }
                StateChanged?.Invoke(this, new EventArgs());
            }
        }

        public void ClearUndo()
        {
            if (states.Count != 0)
                states = [states.Last()];
        }

        public State? PreviousState =>
            index > 0
            ? states[index - 1]
            : null;

        public State? NextState =>
            index + 1 < states.Count
            ? states[index + 1]
            : null;
    }
}
