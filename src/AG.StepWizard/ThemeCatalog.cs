using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;

namespace AG.StepWizard
{
    /// <summary>
    /// Provides the built-in token catalog for AG.StepWizard.
    /// </summary>
    public static class ThemeCatalog
    {
        public static IEnumerable<StepWizardTheme> BuiltInThemes
        {
            get
            {
                yield return Light;
                yield return System;
                yield return Dark;
                yield return OLEDBlack;
                yield return BlueDark;
                yield return CatppuccinLatte;
                yield return CatppuccinFrappe;
                yield return CatppuccinMacchiato;
                yield return CatppuccinMocha;
                yield return SolarizedLight;
                yield return SolarizedDark;
                yield return Monokai;
                yield return OneDark;
                yield return Matrix;
                yield return Dracula;
                yield return Nord;
                yield return GruvboxLight;
                yield return GruvboxDark;
                yield return TokyoNight;
                yield return GitHubLight;
                yield return GitHubDark;
                yield return VSCodeDarkPlus;
                yield return VisualStudioBlue;
                yield return VisualStudioDark;
                yield return FluentLight;
                yield return FluentDark;
                yield return WindowsClassic;
                yield return HighContrast;
            }
        }

        public static StepWizardTheme Light
        {
            get
            {
                return Create("Light", false, C(248, 250, 252), Color.White, Color.White, C(241, 245, 249), Color.White, C(203, 213, 225), C(15, 23, 42), C(71, 85, 105), C(37, 99, 235), Color.White, C(239, 246, 255), C(219, 234, 254), C(100, 116, 139), C(22, 163, 74), C(217, 119, 6), C(220, 38, 38));
            }
        }

        public static StepWizardTheme System
        {
            get
            {
                if (SystemInformation.HighContrast)
                {
                    return HighContrast;
                }

                return IsWindowsAppsDarkMode() ? Dark : Light;
            }
        }

        public static StepWizardTheme Dark
        {
            get
            {
                return Create("Dark", true, C(15, 23, 42), C(17, 24, 39), C(15, 23, 42), C(2, 6, 23), C(30, 41, 59), C(51, 65, 85), C(248, 250, 252), C(148, 163, 184), C(96, 165, 250), C(2, 6, 23), C(51, 65, 85), C(30, 41, 59), C(100, 116, 139), C(34, 197, 94), C(251, 191, 36), C(248, 113, 113));
            }
        }

