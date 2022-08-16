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
        public string KeyLetter = "";
        public readonly Keyboard.Key KeyboardKey;
        public readonly Mouse.Button MouseButton;
        public int Counter = 0;
        public readonly bool isKey = true;

        public Key(string key)
        {
            KeyLetter = key;
            if (!Enum.TryParse(key, out KeyboardKey))
            {
                if (KeyLetter[0] == 'm')
                {
                    KeyLetter = KeyLetter.Remove(0, 1);
                }
                if (Enum.TryParse(key.Substring(1), out MouseButton))
                //if(!Enum.TryParse(key, out MouseButton))
                {
                    //KeyLetter = key.Substring(1);
                    isKey = false;
                }
                else
                {
                    string exceptName = "Invalid key " + key;
                    throw new InvalidOperationException(exceptName);
                }

            }
        }
    }
}