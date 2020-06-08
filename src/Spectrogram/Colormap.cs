﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Spectrogram
{
    public class Colormap
    {
        public static Colormap Argo => new Colormap(new Colormaps.Argo());
        public static Colormap Blues => new Colormap(new Colormaps.Blues());
        public static Colormap Cividis => new Colormap(new Colormaps.Cividis());
        public static Colormap Grayscale => new Colormap(new Colormaps.Grayscale());
        public static Colormap Greens => new Colormap(new Colormaps.Greens());
        public static Colormap Jet => new Colormap(new Colormaps.Jet());
        public static Colormap Inferno => new Colormap(new Colormaps.Inferno());
        public static Colormap Magma => new Colormap(new Colormaps.Magma());
        public static Colormap Plasma => new Colormap(new Colormaps.Plasma());
        public static Colormap Viridis => new Colormap(new Colormaps.Viridis());

        private readonly IColormap cmap;
        public readonly string Name;
        public Colormap(IColormap colormap)
        {
            cmap = colormap ?? new Colormaps.Grayscale();
            Name = cmap.GetType().Name;
        }

        public override string ToString()
        {
            return $"Colormap {Name}";
        }

        public (byte r, byte g, byte b) GetRGB(byte value)
        {
            return cmap.GetRGB(value);
        }

        public (byte r, byte g, byte b) GetRGB(double fraction)
        {
            fraction = Math.Max(fraction, 0);
            fraction = Math.Min(fraction, 1);
            return cmap.GetRGB((byte)(fraction * 255));
        }

        public int GetInt32(byte value)
        {
            var (r, g, b) = GetRGB(value);
            return 255 << 24 | r << 16 | g << 8 | b;
        }

        public int GetInt32(double fraction)
        {
            var (r, g, b) = GetRGB(fraction);
            return 255 << 24 | r << 16 | g << 8 | b;
        }

        public Color GetColor(byte value)
        {
            return Color.FromArgb(GetInt32(value));
        }

        public Color GetColor(double fraction)
        {
            return Color.FromArgb(GetInt32(fraction));
        }

        public void Apply(Bitmap bmp)
        {
            System.Drawing.Imaging.ColorPalette pal = bmp.Palette;
            for (int i = 0; i < 256; i++)
                pal.Entries[i] = GetColor((byte)i);
            bmp.Palette = pal;
        }
    }
}
