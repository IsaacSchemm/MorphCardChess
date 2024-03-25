﻿using Morph.GameState;
using Morph.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
