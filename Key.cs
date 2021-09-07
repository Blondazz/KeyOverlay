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

        public Key(string keyString)
        {
            string[] key = keyString.Split(new[] { "|" }, StringSplitOptions.None);
            KeyLetter = key[0];
            Counter = int.Parse(key[1]);

            if (!Enum.TryParse(key[0], out KeyboardKey))
            {
                if (KeyLetter[0] == 'm')
                {
                    KeyLetter = KeyLetter.Remove(0, 1);
                }
                if (Enum.TryParse(key[0].Substring(1), out MouseButton))
                //if(!Enum.TryParse(key, out MouseButton))
                {
                    //KeyLetter = key.Substring(1);
                    isKey = false;
                }
                else
                {
                    string exceptName = "Invalid key " + key[0];
                    throw new InvalidOperationException(exceptName);
                }

            }
        }
    }
}