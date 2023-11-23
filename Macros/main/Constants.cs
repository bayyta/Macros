using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace project.main.json
{
    public static class Constants
    {
        public static void Init()
        {
            action_names.Sort();
            mouse_buttons_down.OrderBy(pair => pair.Key);
            mouse_buttons_up.OrderBy(pair => pair.Key);
            command_buttons.OrderBy(pair => pair.Key);
        }

        public static string file_path = Path.Combine(Environment.CurrentDirectory, "macros");

        public const string close_program = "close_program";
        public const string copy_text = "copy_text";
        public const string delay = "delay";
        public const string key_click = "key_click";
        public const string key_down = "key_down";
        public const string open_program = "open_program";
        public const string open_url = "open_url";
        public const string key_up = "key_up";
        public const string mouse_click = "mouse_click";
        public const string mouse_move = "mouse_move";
        public const string mouse_up = "mouse_up";
        public const string mouse_down = "mouse_down";
        public const string type_text = "type_text";

        public static SortedList<string, int> mouse_buttons_up = new SortedList<string, int>
        {
            { s_left, MOUSEEVENTF_LEFTUP },
            { s_right, MOUSEEVENTF_RIGHTUP },
            { s_wheel, MOUSEEVENTF_MIDDLEUP },
            { s_b4, MOUSEEVENTF_XUP },
            { s_b5, MOUSEEVENTF_XUP }
        };
        public static SortedList<string, int> mouse_buttons_down = new SortedList<string, int>
        {
            { s_left, MOUSEEVENTF_LEFTDOWN },
            { s_right, MOUSEEVENTF_RIGHTDOWN },
            { s_wheel, MOUSEEVENTF_MIDDLEDOWN },
            { s_b4, MOUSEEVENTF_XDOWN },
            { s_b5, MOUSEEVENTF_XDOWN }
        };
        public static SortedList<string, byte> command_buttons = new SortedList<string, byte>
        {
            { "Delete", (byte)Keys.Delete},
            { "Left alt", (byte)Keys.LeftAlt},
            { "Left shift", (byte)Keys.LeftShift},
            { "Right alt", (byte)Keys.RightAlt},
            { "Right shift", (byte)Keys.RightShift},
            { "Left control", (byte)Keys.LeftControl},
            { "Right control", (byte)Keys.RightControl},
            { "Caps lock", (byte)Keys.CapsLock},
            { "Tab", (byte)Keys.Tab},
            { "Windows key", (byte)Keys.LeftWindows},
            { "Enter", (byte)Keys.Enter},
            { "End", (byte)Keys.End},
            { "Home", (byte)Keys.Home},
            { "Page down", (byte)Keys.PageDown},
            { "Page up", (byte)Keys.PageUp},
            { "Print screen", (byte)Keys.PrintScreen},
            { "Play / Pause", (byte)Keys.MediaPlayPause},
            { "Next track", (byte)Keys.MediaNextTrack},
            { "Previous track", (byte)Keys.MediaPreviousTrack},
            { "Stop", (byte)Keys.MediaStop},
            { "Num lock", (byte)Keys.NumLock},
            { "F1", (byte) Keys.F1},
            { "F2", (byte)Keys.F2},
            { "F3", (byte)Keys.F3},
            { "F4", (byte)Keys.F4},
            { "F5", (byte)Keys.F5},
            { "F6", (byte)Keys.F6},
            { "F7", (byte)Keys.F7},
            { "F8", (byte)Keys.F8},
            { "F9", (byte)Keys.F9},
            { "F10", (byte)Keys.F10},
            { "F11", (byte)Keys.F11},
            { "F12", (byte)Keys.F12},
            { "Insert", (byte)Keys.Insert},
            { "Escape", (byte)Keys.Escape},
            { "Left", (byte)Keys.Left},
            { "Right", (byte)Keys.Right},
            { "Up", (byte)Keys.Up},
            { "Down", (byte)Keys.Down},
            { "Volume mute", (byte)Keys.VolumeMute},
            { "Volume up", (byte)Keys.VolumeUp},
            { "Volume down", (byte)Keys.VolumeDown}
        };
        public static List<string> action_names = new List<string>
        {
            s_mouse_down,
            s_mouse_up,
            s_mouse_click,
            s_mouse_move,
            s_key_down,
            s_key_up,
            s_key_click,
            s_open_url,
            s_open_program,
            s_close_program,
            s_delay,
            s_copy_text,
            s_type_text
        };

        public const string s_action_list = "Actions";
        public const string s_macro_name = "Name";
        public const string s_macro_hotkey = "Hotkey";

        public const string s_mouse_down = "Mouse button down";
        public const string s_mouse_up = "Mouse button up";
        public const string s_mouse_click = "Mouse click";
        public const string s_mouse_move = "Mouse move";
        public const string s_key_down = "Key down";
        public const string s_key_up = "Key up";
        public const string s_key_click = "Key click";
        public const string s_open_url = "Open url";
        public const string s_open_program = "Open program";
        public const string s_close_program = "Close program";
        public const string s_delay = "Delay";
        public const string s_copy_text = "Copy text to clipboard";
        public const string s_type_text = "Type text";
        
        public const string s_left = "Button left";
        public const string s_right = "Button right";
        public const string s_wheel = "Mouse wheel";
        public const string s_scroll_up = "Scroll up";
        public const string s_scroll_down = "Scroll down";
        public const string s_b4 = "Side button 1";
        public const string s_b5 = "Side button 2";

        public const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        public const int MOUSEEVENTF_LEFTUP = 0x0004;
        public const int MOUSEEVENTF_MIDDLEUP = 0x0040;
        public const int MOUSEEVENTF_RIGHTUP = 0x0010;
        public const int MOUSEEVENTF_XUP = 0x0100;

        public const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        public const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        public const int MOUSEEVENTF_XDOWN = 0x0080;

        public const int MOUSEEVENTF_WHEEL = 0x0800;
    }
}
