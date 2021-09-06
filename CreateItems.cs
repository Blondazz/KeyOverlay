using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SFML.Graphics;
using SFML.System;

namespace KeyOverlay
{
    public static class CreateItems
    {
        public static readonly Font _font = new Font(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "Resources",
            "consolab.ttf")));
        public static RectangleShape CreateBar(RectangleShape square, int outlineThickness, float barSpeed)
        {
            var rect = new RectangleShape(new Vector2f(square.Size.X + outlineThickness * 2, barSpeed));
            rect.Position = new Vector2f(square.Position.X - outlineThickness,
                square.Position.Y - square.Size.Y - square.OutlineThickness);
            rect.FillColor = square.FillColor;
            return rect;
        }

        public static List<RectangleShape> CreateKeys(int keyAmount, int outlineThickness, float size, float ratioX, float ratioY,
            int margin, int marginBottom, RenderWindow window, Color backgroundColor, Color outlineColor)
        {
            var yPos = 900 * ratioY;
            var width = size + outlineThickness * 2;
            var keyList = new List<RectangleShape>();
            var keySpace = (window.Size.X - (margin + width) * 2)/ (keyAmount - 1);
            var spacing = (window.Size.X - margin * 2 - width * keyAmount) / (keyAmount - 1);
            for (int i = 0; i < keyAmount; i++)
            {
                var square = new RectangleShape(new Vector2f(size, size));
                
                square.FillColor = backgroundColor;
                square.OutlineColor = outlineColor;
                square.OutlineThickness = outlineThickness;
                square.Origin = new Vector2f(0, size);
                square.Position = new Vector2f(margin +outlineThickness + (width + spacing ) * i , yPos); 
                keyList.Add(square);
            }
            return keyList;
        }

        public static Text CreateText(string key, RectangleShape square, Color color, bool counter)
        {
            var text = new Text(key, _font);
            text.CharacterSize = (uint)(50 * square.Size.X / 140);
            text.Style = Text.Styles.Bold;
            text.FillColor = color;
            text.Origin = new Vector2f(text.GetLocalBounds().Width / 2f, 32 * square.Size.X / 140f);
            if(counter)
                text.Position = new Vector2f(square.GetGlobalBounds().Left + square.OutlineThickness + square.Size.X / 2f,
                    square.GetGlobalBounds().Top + square.OutlineThickness + square.Size.Y +text.CharacterSize);
            else
                text.Position = new Vector2f(square.GetGlobalBounds().Left + square.OutlineThickness + square.Size.X / 2f,
                    square.GetGlobalBounds().Top + square.OutlineThickness + square.Size.Y / 2f);

            return text;
        }
        
        public static Color CreateColor(string s)
        {
            var bytes = s.Split(',').Select(int.Parse).Select(Convert.ToByte).ToArray();
            return new Color(bytes[0], bytes[1], bytes[2], bytes[3]);
        }
    }
}