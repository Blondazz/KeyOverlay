using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace KeyOverlay
{
    public class AppWindow
    {
        private readonly RenderWindow _window;
        private readonly List<Key> _keyList = new();
        private readonly List<RectangleShape> _squareList;
        private readonly float _barSpeed;
        private readonly float _ratioX;
        private readonly float _ratioY;
        private readonly int _outlineThickness;
        private readonly Color _backgroundColor;
        private readonly Color _keyBackgroundColor;
        private readonly Color _barColor;
        private readonly Color _fontColor;
        private readonly Color _pressFontColor;
        private readonly Sprite _background;
        private readonly bool _fading;
        private readonly bool _counter;
        private readonly List<Drawable> _staticDrawables = new();
        private readonly List<Text> _keyText = new();
        private readonly uint _maxFPS;
        private Clock _clock = new();


        public AppWindow()
        {
            var config = ReadConfig();
            var windowWidth = config["windowWidth"];
            var windowHeight = config["windowHeight"];
            _window = new RenderWindow(new VideoMode(uint.Parse(windowWidth!), uint.Parse(windowHeight!)),
                "KeyOverlay", Styles.Default);

            //calculate screen ratio relative to original program size for easy resizing
            _ratioX = float.Parse(windowWidth) / 480f;
            _ratioY = float.Parse(windowHeight) / 960f;

            _barSpeed = float.Parse(config["barSpeed"], CultureInfo.InvariantCulture);
            _outlineThickness = int.Parse(config["outlineThickness"]);
            _backgroundColor = CreateItems.CreateColor(config["backgroundColor"]);
            _keyBackgroundColor = CreateItems.CreateColor(config["keyColor"]);
            _barColor = CreateItems.CreateColor(config["barColor"]);
            _maxFPS = uint.Parse(config["maxFPS"]);

            //get background image if in config
            if (config["backgroundImage"] != "")
                _background = new Sprite(new Texture(
                    Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "Resources",
                        config["backgroundImage"]))));

            //create keys which will be used to create the squares and text
            var keyAmount = int.Parse(config["keyAmount"]);
            for (var i = 1; i <= keyAmount; i++)
                try
                {
                    var key = new Key(config[$"key" + i]);
                    if (config.ContainsKey($"displayKey" + i))
                        if (config[$"displayKey" + i] != "")
                            key.KeyLetter = config[$"displayKey" + i];
                    _keyList.Add(key);
                }
                catch (InvalidOperationException e)
                {
                    //invalid key
                    Console.WriteLine(e.Message);
                    using var sw = new StreamWriter("keyErrorMessage.txt");
                    sw.WriteLine(e.Message);
                }

            //create squares and add them to _staticDrawables list
            var outlineColor = CreateItems.CreateColor(config["borderColor"]);
            var keySize = int.Parse(config["keySize"]);
            var margin = int.Parse(config["margin"]);
            _squareList = CreateItems.CreateKeys(keyAmount, _outlineThickness, keySize, _ratioX, _ratioY, margin,
                _window, _keyBackgroundColor, outlineColor);
            foreach (var square in _squareList) _staticDrawables.Add(square);

            //create text and add it ti _staticDrawables list
            _fontColor = CreateItems.CreateColor(config["fontColor"]);
            _pressFontColor = CreateItems.CreateColor(config["pressFontColor"]);
            for (var i = 0; i < keyAmount; i++)
            {
                var text = CreateItems.CreateText(_keyList.ElementAt(i).KeyLetter, _squareList.ElementAt(i),
                    _fontColor, false);
                _keyText.Add(text);
                _staticDrawables.Add(text);
            }

            if (config["fading"] == "yes")
                _fading = true;
            if (config["keyCounter"] == "yes")
                _counter = true;
        }

        private Dictionary<string, string> ReadConfig()
        {
            var objectDict = new Dictionary<string, string>();
            var file = File.ReadLines("config.txt").ToArray();
            foreach (var s in file) objectDict.Add(s.Split("=")[0], s.Split("=")[1]);
            return objectDict;
        }

        private void OnClose(object sender, EventArgs e)
        {
            _window.Close();
        }

        public void Run()
        {
            _window.Closed += OnClose;
            _window.SetFramerateLimit(_maxFPS);

            //Creating a sprite for the fading effect
            var fadingList = Fading.GetBackgroundColorFadingTexture(_backgroundColor, _window.Size.X, _ratioY);
            var fadingTexture = new RenderTexture(_window.Size.X, (uint)(255 * 2 * _ratioY));
            fadingTexture.Clear(Color.Transparent);
            if (_fading)
                foreach (var sprite in fadingList)
                    fadingTexture.Draw(sprite);
            fadingTexture.Display();
            var fadingSprite = new Sprite(fadingTexture.Texture);


            while (_window.IsOpen)
            {
                _window.Clear(_backgroundColor);
                _window.DispatchEvents();
                //if no keys are being held fill the square with bg color
                foreach (var square in _squareList) square.FillColor = _keyBackgroundColor;
                //if a key is being held, change the key bg and increment hold variable of key
                foreach (var key in _keyList)
                    if (key.isKey && Keyboard.IsKeyPressed(key.KeyboardKey) ||
                        !key.isKey && Mouse.IsButtonPressed(key.MouseButton))
                    {
                        _keyText.ElementAt(_keyList.IndexOf(key)).FillColor = _pressFontColor;
                        key.Hold++;
                        _squareList.ElementAt(_keyList.IndexOf(key)).FillColor = _barColor;
                    }
                    else
                    {
                        _keyText.ElementAt(_keyList.IndexOf(key)).FillColor = _fontColor;
                        key.Hold = 0;
                    }

                MoveBars(_keyList, _squareList);

                //draw bg from image if not null

                if (_background is not null)
                    _window.Draw(_background);
                foreach (var staticDrawable in _staticDrawables) _window.Draw(staticDrawable);

                foreach (var key in _keyList)
                {
                    if (_counter)
                    {
                        var text = CreateItems.CreateText(Convert.ToString(key.Counter),
                            _squareList.ElementAt(_keyList.IndexOf(key)),
                            _fontColor, true);
                        _window.Draw(text);
                    }

                    foreach (var bar in key.BarList)
                        _window.Draw(bar);
                }

                _window.Draw(fadingSprite);

                _window.Display();
            }
        }

        /// <summary>
        /// if a key is a new input create a new bar, if it is being held stretch it and move all bars up
        /// </summary>
        private void MoveBars(List<Key> keyList, List<RectangleShape> squareList)
        {
            var moveDist = _clock.Restart().AsSeconds() * _barSpeed;

            foreach (var key in keyList)
            {
                if (key.Hold == 1)
                {
                    var rect = CreateItems.CreateBar(squareList.ElementAt(keyList.IndexOf(key)), _outlineThickness,
                        moveDist);
                    key.BarList.Add(rect);
                    key.Counter++;
                }
                else if (key.Hold > 1)
                {
                    var rect = key.BarList.Last();
                    rect.Size = new Vector2f(rect.Size.X, rect.Size.Y + moveDist);
                }

                foreach (var rect in key.BarList)
                    rect.Position = new Vector2f(rect.Position.X, rect.Position.Y - moveDist);
                if (key.BarList.Count > 0 && key.BarList.First().Position.Y + key.BarList.First().Size.Y < 0)
                    key.BarList.RemoveAt(0);
            }
        }
    }
}