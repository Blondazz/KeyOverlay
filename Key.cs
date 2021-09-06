using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SFML.Window;

namespace KeyOverlay {
    public class Key {
        public float Size { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public bool IsClicked { get; set; }
        public string KeyLetter;

        public readonly Keyboard.Key activatorKey;

        public Key(string key) {
            Size = 150f;
            KeyLetter = key;
            Enum.TryParse(key, out activatorKey);
        }
    }

    public class Button { 
        public float Size { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public bool IsClicked { get; set; }

        public string KeyLetter;

        public readonly Mouse.Button activatorKey;


        public Button(string button)
        {
            Size = 150f;
            KeyLetter = button;
            Enum.TryParse(button, out activatorKey);
        }


    }
}
