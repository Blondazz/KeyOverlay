using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Diagnostics;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace KeyOverlay {
    public class AppWindow
    {
        private readonly RenderWindow _window = null;
        private readonly Key _k1Key = null;
        private readonly Key _k2Key = null;
        private readonly Button _m1Key = null;
        private readonly Button _m2Key = null;
        private readonly float _barSpeed = 0;
        private readonly float _ratioX = 0;
        private readonly float _ratioY = 0;
        private readonly int _margin = 0;
        private readonly int _outlineThickness = 0;
        private readonly uint _maxFPS = 0;
        private Stopwatch stopWatch = new Stopwatch();

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
            Mouse.Button mouseTest = 0;

            if(Enum.TryParse(k1, out mouseTest))
            {
                _m1Key = new Button(k1);
            }
            else
            {
                _k1Key = new Key(k1);
            }

            if (Enum.TryParse(k2, out mouseTest))
            {
                _m2Key = new Button(k2);
            }
            else
            {
                _k2Key = new Key(k2);
            }

            if (config["displayKey1"] != "")
                _k1Key.KeyLetter = config["displayKey1"];
            if (config["displayKey2"] != "")
                _k2Key.KeyLetter = config["displayKey2"];
            _barSpeed = float.Parse(config["barSpeed"], CultureInfo.InvariantCulture);
            _maxFPS = uint.Parse(config["maxFPS"]);
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
            _window.SetFramerateLimit(_maxFPS);

            RectangleShape squareLeft = CreateItems.CreateSquare(true, _outlineThickness, _ratioX, _ratioY, _margin, _window);
            RectangleShape squareRight = CreateItems.CreateSquare(false, _outlineThickness, _ratioX, _ratioY, _margin, _window);

            Text textLeft = null;
            Text textRight = null;

            if(_k1Key != null)
            {
                textLeft = CreateItems.CreateText(_k1Key.KeyLetter, squareLeft);
            }
            else
            {
                textLeft = CreateItems.CreateText(_m1Key.KeyLetter, squareLeft);
            }

            if (_k2Key != null)
            {
                textRight = CreateItems.CreateText(_k2Key.KeyLetter, squareRight);
            }
            else
            {
                textRight = CreateItems.CreateText(_m2Key.KeyLetter, squareRight);
            }

            Sprite image = new Sprite(new Texture(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "Resources", "fading.png"))));
            image.Scale = new Vector2f(image.Scale.X * _window.Size.X / 480f, image.Scale.Y *_ratioY);

            List<RectangleShape> rectListLeft = new();
            List<RectangleShape> rectListRight = new();

            
            while (_window.IsOpen) {
                stopWatch.Start();
                _window.Clear();
                _window.DispatchEvents();
                squareLeft.FillColor = Color.Transparent;
                squareRight.FillColor = Color.Transparent;


                //uses short circuiting to ensure that a null value is never checked
                if ((_k1Key != null && Keyboard.IsKeyPressed(_k1Key.activatorKey)) || (_m1Key != null && Mouse.IsButtonPressed(_m1Key.activatorKey))) {
                    _leftHold++;
                    squareLeft.FillColor = new Color(0xff, 0xff, 0xff, 100);
                }
                else {
                    _leftHold = 0;
                }
                if ((_k2Key != null && Keyboard.IsKeyPressed(_k2Key.activatorKey)) || (_m2Key != null && Mouse.IsButtonPressed(_m2Key.activatorKey))) {
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
            stopWatch.Stop();
            float dt = Convert.ToSingle(stopWatch.Elapsed.TotalMilliseconds / 1000);
            float moveDist = dt * _barSpeed;
            
            stopWatch.Reset();

            if (leftHold == 1) {
                var rect = CreateItems.CreateRect(squareLeft, _outlineThickness, moveDist);
                rectListLeft.Add(rect);
            }
            else if (leftHold > 1) {
                var rect = rectListLeft.Last();
                rect.Size = new Vector2f(rect.Size.X, rect.Size.Y + moveDist);
            }

            if (rightHold == 1) {
                var rect = CreateItems.CreateRect(squareRight, _outlineThickness, moveDist);
                rectListRight.Add(rect);
            }
            else if (rightHold > 1) {
                var rect = rectListRight.Last();
                rect.Size = new Vector2f(rect.Size.X, rect.Size.Y + moveDist);

            }

            foreach (var rect in rectListLeft) {
                rect.Position = new Vector2f(rect.Position.X, rect.Position.Y - moveDist);
            }
            foreach (var rect in rectListRight) {
                rect.Position = new Vector2f(rect.Position.X, rect.Position.Y - moveDist);
            }

            if (rectListLeft.Count > 0 && rectListLeft.First().Position.Y + rectListLeft.First().Size.Y < 0) {
                rectListLeft.RemoveAt(0);

            }
            if (rectListRight.Count > 0 && rectListRight.First().Position.Y + rectListRight.First().Size.Y < 0) {
                rectListRight.RemoveAt(0);
            }
            stopWatch.Start();
        }

        
    }
}
