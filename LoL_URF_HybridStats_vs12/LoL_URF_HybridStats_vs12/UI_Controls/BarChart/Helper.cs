namespace LoL_URF_HybridStats_vs12.Charts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Media;

    public static class Helper
    {
        private static List<Color> colorList = new List<Color>();
        private static List<Color> darkColorList = new List<Color>();
        private static Dictionary<string, Color> addedList = new Dictionary<string, Color>();

        private static object lockObject = new object();


        const int totalColorIndex = 139;


        static Helper()
        {
            colorList.Add(Colors.AliceBlue);
            colorList.Add(Colors.AntiqueWhite);
            colorList.Add(Colors.Aqua);
            colorList.Add(Colors.Aquamarine);
            colorList.Add(Colors.Azure);
            colorList.Add(Colors.Beige);
            colorList.Add(Colors.Bisque);
            colorList.Add(Colors.Black);
            colorList.Add(Colors.BlueViolet);
            colorList.Add(Colors.Brown);
            colorList.Add(Colors.BurlyWood);
            colorList.Add(Colors.CadetBlue);
            colorList.Add(Colors.Chartreuse);
            colorList.Add(Colors.Chocolate);
            colorList.Add(Colors.Coral);
            colorList.Add(Colors.CornflowerBlue);
            colorList.Add(Colors.Cornsilk);
            colorList.Add(Colors.Crimson);
            colorList.Add(Colors.Cyan);
            colorList.Add(Colors.DarkBlue);
            colorList.Add(Colors.DarkCyan);
            colorList.Add(Colors.DarkGoldenrod);
            colorList.Add(Colors.DarkGray);
            colorList.Add(Colors.DarkGreen);
            colorList.Add(Colors.DarkKhaki);
            colorList.Add(Colors.DarkMagenta);
            colorList.Add(Colors.DarkOliveGreen);
            colorList.Add(Colors.DarkOrange);
            colorList.Add(Colors.DarkOrchid);
            colorList.Add(Colors.DarkRed);
            colorList.Add(Colors.DarkSalmon);
            colorList.Add(Colors.DarkSeaGreen);
            colorList.Add(Colors.DarkSlateBlue);
            colorList.Add(Colors.DarkSlateGray);
            colorList.Add(Colors.DarkTurquoise);
            colorList.Add(Colors.DarkViolet);
            colorList.Add(Colors.DeepPink);
            colorList.Add(Colors.DeepSkyBlue);
            colorList.Add(Colors.DimGray);
            colorList.Add(Colors.DodgerBlue);
            colorList.Add(Colors.Firebrick);
            colorList.Add(Colors.FloralWhite);
            colorList.Add(Colors.ForestGreen);
            colorList.Add(Colors.Fuchsia);
            colorList.Add(Colors.Gainsboro);
            colorList.Add(Colors.GhostWhite);
            colorList.Add(Colors.Gold);
            colorList.Add(Colors.Goldenrod);
            colorList.Add(Colors.Gray);
            colorList.Add(Colors.Green);
            colorList.Add(Colors.GreenYellow);
            colorList.Add(Colors.Honeydew);
            colorList.Add(Colors.HotPink);
            colorList.Add(Colors.IndianRed);
            colorList.Add(Colors.Indigo);
            colorList.Add(Colors.Ivory);
            colorList.Add(Colors.Khaki);
            colorList.Add(Colors.Lavender);
            colorList.Add(Colors.LavenderBlush);
            colorList.Add(Colors.LawnGreen);
            colorList.Add(Colors.LemonChiffon);
            colorList.Add(Colors.LightBlue);
            colorList.Add(Colors.LightCoral);
            colorList.Add(Colors.LightCyan);
            colorList.Add(Colors.LightGoldenrodYellow);
            colorList.Add(Colors.LightGray);
            colorList.Add(Colors.LightGreen);
            colorList.Add(Colors.LightPink);
            colorList.Add(Colors.LightSalmon);
            colorList.Add(Colors.LightSeaGreen);
            colorList.Add(Colors.LightSkyBlue);
            colorList.Add(Colors.LightSlateGray);
            colorList.Add(Colors.LightSteelBlue);
            colorList.Add(Colors.LightYellow);
            colorList.Add(Colors.Lime);
            colorList.Add(Colors.LimeGreen);
            colorList.Add(Colors.Linen);
            colorList.Add(Colors.Magenta);
            colorList.Add(Colors.Maroon);
            colorList.Add(Colors.MediumAquamarine);
            colorList.Add(Colors.MediumBlue);
            colorList.Add(Colors.MediumOrchid);
            colorList.Add(Colors.MediumPurple);
            colorList.Add(Colors.MediumSeaGreen);
            colorList.Add(Colors.MediumSlateBlue);
            colorList.Add(Colors.MediumSpringGreen);
            colorList.Add(Colors.MediumTurquoise);
            colorList.Add(Colors.MediumVioletRed);
            colorList.Add(Colors.MidnightBlue);
            colorList.Add(Colors.MintCream);
            colorList.Add(Colors.MistyRose);
            colorList.Add(Colors.Moccasin);
            colorList.Add(Colors.NavajoWhite);
            colorList.Add(Colors.Navy);
            colorList.Add(Colors.OldLace);
            colorList.Add(Colors.Olive);
            colorList.Add(Colors.OliveDrab);
            colorList.Add(Colors.Orange);
            colorList.Add(Colors.OrangeRed);
            colorList.Add(Colors.Orchid);
            colorList.Add(Colors.PaleGoldenrod);
            colorList.Add(Colors.PaleGreen);
            colorList.Add(Colors.PaleTurquoise);
            colorList.Add(Colors.PaleVioletRed);
            colorList.Add(Colors.PapayaWhip);
            colorList.Add(Colors.PeachPuff);
            colorList.Add(Colors.Peru);
            colorList.Add(Colors.Pink);
            colorList.Add(Colors.Plum);
            colorList.Add(Colors.PowderBlue);
            colorList.Add(Colors.Purple);
            colorList.Add(Colors.Red);
            colorList.Add(Colors.RosyBrown);
            colorList.Add(Colors.RoyalBlue);
            colorList.Add(Colors.SaddleBrown);
            colorList.Add(Colors.Salmon);
            colorList.Add(Colors.SandyBrown);
            colorList.Add(Colors.SeaGreen);
            colorList.Add(Colors.SeaShell);
            colorList.Add(Colors.Sienna);
            colorList.Add(Colors.Silver);
            colorList.Add(Colors.SkyBlue);
            colorList.Add(Colors.SlateBlue);
            colorList.Add(Colors.SlateGray);
            colorList.Add(Colors.Snow);
            colorList.Add(Colors.SpringGreen);
            colorList.Add(Colors.SteelBlue);
            colorList.Add(Colors.Tan);
            colorList.Add(Colors.Teal);
            colorList.Add(Colors.Thistle);
            colorList.Add(Colors.Tomato);
            colorList.Add(Colors.Transparent);
            colorList.Add(Colors.Turquoise);
            colorList.Add(Colors.Violet);
            colorList.Add(Colors.Wheat);
            colorList.Add(Colors.White);
            colorList.Add(Colors.WhiteSmoke);
            colorList.Add(Colors.Yellow);
            colorList.Add(Colors.YellowGreen);

            //maxColorIndex = colorList.Count;


            darkColorList.Add(Colors.MediumBlue);
            darkColorList.Add(Colors.Tomato);
            darkColorList.Add(Colors.Green);
            darkColorList.Add(Colors.Yellow);

            darkColorList.Add(Colors.SteelBlue);
            darkColorList.Add(Colors.SlateBlue);
            darkColorList.Add(Colors.SeaGreen);
            darkColorList.Add(Colors.PeachPuff);

            darkColorList.Add(Colors.MediumAquamarine);
            darkColorList.Add(Colors.MediumOrchid);
            darkColorList.Add(Colors.MediumPurple);
            darkColorList.Add(Colors.MediumSeaGreen);
            darkColorList.Add(Colors.MediumSlateBlue);
            darkColorList.Add(Colors.MediumSpringGreen);
            darkColorList.Add(Colors.MediumTurquoise);
            darkColorList.Add(Colors.MediumVioletRed);

            darkColorList.Add(Colors.DarkBlue);
            darkColorList.Add(Colors.DarkCyan);
            darkColorList.Add(Colors.DarkGoldenrod);
            darkColorList.Add(Colors.DarkGray);
            darkColorList.Add(Colors.DarkGreen);
            darkColorList.Add(Colors.DarkKhaki);
            darkColorList.Add(Colors.DarkMagenta);
            darkColorList.Add(Colors.DarkOliveGreen);
            darkColorList.Add(Colors.DarkOrange);
            darkColorList.Add(Colors.DarkOrchid);
            darkColorList.Add(Colors.DarkRed);
            darkColorList.Add(Colors.DarkSalmon);
            darkColorList.Add(Colors.DarkSeaGreen);
            darkColorList.Add(Colors.DarkSlateBlue);
            darkColorList.Add(Colors.DarkSlateGray);
            darkColorList.Add(Colors.DarkTurquoise);
            darkColorList.Add(Colors.DarkViolet);

        }

        public static int GetColorArrayLength()
        {
            return colorList.Count;
        }

        public static Color GetColorByIndex(int index)
        {
            return colorList[index];
        }


        public static Color GetDarkColorByIndex(int index)
        {
            if (index >= darkColorList.Count)
                index = new Random(Environment.TickCount).Next(0, darkColorList.Count);
            return darkColorList[index];
        }



        public static Color GetDarkColorByAI(int index, string label)
        {
            if (index >= darkColorList.Count)
                index = new Random(Environment.TickCount).Next(0, darkColorList.Count - 1);

            if (addedList.ContainsKey(label))
                return addedList[label];

            addedList[label] = darkColorList[index];

            return darkColorList[index];
        }


        public static Color GetColorBySeed(int seed)
        {
            Random rnd = new Random(seed);
            rnd.Next(Environment.TickCount);

            return colorList[rnd.Next(0, 139)];
        }


        public static byte GetRandomColorByte(byte min, byte max)
        {
            Random rnd = new Random();
            byte b = Convert.ToByte(rnd.Next(min, max));
            return b;
        }


        public static Color GetRandomColor()
        {
            int index;
            lock (lockObject)
            {
                Random rnd = new Random(Environment.TickCount);
                index = rnd.Next(0, 139);

            }
            return colorList[index];

        }

    }

    public class Parser
    {
        #region Static Methods


        /// <summary>
        /// Returns a darker shade of the color by decreasing the brightness by the given intensity value
        /// </summary>
        /// <param name="color"></param>
        /// <param name="intensity"></param>
        /// <returns></returns>
        public static System.Windows.Media.Color GetDarkerColor(System.Windows.Media.Color color, Double intensity)
        {
            Color darkerShade = new Color();
            intensity = (intensity < 0 || intensity > 1) ? 1 : intensity;
            darkerShade.R = (Byte)(color.R * intensity);
            darkerShade.G = (Byte)(color.G * intensity);
            darkerShade.B = (Byte)(color.B * intensity);
            darkerShade.A = 255;
            return darkerShade;
        }
        public static System.Windows.Media.Color GetDarkerColor(System.Windows.Media.Color color, Double intensityR, Double intensityG, Double intensityB)
        {
            Color darkerShade = new Color();
            intensityR = (intensityR < 0 || intensityR > 1) ? 1 : intensityR;
            intensityG = (intensityG < 0 || intensityG > 1) ? 1 : intensityG;
            intensityB = (intensityB < 0 || intensityB > 1) ? 1 : intensityB;
            darkerShade.R = (Byte)(color.R * intensityR);
            darkerShade.G = (Byte)(color.G * intensityG);
            darkerShade.B = (Byte)(color.B * intensityB);
            darkerShade.A = 255;
            return darkerShade;
        }
        /// <summary>
        /// Returns a lighter shade of the color by increasing the brightness by the given intensity value
        /// </summary>
        /// <param name="color"></param>
        /// <param name="intensity"></param>
        /// <returns></returns>
        public static System.Windows.Media.Color GetLighterColor(System.Windows.Media.Color color, Double intensity)
        {
            Color lighterShade = new Color();
            intensity = (intensity < 0 || intensity > 1) ? 1 : intensity;
            lighterShade.R = (Byte)(256 - ((256 - color.R) * intensity));
            lighterShade.G = (Byte)(256 - ((256 - color.G) * intensity));
            lighterShade.B = (Byte)(256 - ((256 - color.B) * intensity));
            lighterShade.A = 255;
            return lighterShade;
        }
        public static System.Windows.Media.Color GetLighterColor(System.Windows.Media.Color color, Double intensityR, Double intensityG, Double intensityB)
        {
            Color lighterShade = new Color();
            intensityR = (intensityR < 0 || intensityR > 1) ? 1 : intensityR;
            intensityG = (intensityG < 0 || intensityG > 1) ? 1 : intensityG;
            intensityB = (intensityB < 0 || intensityB > 1) ? 1 : intensityB;
            lighterShade.R = (Byte)(256 - ((256 - color.R) * intensityR));
            lighterShade.G = (Byte)(256 - ((256 - color.G) * intensityG));
            lighterShade.B = (Byte)(256 - ((256 - color.B) * intensityB));
            lighterShade.A = 255;
            return lighterShade;
        }

        public static void GenerateDarkerGradientBrush(GradientBrush source, GradientBrush result, Double intensity)
        {
            foreach (GradientStop grad in source.GradientStops)
            {
                GradientStop gs = new GradientStop();
                gs.Color = Parser.GetDarkerColor(grad.Color, intensity);
                gs.Offset = grad.Offset;
                result.GradientStops.Add(gs);
            }
        }

        public static void GenerateLighterGradientBrush(GradientBrush source, GradientBrush result, Double intensity)
        {
            foreach (GradientStop grad in source.GradientStops)
            {
                GradientStop gs = new GradientStop();
                gs.Color = Parser.GetLighterColor(grad.Color, intensity);
                gs.Offset = grad.Offset;
                result.GradientStops.Add(gs);
            }
        }

        public static System.Windows.Media.Color InvertColor(System.Windows.Media.Color color)
        {
            Color newColor = new Color();
            newColor.A = 255;
            newColor.R = (Byte)(255 - color.R);
            newColor.G = (Byte)(255 - color.G);
            newColor.B = (Byte)(255 - color.B);
            return newColor;
        }

        public static Double GetBrushIntensity(Brush brush)
        {
            Color color = new Color();
            Double intensity = 0;
            if (brush == null) return 1;
            if (brush.GetType().Name == "SolidColorBrush")
            {
                color = (brush as SolidColorBrush).Color;
                intensity = (Double)(color.R + color.G + color.B) / (3 * 255);

            }
            else if (brush.GetType().Name == "LinearGradientBrush" || brush.GetType().Name == "RadialGradientBrush")
            {

                foreach (GradientStop grad in (brush as GradientBrush).GradientStops)
                {
                    color = grad.Color;
                    intensity += (Double)(color.R + color.G + color.B) / (3 * 255);
                }
                intensity /= (brush as GradientBrush).GradientStops.Count;
            }
            else
            {
                intensity = 0;
            }
            return intensity;
        }
        #endregion Static Methods

    }
}
