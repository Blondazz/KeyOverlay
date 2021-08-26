using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace KeyOverlay {
    public class AppWindow
    {
        private readonly RenderWindow _window;
        private readonly Key _k1Key;
        private readonly Key _k2Key;
        private readonly float _barSpeed;
        private readonly float _ratioX;
        private readonly float _ratioY;
        private readonly int _margin;
        private readonly int _outlineThickness;

        private int _leftHold;
        private int _rightHold;
        public AppWindow() {
            var config = ReadConfig();
            var windowWidth = config["windowWidth"];
            var windowHeight = config["windowHeight"];
            _window = new RenderWindow(new VideoMode(uint.Parse(windowWidth!), uint.Parse(windowHeight!)), 
                "KeyOverlay", Styles.Default);
            

            _ratioX = float.Parse(windowWidth) / 480f;
            _ratioY = float.Parse(windowHeight) / 960f;

            var k1 = config["key1"];
            var k2 = config["key2"];
            _k1Key = new Key(k1);
            _k2Key = new Key(k2);
            if (config["displayKey1"] != "")
                _k1Key.KeyLetter = config["displayKey1"];
            if (config["displayKey2"] != "")
                _k2Key.KeyLetter = config["displayKey2"];
            _barSpeed = float.Parse(config["barSpeed"], CultureInfo.InvariantCulture);
            _margin = int.Parse(config["margin"]);
            _outlineThickness = int.Parse(config["outlineThickness"]);
        }

        private Dictionary<string, string> ReadConfig() {
            Dictionary<string, string> objectDict = new Dictionary<string, string>();
            var file = File.ReadLines("config.txt").ToArray();
            foreach (var s in file) {
                objectDict.Add(s.Split("=")[0], s.Split("=")[1]);
            }
            return objectDict;
        }
       
        private void OnClose(object sender, EventArgs e) {
            _window.Close();
        }

        public void Run() {

            Clock clock = new Clock();
            Event eEvent;
            _window.Closed += OnClose;
            _window.SetFramerateLimit(144);

            RectangleShape squareLeft = CreateSquare(true);
            RectangleShape squareRight = CreateSquare(false);

            Text textLeft = CreateText(_k1Key.KeyLetter, squareLeft);
            Text textRight = CreateText(_k2Key.KeyLetter, squareRight);

            Sprite image = new Sprite(new Texture(@"Resources\fading.png"));
            image.Scale = new Vector2f(image.Scale.X * _window.Size.X / 480f, image.Scale.Y *_ratioY);

            List<RectangleShape> rectListLeft = new();
            List<RectangleShape> rectListRight = new();

            while (_window.IsOpen) {        
                _window.Clear();
                _window.DispatchEvents();
                squareLeft.FillColor = Color.Transparent;
                squareRight.FillColor = Color.Transparent;
                if (Keyboard.IsKeyPressed(_k1Key.activatorKey)) {
                    _leftHold++;
                    squareLeft.FillColor = new Color(0xff, 0xff, 0xff, 100);
                }
                else {
                    _leftHold = 0;
                }
                if (Keyboard.IsKeyPressed(_k2Key.activatorKey)) {
                    _rightHold++;
                    squareRight.FillColor = new Color(0xff, 0xff, 0xff, 100);
                }
                else {
                    _rightHold = 0;
                }

                MoveRectangles(rectListLeft, rectListRight, _leftHold, _rightHold, squareLeft, squareRight);

                _window.Draw(squareLeft);
                _window.Draw(squareRight);
                _window.Draw(textLeft);
                _window.Draw(textRight);

                foreach (var rectangleShape in rectListLeft) {
                    _window.Draw(rectangleShape);
                }
                foreach (var rectangleShape in rectListRight) {
                    _window.Draw(rectangleShape);
                }
                
                _window.Draw(image);

                _window.Display();
            }

        }

        private void MoveRectangles(List<RectangleShape> rectListLeft, List<RectangleShape> rectListRight, int leftHold, int rightHold, 
            RectangleShape squareLeft, RectangleShape squareRight) {

            if (leftHold == 1) {
                var rect = CreateRect(squareLeft);
                rectListLeft.Add(rect);
            }
            else if (leftHold > 1) {
                var rect = rectListLeft.Last();
                rect.Size = new Vector2f(rect.Size.X, rect.Size.Y + _barSpeed);
            }

            if (rightHold == 1) {
                var rect = CreateRect(squareRight);
                rectListRight.Add(rect);
            }
            else if (rightHold > 1) {
                var rect = rectListRight.Last();
                rect.Size = new Vector2f(rect.Size.X, rect.Size.Y + _barSpeed);

            }

            foreach (var rect in rectListLeft) {
                rect.Position = new Vector2f(rect.Position.X, rect.Position.Y - _barSpeed);
            }
            foreach (var rect in rectListRight) {
                rect.Position = new Vector2f(rect.Position.X, rect.Position.Y - _barSpeed);
            }

            if (rectListLeft.Count > 0 && rectListLeft.First().Position.Y + rectListLeft.First().Size.Y < 0) {
                rectListLeft.RemoveAt(0);

            }
            if (rectListRight.Count > 0 && rectListRight.First().Position.Y + rectListRight.First().Size.Y < 0) {
                rectListRight.RemoveAt(0);
            }
        }

        private RectangleShape CreateRect(RectangleShape square) {
            RectangleShape rect = new RectangleShape(new Vector2f(square.Size.X+_outlineThickness*2, 0));
            rect.Position = new Vector2f(square.Position.X-_outlineThickness, square.Position.Y+1+(4-square.OutlineThickness));
            rect.FillColor = square.FillColor;
            return rect;
        }

        private RectangleShape CreateSquare(bool left) {
            RectangleShape square = new RectangleShape(new Vector2f(140f * _ratioX, 140f * _ratioY));
            square.FillColor = Color.Transparent;
            square.OutlineColor = Color.White;
            square.OutlineThickness = _outlineThickness;
            square.Position = left ? new Vector2f(_margin *_ratioX, 760 * _ratioY) 
                : new Vector2f(_window.Size.X - square.Size.X - (_margin * _ratioX), 760 * _ratioY);
            return square;
        }

        private Text CreateText(string key, RectangleShape square) {
            Font font = new Font(@"Resources\consolab.ttf");
            Text text = new Text(key, font);
            text.CharacterSize = (uint)(50 * square.Size.X / 140);
            text.Style = Text.Styles.Bold;

            text.Origin = new Vector2f(text.GetLocalBounds().Width / 2f, 32*square.Size.X/140f);
            text.Position = new Vector2f(square.GetGlobalBounds().Left+square.OutlineThickness+square.Size.X/2f, 
                square.GetGlobalBounds().Top+square.OutlineThickness+square.Size.Y/2f);
            
            return text;
        }
    }
}
