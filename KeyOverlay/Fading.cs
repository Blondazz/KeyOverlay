﻿using System.Collections.Generic;
using System.ComponentModel;
using SFML.Graphics;
using SFML.System;

namespace KeyOverlay {
    public static class Fading
    {
        public static List<Sprite> GetBackgroundColorFadingTexture(Color backgroundColor, uint windowWidth,
            float ratioY) {
            var sprites = new List<Sprite>();
            var alpha = 255;
            var color = backgroundColor;
            for (int i = 0; i < 255; i++)
            {
                Image img = ratioY>=.5f ? new Image(windowWidth, (uint)(2 * ratioY), color) : new Image(windowWidth, 1, color);
                var sprite = new Sprite(new Texture(img));
                sprite.Position = new Vector2f(0, img.Size.Y * i);
                sprites.Add(sprite);
                alpha -= 1;
                color.A = (byte)alpha;
            }

            return sprites;
        }
    }
}
