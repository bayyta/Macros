using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using Newtonsoft.Json;
using project.main.json;
using Newtonsoft.Json.Linq;
using System.Windows.Media;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace project.main
{
    class Macro
    {
        [JsonIgnore]
        public Border Parent { get; set; }
        [JsonProperty(PropertyName = Constants.s_macro_name)]
        public string Name { get; set; }
        [JsonProperty(PropertyName = Constants.s_macro_hotkey)]
        public int Hotkey { get; set; }
        private bool selected { get; set; }
        private bool loaded { get; set; }

        [JsonProperty(PropertyName = Constants.s_action_list)]
        public List<ActionObject> actionList = new List<ActionObject>();

        public Macro(UIElement parent, string name)
        {
            Parent = parent as Border;
            Name = name;
            selected = false;
        }

        public void SetName(string name)
        {
            string path = Path.Combine(Constants.file_path, Name + ".json");
            string newPath = Path.Combine(Constants.file_path, name + ".json");
            if (File.Exists(path))
                File.Move(path, newPath);
            Name = name;
        }

        public bool isLoaded()
        {
            return loaded;
        }

        public void Load()
        {
            loaded = true;
        }

        public bool isSelected()
        {
            return selected;
        }

        public void Select()
        {
            selected = true;
            Parent.Background = (Brush)Application.Current.Resources["accent_color"];
        }

        public void Deselect()
        {
            selected = false;
            Parent.Background = (Brush)Application.Current.Resources["dark_color"];
        }

        public void Highlight()
        {
            if (!selected)
                Parent.Background = (Brush)Application.Current.Resources["light_color"];
        }

        public void Unhighlight()
        {
            if (!selected)
                Parent.Background = (Brush)Application.Current.Resources["dark_color"];
        }

        public void EditName()
        {
            TextBox tb = GetTextNameBox();
            tb.IsReadOnly = false;
            tb.IsHitTestVisible = true;
            tb.Focus();
            tb.SelectAll();
            tb.CaretIndex = tb.Text.Length;
            tb.Background = (Brush)Application.Current.Resources["dark_accent_color"];
        }

        public void EditNameDone()
        {
            TextBox tb = GetTextNameBox();
            System.Windows.Input.Keyboard.ClearFocus();
            tb.IsReadOnly = true;
            tb.IsHitTestVisible = false;
            tb.Background = null;
            SetName(tb.Text);
        }

        public void EditNameCanceled()
        {
            TextBox tb = GetTextNameBox();
            System.Windows.Input.Keyboard.ClearFocus();
            tb.IsReadOnly = true;
            tb.IsHitTestVisible = false;
            tb.Background = null;
            tb.Text = Name;
        }

        public TextBox GetTextNameBox()
        {
            return ((Parent.Child as Grid).Children[0] as DockPanel).Children[1] as TextBox;
        }

    }
}
