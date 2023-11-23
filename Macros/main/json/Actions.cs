using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Input;

namespace project.main.json
{
    class MacroObject
    {
        public string name { get; set; }
        public int hotkey { set; get; }
        public List<ActionObject> actions = new List<ActionObject>();
    }

    abstract class ActionObject
    {
        [JsonIgnore]
        public abstract string name { get; }
        public abstract string type { get; }
        public abstract void Execute();

        private Grid grid_ClearFocus = null;
        protected int l_width = 170, cb_width = 200;

        public void Init(Grid clearFocus)
        {
            grid_ClearFocus = clearFocus;
        }

        protected void ClearFocus()
        {
            grid_ClearFocus.Focus();
        }
        public abstract void CreateUI(Grid grid);
        protected void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ClearFocus();
            }
        }
        protected void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            tb.HorizontalContentAlignment = HorizontalAlignment.Center;
        }
        protected void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            tb.HorizontalContentAlignment = HorizontalAlignment.Left;
        }
        protected void TextBox_Max_1_Changed(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb.Text.Length == 0) return;
            string s = tb.Text;
            tb.Text = s.Substring(s.Length - 1);
            tb.CaretIndex = tb.Text.Length;
        }
    }

    class CloseProgramObject : ActionObject
    {
        public override string name
        {
            get { return Constants.s_close_program; }
        }

        public override string type
        {
            get { return Constants.close_program; }
        }

        [JsonProperty(PropertyName = Constants.close_program)]
        public CloseProgram close_program = new CloseProgram();
        public class CloseProgram
        {
            public string process { get; set; }
        }

        public override void Execute()
        {
            //Console.WriteLine(copy_text.text + " HELLAOEKSOD");
        }

        public override void CreateUI(Grid grid)
        {
            grid.ColumnDefinitions.Add(CustomUIElements.CreateColumnDefinition(1, GridUnitType.Star));
            grid.ColumnDefinitions.Add(CustomUIElements.CreateColumnDefinition(30, GridUnitType.Pixel));

            DockPanel dp = CustomUIElements.CreateDockPanel(1, 0);

            Label l1 = CustomUIElements.CreateLabel("Process name:", 12, (Brush)Application.Current.Resources["font_color"]);
            DockPanel.SetDock(l1, Dock.Left);

            dp.Children.Add(l1);
            dp.Children.Add(CustomUIElements.CreateStandardBackgroundBorder(CustomUIElements.CreateActionTextBox(close_program.process, TextBox_KeyDown, TextBox_LostFocus, TextBox_GotFocus)));
            grid.Children.Add(dp);
        }

        private void LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox_LostFocus(sender, e);
            close_program.process = (sender as TextBox).Text;
        }
    }

    class CopyTextObject : ActionObject
    {
        public override string name
        {
            get { return Constants.s_copy_text; }
        }

        public override string type
        {
            get { return Constants.copy_text; }
        }

        [JsonProperty(PropertyName = Constants.copy_text)]
        public CopyText copy_text = new CopyText();
        public class CopyText
        {
            public string text { get; set; }
        }

        public override void Execute()
        {
            //Console.WriteLine(copy_text.text + " HELLAOEKSOD");
        }

        public override void CreateUI(Grid grid)
        {
            grid.ColumnDefinitions.Add(CustomUIElements.CreateColumnDefinition(1, GridUnitType.Star));
            grid.ColumnDefinitions.Add(CustomUIElements.CreateColumnDefinition(30, GridUnitType.Pixel));

            DockPanel dp = CustomUIElements.CreateDockPanel(1, 0);

            Label l1 = CustomUIElements.CreateLabel("Text:", 12, (Brush)Application.Current.Resources["font_color"]);
            DockPanel.SetDock(l1, Dock.Left);

            dp.Children.Add(l1);
            dp.Children.Add(CustomUIElements.CreateStandardBackgroundBorder(CustomUIElements.CreateActionTextBox(copy_text.text, TextBox_KeyDown, LostFocus, TextBox_GotFocus)));
            grid.Children.Add(dp);
        }

        private void LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox_LostFocus(sender, e);
            copy_text.text = (sender as TextBox).Text;
        }
    }

    class DelayObject : ActionObject
    {
        public override string name
        {
            get { return Constants.s_delay; }
        }

        public override string type
        {
            get { return Constants.delay; }
        }

        [JsonProperty(PropertyName = Constants.delay)]
        public Delay delay = new Delay();
        public class Delay
        {
            public int ms { get; set; }
        }

        public override void Execute()
        {
            //Console.WriteLine(copy_text.text + " HELLAOEKSOD");
        }

        public override void CreateUI(Grid grid)
        {
            grid.ColumnDefinitions.Add(CustomUIElements.CreateColumnDefinition(1, GridUnitType.Star));
            grid.ColumnDefinitions.Add(CustomUIElements.CreateColumnDefinition(30, GridUnitType.Pixel));

            DockPanel dp = CustomUIElements.CreateDockPanel(1, 0);

            Label l1 = CustomUIElements.CreateLabel("Delay (ms):", 12, (Brush)Application.Current.Resources["font_color"]);
            DockPanel.SetDock(l1, Dock.Left);

            dp.Children.Add(l1);
            dp.Children.Add(CustomUIElements.CreateStandardBackgroundBorder(CustomUIElements.CreateActionTextBox(delay.ms.ToString(), TextBox_KeyDown, LostFocus, TextBox_GotFocus)));
            grid.Children.Add(dp);
        }

        private void LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox_LostFocus(sender, e);
            int result = 0;
            int.TryParse((sender as TextBox).Text, out result);
            delay.ms = result;
        }
    }

    class KeyClickObject : ActionObject
    {
        public override string name
        {
            get { return Constants.s_key_click; }
        }

        public override string type
        {
            get { return Constants.key_click; }
        }

        [JsonProperty(PropertyName = Constants.key_click)]
        public KeyClick key_click = new KeyClick();
        public class KeyClick
        {
            public byte key { get; set; }
        }

        public override void Execute()
        {
            //Console.WriteLine(copy_text.text + " HELLAOEKSOD");
        }

        private TextBox textBox;
        private ComboBox comboBox;

        public override void CreateUI(Grid grid)
        {
            string text = "";
            int selectedIndex = 0;
            if (key_click.key != 0)
            {
                if (Constants.command_buttons.Values.Contains(key_click.key))
                {
                    selectedIndex = Constants.command_buttons.Values.IndexOf(key_click.key) + 1;
                }
                else
                {
                    text = ((Key)key_click.key).ToString();
                }
            }

            grid.ColumnDefinitions.Add(CustomUIElements.CreateColumnDefinition(1, GridUnitType.Star));
            grid.ColumnDefinitions.Add(CustomUIElements.CreateColumnDefinition(30, GridUnitType.Pixel));
            grid.ColumnDefinitions.Add(CustomUIElements.CreateColumnDefinition(1, GridUnitType.Star));
            grid.ColumnDefinitions.Add(CustomUIElements.CreateColumnDefinition(30, GridUnitType.Pixel));

            DockPanel dp1 = CustomUIElements.CreateDockPanel(1, 0);

            Label l1 = CustomUIElements.CreateLabel("Type key:", 12, (Brush)Application.Current.Resources["font_color"]);
            DockPanel.SetDock(l1, Dock.Left);

            dp1.Children.Add(l1);
            textBox = CustomUIElements.CreateActionTextBox(text, TextBox_KeyDown, LostFocus, TextBox_GotFocus, TextBox_Max_1_Changed);
            dp1.Children.Add(CustomUIElements.CreateStandardBackgroundBorder(textBox));
            grid.Children.Add(dp1);

            grid.Children.Add(CustomUIElements.CreateLabel("or", 12, (Brush)Application.Current.Resources["font_color"], 2, 0));

            comboBox = CustomUIElements.CreateComboBox(selectedIndex, "Select command key...", Constants.command_buttons.Keys, (grid.Width - 30 * 2 - l_width) / 2, grid.Height, 3, 0, SelectionChanged);
            grid.Children.Add(comboBox);
        }

        private void LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox_LostFocus(sender, e);
            var tb = sender as TextBox;
            Empty:
            if (tb.Text == "")
            {
                if (comboBox.SelectedIndex == 0)
                {
                    key_click.key = 0;
                }
                return;
            }
            try
            {
                // get byte value from string
                byte value = Convert.ToByte((Key)(new KeyConverter().ConvertFromString(tb.Text)));
                key_click.key = value;
                comboBox.SelectedIndex = 0;
            }
            catch (Exception exception)
            {
                // unvalid string to convert
                tb.Text = "";
                goto Empty;
            }
            try
            {
                tb.Text = tb.Text.ToUpper();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Couldn't convert text to upper");
            }
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            byte value = 0;
            string text = (e.AddedItems[0] as ComboBoxItem).Content.ToString();
            if (Constants.command_buttons.TryGetValue(text, out value))
            {
                key_click.key = value;
                textBox.Text = "";
            }
        }
    }

    class KeyDownObject : ActionObject
    {
        public override string name
        {
            get { return Constants.s_key_down; }
        }

        public override string type
        {
            get { return Constants.key_down; }
        }

        [JsonProperty(PropertyName = Constants.key_down)]
        public KeyDown key_down = new KeyDown();
        public class KeyDown
        {
            public byte key { get; set; }
        }

        public override void Execute()
        {
            //Console.WriteLine(copy_text.text + " HELLAOEKSOD");
        }

        private TextBox textBox;
        private ComboBox comboBox;

        public override void CreateUI(Grid grid)
        {
            string text = "";
            int selectedIndex = 0;
            if (key_down.key != 0)
            {
                if (Constants.command_buttons.Values.Contains(key_down.key))
                {
                    selectedIndex = Constants.command_buttons.Values.IndexOf(key_down.key) + 1;
                }
                else
                {
                    text = ((Key)key_down.key).ToString();
                }
            }

            grid.ColumnDefinitions.Add(CustomUIElements.CreateColumnDefinition(1, GridUnitType.Star));
            grid.ColumnDefinitions.Add(CustomUIElements.CreateColumnDefinition(30, GridUnitType.Pixel));
            grid.ColumnDefinitions.Add(CustomUIElements.CreateColumnDefinition(1, GridUnitType.Star));
            grid.ColumnDefinitions.Add(CustomUIElements.CreateColumnDefinition(30, GridUnitType.Pixel));

            DockPanel dp1 = CustomUIElements.CreateDockPanel(1, 0);

            Label l1 = CustomUIElements.CreateLabel("Type key:", 12, (Brush)Application.Current.Resources["font_color"]);
            DockPanel.SetDock(l1, Dock.Left);

            dp1.Children.Add(l1);
            textBox = CustomUIElements.CreateActionTextBox(text, TextBox_KeyDown, LostFocus, TextBox_GotFocus, TextBox_Max_1_Changed);
            dp1.Children.Add(CustomUIElements.CreateStandardBackgroundBorder(textBox));
            grid.Children.Add(dp1);

            grid.Children.Add(CustomUIElements.CreateLabel("or", 12, (Brush)Application.Current.Resources["font_color"], 2, 0));

            double width = (grid.Width - 30 * 2 - l_width) / 2;
            comboBox = CustomUIElements.CreateComboBox(selectedIndex, "Select command key...", Constants.command_buttons.Keys, width, grid.Height, 3, 0, SelectionChanged);
            grid.Children.Add(comboBox);
        }

        private void LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox_LostFocus(sender, e);
            var tb = sender as TextBox;
            Empty:
            if (tb.Text == "")
            {
                if (comboBox.SelectedIndex == 0)
                {
                    key_down.key = 0;
                }
                return;
            }
            try
            {
                // get byte value from string
                byte value = Convert.ToByte((Key)(new KeyConverter().ConvertFromString(tb.Text)));
                key_down.key = value;
                comboBox.SelectedIndex = 0;
            }
            catch (Exception exception)
            {
                // unvalid string to convert
                tb.Text = "";
                goto Empty;
            }
            try
            {
                tb.Text = tb.Text.ToUpper();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Couldn't convert text to upper");
            }
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            byte value = 0;
            string text = (e.AddedItems[0] as ComboBoxItem).Content.ToString();
            if (Constants.command_buttons.TryGetValue(text, out value))
            {
                key_down.key = value;
                textBox.Text = "";
            }
        }

    }

    class KeyUpObject : ActionObject
    {
        public override string name
        {
            get { return Constants.s_key_up; }
        }

        public override string type
        {
            get { return Constants.key_up; }
        }

        [JsonProperty(PropertyName = Constants.key_up)]
        public KeyUp key_up = new KeyUp();
        public class KeyUp
        {
            public byte key { get; set; }
        }

        public override void Execute()
        {
            //Console.WriteLine(copy_text.text + " HELLAOEKSOD");
        }

        private TextBox textBox;
        private ComboBox comboBox;

        public override void CreateUI(Grid grid)
        {
            string text = "";
            int selectedIndex = 0;
            if (key_up.key != 0)
            {
                if (Constants.command_buttons.Values.Contains(key_up.key))
                {
                    selectedIndex = Constants.command_buttons.Values.IndexOf(key_up.key) + 1;
                }
                else
                {
                    text = ((Key)key_up.key).ToString();
                }
            }

            grid.ColumnDefinitions.Add(CustomUIElements.CreateColumnDefinition(1, GridUnitType.Star));
            grid.ColumnDefinitions.Add(CustomUIElements.CreateColumnDefinition(30, GridUnitType.Pixel));
            grid.ColumnDefinitions.Add(CustomUIElements.CreateColumnDefinition(1, GridUnitType.Star));
            grid.ColumnDefinitions.Add(CustomUIElements.CreateColumnDefinition(30, GridUnitType.Pixel));

            DockPanel dp1 = CustomUIElements.CreateDockPanel(1, 0);

            Label l1 = CustomUIElements.CreateLabel("Type key:", 12, (Brush)Application.Current.Resources["font_color"]);
            DockPanel.SetDock(l1, Dock.Left);

            dp1.Children.Add(l1);
            textBox = CustomUIElements.CreateActionTextBox(text, TextBox_KeyDown, LostFocus, TextBox_GotFocus, TextBox_Max_1_Changed);
            dp1.Children.Add(CustomUIElements.CreateStandardBackgroundBorder(textBox));
            grid.Children.Add(dp1);

            grid.Children.Add(CustomUIElements.CreateLabel("or", 12, (Brush)Application.Current.Resources["font_color"], 2, 0));

            double width = (grid.Width - 30 * 2 - l_width) / 2;
            comboBox = CustomUIElements.CreateComboBox(selectedIndex, "Select command key...", Constants.command_buttons.Keys, width, grid.Height, 3, 0, SelectionChanged);
            grid.Children.Add(comboBox);
        }

        private void LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox_LostFocus(sender, e);
            var tb = sender as TextBox;
            Empty:
            if (tb.Text == "")
            {
                if (comboBox.SelectedIndex == 0)
                {
                    key_up.key = 0;
                }
                return;
            }
            try
            {
                // get byte value from string
                byte value = Convert.ToByte((Key)(new KeyConverter().ConvertFromString(tb.Text)));
                key_up.key = value;
                comboBox.SelectedIndex = 0;
            }
            catch (Exception exception)
            {
                // unvalid string to convert
                tb.Text = "";
                goto Empty;
            }
            try
            {
                tb.Text = tb.Text.ToUpper();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Couldn't convert text to upper");
            }
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            byte value = 0;
            string text = (e.AddedItems[0] as ComboBoxItem).Content.ToString();
            if (Constants.command_buttons.TryGetValue(text, out value))
            {
                key_up.key = value;
                textBox.Text = "";
            }
        }
    }

    class MouseClickObject : ActionObject
    {
        public override string name
        {
            get { return Constants.s_mouse_click; }
        }

        public override string type
        {
            get { return Constants.mouse_click; }
        }

        [JsonProperty(PropertyName = Constants.mouse_click)]
        public MouseClick mouse_click = new MouseClick();
        public class MouseClick
        {
            public int button { get; set; }
        }

        public override void Execute()
        {
            //Console.WriteLine(copy_text.text + " HELLAOEKSOD");
        }

        public override void CreateUI(Grid grid)
        {
            int selectedIndex = 0;
            if (mouse_click.button != 0)
            {
                if (Constants.mouse_buttons_down.Values.Contains(mouse_click.button))
                {
                    selectedIndex = Constants.mouse_buttons_down.IndexOfValue(mouse_click.button) + 1;
                }
            }

            grid.ColumnDefinitions.Add(CustomUIElements.CreateColumnDefinition(1, GridUnitType.Star));
            grid.ColumnDefinitions.Add(CustomUIElements.CreateColumnDefinition(30, GridUnitType.Pixel));

            grid.Children.Add(CustomUIElements.CreateComboBox(selectedIndex, "Select mouse button...", Constants.mouse_buttons_up.Keys, cb_width, grid.Height, 1, 0, SelectionChanged));
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int value = 0;
            string text = (e.AddedItems[0] as ComboBoxItem).Content.ToString();
            if (Constants.mouse_buttons_down.TryGetValue(text, out value))
            {
                mouse_click.button = value;
            }
            else
            {
                mouse_click.button = 0;
            }
        }
    }

    class MouseMoveObject : ActionObject
    {
        public override string name
        {
            get { return Constants.s_mouse_move; }
        }

        public override string type
        {
            get { return Constants.mouse_move; }
        }

        [JsonProperty(PropertyName = Constants.mouse_move)]
        public MouseMove mouse_move = new MouseMove();
        public class MouseMove
        {
            public int x { set; get; }
            public int y { set; get; }
        }

        public override void Execute()
        {
            //Console.WriteLine(copy_text.text + " HELLAOEKSOD");
        }

        public override void CreateUI(Grid grid)
        {
            grid.ColumnDefinitions.Add(CustomUIElements.CreateColumnDefinition(1, GridUnitType.Star));
            grid.ColumnDefinitions.Add(CustomUIElements.CreateColumnDefinition(1, GridUnitType.Star));
            grid.ColumnDefinitions.Add(CustomUIElements.CreateColumnDefinition(30, GridUnitType.Pixel));

            DockPanel dp = CustomUIElements.CreateDockPanel(1, 0);

            Label l1 = CustomUIElements.CreateLabel("X:", 12, (Brush)Application.Current.Resources["font_color"]);
            DockPanel.SetDock(l1, Dock.Left);

            dp.Children.Add(l1);
            dp.Children.Add(CustomUIElements.CreateStandardBackgroundBorder(CustomUIElements.CreateActionTextBox(mouse_move.x.ToString(), TextBox_KeyDown, LostFocusX, TextBox_GotFocus)));
            grid.Children.Add(dp);

            DockPanel dp2 = CustomUIElements.CreateDockPanel(2, 0);

            Label l2 = CustomUIElements.CreateLabel("Y:", 12, (Brush)Application.Current.Resources["font_color"]);
            DockPanel.SetDock(l2, Dock.Left);

            dp2.Children.Add(l2);
            dp2.Children.Add(CustomUIElements.CreateStandardBackgroundBorder(CustomUIElements.CreateActionTextBox(mouse_move.y.ToString(), TextBox_KeyDown, LostFocusY, TextBox_GotFocus)));
            grid.Children.Add(dp2);
        }

        private void LostFocusX(object sender, RoutedEventArgs e)
        {
            TextBox_LostFocus(sender, e);
            TextBox tb = sender as TextBox;
            int xVal = -1;
            if (int.TryParse(tb.Text, out xVal))
            {
                mouse_move.x = xVal;
            }
            else
            {
                tb.Text = "0";
                mouse_move.x = 0;
            }
        }

        private void LostFocusY(object sender, RoutedEventArgs e)
        {
            TextBox_LostFocus(sender, e);
            TextBox tb = sender as TextBox;
            int yVal = -1;
            if (int.TryParse(tb.Text, out yVal))
            {
                mouse_move.y = yVal;
            }
            else
            {
                tb.Text = "0";
                mouse_move.y = 0;
            }
        }
    }

    class MouseDownObject : ActionObject
    {
        public override string name
        {
            get { return Constants.s_mouse_down; }
        }

        public override string type
        {
            get { return Constants.mouse_down; }
        }

        [JsonProperty(PropertyName = Constants.mouse_down)]
        public MouseDown mouse_down = new MouseDown();
        public class MouseDown
        {
            public int button { set; get; }
        }

        public override void Execute()
        {
            //Console.WriteLine(copy_text.text + " HELLAOEKSOD");
        }

        public override void CreateUI(Grid grid)
        {
            int selectedIndex = 0;
            if (mouse_down.button != 0)
            {
                if (Constants.mouse_buttons_down.Values.Contains(mouse_down.button))
                {
                    selectedIndex = Constants.mouse_buttons_down.IndexOfValue(mouse_down.button) + 1;
                }
            }

            grid.ColumnDefinitions.Add(CustomUIElements.CreateColumnDefinition(1, GridUnitType.Star));
            grid.ColumnDefinitions.Add(CustomUIElements.CreateColumnDefinition(30, GridUnitType.Pixel));

            grid.Children.Add(CustomUIElements.CreateComboBox(selectedIndex, "Select mouse button...", Constants.mouse_buttons_up.Keys, cb_width, grid.Height, 1, 0, SelectionChanged));
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int value = 0;
            string text = (e.AddedItems[0] as ComboBoxItem).Content.ToString();
            if (Constants.mouse_buttons_down.TryGetValue(text, out value))
            {
                mouse_down.button = value;
            }
            else
            {
                mouse_down.button = 0;
            }
        }
    }

    class MouseUpObject : ActionObject
    {
        public override string name
        {
            get { return Constants.s_mouse_up; }
        }

        public override string type
        {
            get { return Constants.mouse_up; }
        }

        [JsonProperty(PropertyName = Constants.mouse_up)]
        public MouseUp mouse_up = new MouseUp();
        public class MouseUp
        {
            public int button { set; get; }
        }

        public override void Execute()
        {
            //Console.WriteLine(copy_text.text + " HELLAOEKSOD");
        }

        public override void CreateUI(Grid grid)
        {
            int selectedIndex = 0;
            if (mouse_up.button != 0)
            {
                if (Constants.mouse_buttons_down.Values.Contains(mouse_up.button))
                {
                    selectedIndex = Constants.mouse_buttons_down.IndexOfValue(mouse_up.button) + 1;
                }
            }

            grid.ColumnDefinitions.Add(CustomUIElements.CreateColumnDefinition(1, GridUnitType.Star));
            grid.ColumnDefinitions.Add(CustomUIElements.CreateColumnDefinition(30, GridUnitType.Pixel));

            grid.Children.Add(CustomUIElements.CreateComboBox(selectedIndex, "Select mouse button...", Constants.mouse_buttons_up.Keys, cb_width, grid.Height, 1, 0, SelectionChanged));
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int value = 0;
            string text = (e.AddedItems[0] as ComboBoxItem).Content.ToString();
            if (Constants.mouse_buttons_down.TryGetValue(text, out value))
            {
                mouse_up.button = value;
            }
            else
            {
                mouse_up.button = 0;
            }
        }
    }

    class OpenProgramObject : ActionObject
    {
        public override string name
        {
            get { return Constants.s_open_program; }
        }

        public override string type
        {
            get { return Constants.open_program; }
        }

        [JsonProperty(PropertyName = Constants.open_program)]
        public OpenProgram open_program = new OpenProgram();
        public class OpenProgram
        {
            public string path { get; set; }
        }

        public override void Execute()
        {
            //Console.WriteLine(copy_text.text + " HELLAOEKSOD");
        }

        public override void CreateUI(Grid grid)
        {
            grid.ColumnDefinitions.Add(CustomUIElements.CreateColumnDefinition(1, GridUnitType.Star));
            grid.ColumnDefinitions.Add(CustomUIElements.CreateColumnDefinition(30, GridUnitType.Pixel));

            DockPanel dp = CustomUIElements.CreateDockPanel(1, 0);

            Label l1 = CustomUIElements.CreateLabel("Path:", 12, (Brush)Application.Current.Resources["font_color"]);
            DockPanel.SetDock(l1, Dock.Left);

            dp.Children.Add(l1);
            dp.Children.Add(CustomUIElements.CreateStandardBackgroundBorder(CustomUIElements.CreateActionTextBox(open_program.path, TextBox_KeyDown, LostFocus, TextBox_GotFocus)));
            grid.Children.Add(dp);
        }

        private void LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox_LostFocus(sender, e);
            open_program.path = (sender as TextBox).Text;
        }
    }

    class OpenURLObject : ActionObject
    {
        public override string name
        {
            get { return Constants.s_open_url; }
        }

        public override string type
        {
            get { return Constants.open_url; }
        }

        [JsonProperty(PropertyName = Constants.open_url)]
        public OpenURL open_url = new OpenURL();
        public class OpenURL
        {
            public string url { get; set; }
        }

        public override void Execute()
        {
            //Console.WriteLine(copy_text.text + " HELLAOEKSOD");
        }

        public override void CreateUI(Grid grid)
        {
            grid.ColumnDefinitions.Add(CustomUIElements.CreateColumnDefinition(1, GridUnitType.Star));
            grid.ColumnDefinitions.Add(CustomUIElements.CreateColumnDefinition(30, GridUnitType.Pixel));

            DockPanel dp = CustomUIElements.CreateDockPanel(1, 0);

            Label l1 = CustomUIElements.CreateLabel("URL:", 12, (Brush)Application.Current.Resources["font_color"]);
            DockPanel.SetDock(l1, Dock.Left);

            dp.Children.Add(l1);
            dp.Children.Add(CustomUIElements.CreateStandardBackgroundBorder(CustomUIElements.CreateActionTextBox(open_url.url, TextBox_KeyDown, LostFocus, TextBox_GotFocus)));
            grid.Children.Add(dp);
        }

        private void LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox_LostFocus(sender, e);
            open_url.url = (sender as TextBox).Text;
        }
    }

    class TypeTextObject : ActionObject
    {
        public override string name
        {
            get { return Constants.s_type_text; }
        }

        public override string type
        {
            get { return Constants.type_text; }
        }

        [JsonProperty(PropertyName = Constants.type_text)]
        public TypeText type_text = new TypeText();
        public class TypeText
        {
            public string text { get; set; }
        }

        public override void Execute()
        {
            //Console.WriteLine(copy_text.text + " HELLAOEKSOD");
        }

        public override void CreateUI(Grid grid)
        {
            grid.ColumnDefinitions.Add(CustomUIElements.CreateColumnDefinition(1, GridUnitType.Star));
            grid.ColumnDefinitions.Add(CustomUIElements.CreateColumnDefinition(30, GridUnitType.Pixel));

            DockPanel dp = CustomUIElements.CreateDockPanel(1, 0);

            Label l1 = CustomUIElements.CreateLabel("Text:", 12, (Brush)Application.Current.Resources["font_color"]);
            DockPanel.SetDock(l1, Dock.Left);
            dp.Children.Add(l1);
            dp.Children.Add(CustomUIElements.CreateStandardBackgroundBorder(CustomUIElements.CreateActionTextBox(type_text.text, TextBox_KeyDown, LostFocus, TextBox_GotFocus)));
            grid.Children.Add(dp);
        }

        private void LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox_LostFocus(sender, e);
            type_text.text = (sender as TextBox).Text;
        }
    }
}