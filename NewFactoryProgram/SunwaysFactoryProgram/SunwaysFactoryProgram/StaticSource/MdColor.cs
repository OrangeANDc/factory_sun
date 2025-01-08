using MaterialDesignColors;
using MaterialDesignColors.ColorManipulation;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SunwaysFactoryProgram.StaticSource
{
    public static class MdColor
    {
        public static void SetThemeColor(string colorCode)
        {
            PaletteHelper _paletteHelper = new PaletteHelper();

            var color = (Color)System.Windows.Media.ColorConverter.ConvertFromString(colorCode); ;
            Theme theme = _paletteHelper.GetTheme();
            theme.PrimaryLight = new ColorPair(color.Lighten());
            theme.PrimaryMid = new ColorPair(color);
            theme.PrimaryDark = new ColorPair(color.Darken());

            _paletteHelper.SetTheme(theme);
        }
    }
}
