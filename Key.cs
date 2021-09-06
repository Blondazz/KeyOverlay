using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.Window;

namespace KeyOverlay
{
    public class Key
    {
        public int Hold { get; set; }
        public List<RectangleShape> BarList = new();
        public string KeyLetter;
        public readonly Keyboard.Key ActivatorKey;
        public int Counter = 0;

        public Key(string key)
        {
            KeyLetter = key;
            Enum.TryParse(key, out ActivatorKey);
        }
    }
}