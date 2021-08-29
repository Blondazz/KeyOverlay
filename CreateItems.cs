using SFML.Graphics;
using SFML.System;

namespace KeyOverlay {
    public static class CreateItems {
        public static RectangleShape CreateRect(RectangleShape square, int outlineThickness, float barSpeed) {
            RectangleShape rect = new RectangleShape(new Vector2f(square.Size.X + outlineThickness * 2, barSpeed));
            rect.Position = new Vector2f(square.Position.X - outlineThickness, square.Position.Y-square.Size.Y + 1);
            rect.FillColor = square.FillColor;
            return rect;
        }

        public static RectangleShape CreateSquare(bool left, int outlineThickness, float ratioX, float ratioY, float margin, RenderWindow window) {
            RectangleShape square = new RectangleShape(new Vector2f(140f * ratioX, 140f * ratioX));
            square.FillColor = Color.Transparent;
            square.OutlineColor = Color.White;
            square.OutlineThickness = outlineThickness;
            square.Origin = new Vector2f(0, 140f * ratioX);
            square.Position = left ? new Vector2f(margin * ratioX, 900 * ratioY)
                : new Vector2f(window.Size.X - square.Size.X - (margin * ratioX), 900 * ratioY);
            return square;
        }

        public static Text CreateText(string key, RectangleShape square) {
            Font font = new Font(@"Resources\consolab.ttf");
            Text text = new Text(key, font);
            text.CharacterSize = (uint)(50 * square.Size.X / 140);
            text.Style = Text.Styles.Bold;

            text.Origin = new Vector2f(text.GetLocalBounds().Width / 2f, 32 * square.Size.X / 140f);
            text.Position = new Vector2f(square.GetGlobalBounds().Left + square.OutlineThickness + square.Size.X / 2f,
                square.GetGlobalBounds().Top + square.OutlineThickness + square.Size.Y / 2f);

            return text;
        }
    }
}
