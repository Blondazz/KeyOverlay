using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SFML.Graphics;
using SFML.System;

namespace KeyOverlay
{
    
    public class CreateItems
    {


        public static readonly Font _font = new Font(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "Resources",
            "consolab.ttf")));
        
        public static List<CircleShape> CreateKeys(int keyAmount, int outlineThickness, float size, float ratioX, float ratioY,
            int margin, RenderWindow window, Color backgroundColor, Color outlineColor)
        {
                var yPos = 870 * ratioY;
                var keyList = new List<CircleShape>();
                var spacing = (window.Size.X - margin * 2 - size * keyAmount) / (keyAmount - 1);

                for (int i = 0; i < keyAmount; i++)
                {
                    var circle = new CircleShape(size / 2);

                    circle.FillColor = backgroundColor;
                    circle.OutlineColor = outlineColor;
                    circle.OutlineThickness = outlineThickness;
                    
                    circle.Origin = new Vector2f(size / 2, size / 2);
                    circle.Position = new Vector2f(margin + size / 2 + outlineThickness - 5 + (size + spacing) * i, yPos);
                    keyList.Add(circle);
                }

                return keyList;
        }

        public static Text CreateText(string textString, CircleShape shape, Color textColor, bool centerText = true)
        {
            Text text = new Text(textString, new Font(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "Resources",
            "consolab.ttf"))));

            text.FillColor = textColor;
            text.Origin = new Vector2f(text.GetLocalBounds().Width / 1.8f, text.GetLocalBounds().Height );
            text.Position = shape.Position;

            return text;            
        }
        
        public static Color CreateColor(string s)
        {
            var bytes = s.Split(',').Select(int.Parse).Select(Convert.ToByte).ToArray();

            return new Color(bytes[0], bytes[1], bytes[2], bytes[3]);
        }

        public static CircleShape CreateBar(CircleShape circle, int outlineThickness, float moveDist, Color barColor, int barSides, int rotation)
        {
            
            uint Ubar = Convert.ToUInt32(barSides);

            var bar = new CircleShape(circle.Radius, Ubar);

            bar.Origin = new Vector2f(bar.Radius, bar.Radius);
            bar.FillColor = barColor;
            bar.Rotation = rotation;
            bar.Position = new Vector2f(circle.Position.X, circle.Position.Y - circle.Radius / 2);

            return bar;
        }
    }


}
