using Morph.GameState;

namespace Morph.GameStateManagement
{
    public class GameStateManager
    {
        public event EventHandler? StateChanged;

        private List<State> states = [];
        private int index = -1;

        public bool AutoAdvance { get; set; } = true;

        public State State
        {
            get => states[index];
            set
            {
                var effectiveNextState = value;

                if (states.Contains(effectiveNextState))
                {
                    index = states.IndexOf(effectiveNextState);
                }
                else
                {
                    while (AutoAdvance)
                    {
                        var buttons = Interactive.GetAllButtons(effectiveNextState)
                            .Where(b => b.Enabled);

                        if (buttons.Any(b => !b.Auto))
                            break;

                        var states = buttons
                            .Select(b => b.NextState.Value)
                            .Distinct();
                        if (states.Count() != 1)
                            break;

                        if (effectiveNextState == states.Single())
                            break;

                        effectiveNextState = states.Single();
                    }

                    states = states.Take(index + 1).Concat([effectiveNextState]).ToList();
                    index++;
                }
                StateChanged?.Invoke(this, new EventArgs());
            }
        }

        public void Clear()
        {
            index = -1;
            states = [];
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
