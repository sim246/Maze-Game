using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace MazeGame
{
    public sealed class InputManager
    {
        private static InputManager instance = null;
        private List<Action> _actions;
        private List<Keys> _keys;
        private KeyboardState _previousState;

        private InputManager()
        {
            _actions = new List<Action>();
            _keys = new List<Keys>();
        }
        public static InputManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new InputManager();
                }
                return instance;
            }
        }
        public void Update()
        {
            var state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Up) & !_previousState.IsKeyDown(Keys.Up))
            {
                LoopKeys(state);
            }
            else if (state.IsKeyDown(Keys.Down) & !_previousState.IsKeyDown(Keys.Down))
            {
                LoopKeys(state);
            }
            else if (state.IsKeyDown(Keys.Left) & !_previousState.IsKeyDown(Keys.Left))
            {
                LoopKeys(state);
            }
            else if (state.IsKeyDown(Keys.Right) & !_previousState.IsKeyDown(Keys.Right))
            {
                LoopKeys(state);
            }

            _previousState = state;
        }

        private void LoopKeys(KeyboardState state)
        {
            int i = 0;
            foreach (var key in _keys)
            {
                if (state.IsKeyDown(key))
                {
                    _actions[i].Invoke();
                }
                i++;
            }
        }

        public void AddKeyHandler(Keys key, Action action)
        {
            _actions.Add(action);
            _keys.Add(key);
        }
    }
}
