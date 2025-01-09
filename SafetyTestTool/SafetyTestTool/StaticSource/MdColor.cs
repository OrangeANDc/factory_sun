using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using MaterialDesignColors.ColorManipulation;
using System.Windows.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafetyTestTool.StaticSource
{
    public static class MdColor
    {
        public static void SetThemeColor(string colorCode)
        {
            PaletteHelper _paletteHelper = new PaletteHelper();

            var color = (Color)System.Windows.Media.ColorConverter.ConvertFromString(colorCode); ;
            Theme theme = (Theme)_paletteHelper.GetTheme();
            theme.PrimaryLight = new ColorPair(color.Lighten());
            theme.PrimaryMid = new ColorPair(color);
            theme.PrimaryDark = new ColorPair(color.Darken());

            _paletteHelper.SetTheme(theme);
        }
    }
}