        public static StepWizardTheme OLEDBlack { get { return Create("OLED Black", true, Color.Black, Color.Black, Color.Black, Color.Black, C(10, 10, 10), C(56, 56, 56), C(245, 245, 245), C(160, 160, 160), C(0, 200, 255), Color.Black, C(20, 20, 20), C(26, 26, 26), C(110, 110, 110), C(0, 230, 118), C(255, 213, 79), C(255, 82, 82)); } }
        public static StepWizardTheme BlueDark { get { return Create("Blue Dark", true, C(8, 18, 36), C(10, 25, 49), C(7, 16, 32), C(4, 12, 27), C(12, 48, 95), C(30, 64, 111), C(232, 244, 255), C(164, 196, 230), C(56, 189, 248), C(4, 12, 27), C(18, 72, 132), C(12, 48, 95), C(105, 139, 174), C(56, 189, 248), C(250, 204, 21), C(251, 113, 133)); } }
        public static StepWizardTheme CatppuccinLatte { get { return Create("Catppuccin Latte", false, C(239, 241, 245), C(250, 251, 252), C(250, 251, 252), C(230, 233, 239), C(239, 241, 245), C(188, 192, 204), C(76, 79, 105), C(108, 111, 133), C(136, 57, 239), Color.White, C(220, 224, 232), C(228, 233, 244), C(124, 127, 147), C(64, 160, 43), C(223, 142, 29), C(210, 15, 57)); } }
        public static StepWizardTheme CatppuccinFrappe { get { return Create("Catppuccin Frappe", true, C(35, 38, 52), C(41, 44, 60), C(35, 38, 52), C(30, 32, 48), C(65, 69, 89), C(81, 87, 109), C(198, 208, 245), C(165, 173, 206), C(202, 158, 230), C(35, 38, 52), C(65, 69, 89), C(73, 77, 100), C(131, 139, 167), C(166, 209, 137), C(229, 200, 144), C(231, 130, 132)); } }
        public static StepWizardTheme CatppuccinMacchiato { get { return Create("Catppuccin Macchiato", true, C(30, 32, 48), C(36, 39, 58), C(30, 32, 48), C(24, 25, 38), C(54, 58, 79), C(73, 77, 100), C(202, 211, 245), C(165, 173, 203), C(198, 160, 246), C(30, 32, 48), C(54, 58, 79), C(64, 69, 89), C(128, 135, 162), C(166, 218, 149), C(238, 212, 159), C(237, 135, 150)); } }
        public static StepWizardTheme CatppuccinMocha { get { return Create("Catppuccin Mocha", true, C(17, 17, 27), C(24, 24, 37), C(24, 24, 37), C(17, 17, 27), C(49, 50, 68), C(69, 71, 90), C(205, 214, 244), C(166, 173, 200), C(203, 166, 247), C(17, 17, 27), C(49, 50, 68), C(58, 59, 77), C(127, 132, 156), C(166, 227, 161), C(249, 226, 175), C(243, 139, 168)); } }
        public static StepWizardTheme SolarizedLight { get { return Create("Solarized Light", false, C(253, 246, 227), C(255, 251, 235), C(253, 246, 227), C(238, 232, 213), C(246, 239, 219), C(147, 161, 161), C(88, 110, 117), C(101, 123, 131), C(38, 139, 210), Color.White, C(238, 232, 213), C(222, 216, 196), C(131, 148, 150), C(133, 153, 0), C(181, 137, 0), C(220, 50, 47)); } }
        public static StepWizardTheme SolarizedDark { get { return Create("Solarized Dark", true, C(0, 43, 54), C(7, 54, 66), C(0, 43, 54), C(0, 35, 44), C(21, 78, 92), C(88, 110, 117), C(238, 232, 213), C(147, 161, 161), C(38, 139, 210), C(0, 43, 54), C(21, 78, 92), C(12, 67, 81), C(101, 123, 131), C(133, 153, 0), C(181, 137, 0), C(220, 50, 47)); } }
        public static StepWizardTheme Linear { get { return Create("Linear", true, C(8, 8, 10), C(15, 15, 18), C(8, 8, 10), C(10, 10, 12), C(35, 35, 42), C(52, 52, 62), C(245, 245, 247), C(163, 163, 172), C(94, 106, 210), Color.White, C(35, 35, 42), C(42, 42, 52), C(120, 120, 130), C(74, 222, 128), C(250, 204, 21), C(248, 113, 113)); } }
        public static StepWizardTheme Notion { get { return Create("Notion", false, C(247, 246, 243), Color.White, Color.White, C(241, 241, 239), Color.White, C(218, 217, 214), C(55, 53, 47), C(120, 119, 116), C(35, 131, 226), Color.White, C(232, 231, 228), C(238, 238, 236), C(150, 148, 145), C(15, 123, 108), C(217, 115, 13), C(224, 62, 62)); } }
        public static StepWizardTheme OpenClaw { get { return Create("OpenClaw", true, C(13, 18, 25), C(17, 24, 34), C(13, 18, 25), C(9, 13, 20), C(32, 45, 62), C(51, 65, 85), C(229, 237, 245), C(148, 163, 184), C(249, 115, 22), Color.White, C(32, 45, 62), C(45, 56, 72), C(107, 114, 128), C(34, 197, 94), C(251, 191, 36), C(239, 68, 68)); } }
        public static StepWizardTheme Monokai { get { return Create("Monokai", true, C(39, 40, 34), C(45, 46, 39), C(39, 40, 34), C(30, 31, 27), C(73, 72, 62), C(92, 92, 80), C(248, 248, 242), C(174, 173, 168), C(166, 226, 46), C(39, 40, 34), C(73, 72, 62), C(84, 82, 70), C(117, 113, 94), C(166, 226, 46), C(253, 151, 31), C(249, 38, 114)); } }
        public static StepWizardTheme OneDark { get { return Create("One Dark", true, C(33, 37, 43), C(40, 44, 52), C(33, 37, 43), C(28, 31, 36), C(49, 56, 66), C(75, 82, 97), C(220, 223, 228), C(171, 178, 191), C(97, 175, 239), C(33, 37, 43), C(49, 56, 66), C(57, 64, 75), C(120, 128, 143), C(152, 195, 121), C(229, 192, 123), C(224, 108, 117)); } }
        public static StepWizardTheme Matrix { get { return Create("Matrix", true, Color.Black, C(0, 12, 4), Color.Black, Color.Black, C(0, 42, 12), C(0, 92, 24), C(180, 255, 190), C(67, 160, 71), C(0, 255, 65), Color.Black, C(0, 42, 12), C(0, 58, 18), C(42, 120, 52), C(0, 255, 65), C(180, 255, 0), C(255, 48, 48)); } }
        public static StepWizardTheme Dracula { get { return Create("Dracula", true, C(40, 42, 54), C(45, 47, 61), C(40, 42, 54), C(33, 34, 44), C(68, 71, 90), C(98, 114, 164), C(248, 248, 242), C(189, 147, 249), C(255, 121, 198), C(40, 42, 54), C(68, 71, 90), C(76, 79, 99), C(120, 124, 154), C(80, 250, 123), C(241, 250, 140), C(255, 85, 85)); } }
        public static StepWizardTheme Nord { get { return Create("Nord", true, C(46, 52, 64), C(59, 66, 82), C(46, 52, 64), C(36, 41, 51), C(67, 76, 94), C(76, 86, 106), C(236, 239, 244), C(216, 222, 233), C(136, 192, 208), C(46, 52, 64), C(67, 76, 94), C(78, 87, 106), C(143, 155, 179), C(163, 190, 140), C(235, 203, 139), C(191, 97, 106)); } }
        public static StepWizardTheme GruvboxLight { get { return Create("Gruvbox Light", false, C(251, 241, 199), C(253, 244, 203), C(251, 241, 199), C(235, 219, 178), C(242, 229, 188), C(168, 153, 132), C(60, 56, 54), C(124, 111, 100), C(181, 118, 20), Color.White, C(213, 196, 161), C(225, 204, 166), C(146, 131, 116), C(121, 116, 14), C(181, 118, 20), C(157, 0, 6)); } }
        public static StepWizardTheme GruvboxDark { get { return Create("Gruvbox Dark", true, C(40, 40, 40), C(50, 48, 47), C(40, 40, 40), C(29, 32, 33), C(60, 56, 54), C(80, 73, 69), C(235, 219, 178), C(168, 153, 132), C(250, 189, 47), C(40, 40, 40), C(60, 56, 54), C(70, 65, 60), C(146, 131, 116), C(184, 187, 38), C(250, 189, 47), C(251, 73, 52)); } }
        public static StepWizardTheme TokyoNight { get { return Create("Tokyo Night", true, C(26, 27, 38), C(31, 35, 53), C(26, 27, 38), C(22, 22, 30), C(41, 46, 66), C(65, 72, 104), C(192, 202, 245), C(154, 165, 206), C(122, 162, 247), C(26, 27, 38), C(41, 46, 66), C(49, 55, 78), C(112, 122, 162), C(158, 206, 106), C(224, 175, 104), C(247, 118, 142)); } }
        public static StepWizardTheme GitHubLight { get { return Create("GitHub Light", false, C(246, 248, 250), Color.White, Color.White, C(246, 248, 250), Color.White, C(208, 215, 222), C(31, 35, 40), C(87, 96, 106), C(9, 105, 218), Color.White, C(221, 244, 255), C(221, 244, 255), C(87, 96, 106), C(26, 127, 55), C(154, 103, 0), C(207, 34, 46)); } }
        public static StepWizardTheme GitHubDark { get { return Create("GitHub Dark", true, C(13, 17, 23), C(22, 27, 34), C(13, 17, 23), C(1, 4, 9), C(33, 38, 45), C(48, 54, 61), C(230, 237, 243), C(125, 133, 144), C(47, 129, 247), C(13, 17, 23), C(33, 38, 45), C(38, 44, 53), C(110, 118, 129), C(63, 185, 80), C(210, 153, 34), C(248, 81, 73)); } }
        public static StepWizardTheme VSCodeDarkPlus { get { return Create("VS Code Dark+", true, C(30, 30, 30), C(37, 37, 38), C(30, 30, 30), C(24, 24, 24), C(43, 45, 46), C(63, 63, 70), C(212, 212, 212), C(156, 156, 156), C(0, 122, 204), Color.White, C(43, 45, 46), C(55, 65, 81), C(117, 117, 117), C(106, 153, 85), C(220, 220, 170), C(244, 71, 71)); } }
        public static StepWizardTheme VisualStudioBlue { get { return Create("Visual Studio Blue", false, C(238, 242, 249), Color.White, C(245, 248, 252), C(217, 226, 242), Color.White, C(155, 174, 205), C(30, 45, 70), C(83, 96, 120), C(0, 122, 204), Color.White, C(201, 218, 246), C(201, 218, 246), C(105, 118, 140), C(16, 124, 16), C(202, 80, 16), C(196, 43, 28)); } }
        public static StepWizardTheme VisualStudioDark { get { return Create("Visual Studio Dark", true, C(30, 30, 30), C(37, 37, 38), C(30, 30, 30), C(45, 45, 48), C(62, 62, 66), C(80, 80, 80), C(241, 241, 241), C(190, 190, 190), C(0, 122, 204), Color.White, C(62, 62, 66), C(51, 80, 110), C(130, 130, 130), C(137, 209, 133), C(255, 204, 102), C(241, 76, 76)); } }
        public static StepWizardTheme FluentLight { get { return Create("Fluent Light", false, C(243, 243, 243), Color.White, Color.White, C(250, 250, 250), Color.White, C(200, 200, 200), C(32, 32, 32), C(96, 96, 96), C(0, 120, 212), Color.White, C(230, 240, 255), C(230, 240, 255), C(117, 117, 117), C(16, 124, 16), C(255, 185, 0), C(196, 43, 28)); } }
        public static StepWizardTheme FluentDark { get { return Create("Fluent Dark", true, C(32, 32, 32), C(40, 40, 40), C(32, 32, 32), C(27, 27, 27), C(54, 54, 54), C(80, 80, 80), C(243, 243, 243), C(200, 200, 200), C(96, 205, 255), C(32, 32, 32), C(54, 54, 54), C(62, 62, 62), C(140, 140, 140), C(107, 203, 119), C(255, 213, 79), C(255, 99, 99)); } }
        public static StepWizardTheme WindowsClassic { get { return Create("Windows Classic", false, SystemColors.Control, SystemColors.Window, SystemColors.Control, SystemColors.Control, SystemColors.Window, SystemColors.ControlDark, SystemColors.ControlText, SystemColors.GrayText, SystemColors.Highlight, SystemColors.HighlightText, SystemColors.ActiveCaption, SystemColors.ActiveCaption, SystemColors.GrayText, C(0, 128, 0), C(255, 191, 0), C(192, 0, 0)); } }
        public static StepWizardTheme HighContrast { get { return Create("High Contrast", false, SystemColors.Window, SystemColors.Window, SystemColors.Window, SystemColors.Window, SystemColors.Window, SystemColors.WindowText, SystemColors.WindowText, SystemColors.GrayText, SystemColors.Highlight, SystemColors.HighlightText, SystemColors.Highlight, SystemColors.Highlight, SystemColors.GrayText, SystemColors.Highlight, SystemColors.HotTrack, C(255, 0, 0)); } }

        private static StepWizardTheme Create(
            string name,
            bool isDark,
            Color windowBack,
            Color contentBack,
            Color headerBack,
            Color sidebarBack,
            Color cardBack,
            Color border,
            Color text,
            Color mutedText,
            Color accent,
            Color accentText,
            Color hoverBack,
            Color selectedBack,
            Color disabledText,
            Color success,
            Color warning,
            Color error)
        {
            return new StepWizardTheme
            {
                Name = name,
                IsDark = isDark,
                WindowBack = windowBack,
                ContentBack = contentBack,
                HeaderBack = headerBack,
                SidebarBack = sidebarBack,
                CardBack = cardBack,
                Border = border,
                Text = text,
                MutedText = mutedText,
                Accent = accent,
                AccentText = accentText,
                HoverBack = hoverBack,
                SelectedBack = selectedBack,
                DisabledText = disabledText,
                Success = success,
                Warning = warning,
                Error = error
            };
        }

        private static Color C(int r, int g, int b)
        {
            return Color.FromArgb(r, g, b);
        }

        private static bool IsWindowsAppsDarkMode()
        {
            try
            {
                using (var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"))
                {
                    var value = key == null ? null : key.GetValue("AppsUseLightTheme");
                    return value is int && (int)value == 0;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
