using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Xna.Framework.Input;
using project.main;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using project.main.json;
using System.Linq.Expressions;

namespace project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Macro> macros = new List<Macro>();

        // temp consts
        private const int margin_top = 4;
        private const int l_width = 170;
        private const int cb_width = 200;

        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            Constants.Init();

            /*
            Macro m = new Macro(null, "Minecraft noobs");
            m.hotkey = 9182;
            CopyTextObject ct = new CopyTextObject();
            ct.copy_text.text = "Copy this text to clipboard";
            m.actionList.Add(ct);

            (m.actionList[0] as ActionObject).Execute();

            using (StreamWriter file = File.CreateText(Path.Combine(Environment.CurrentDirectory, "macros", "CSGObhop" + ".json")))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(file, m);
            }*/

            LoadMacros();
        }

        private void AddAction(int index = -1)
        {
            Grid grid = CustomUIElements.CreateGrid((double)Application.Current.Resources["action_width"], (double)Application.Current.Resources["action_height"]);
            grid.Margin = new Thickness(0, margin_top, 0, 0);
            grid.ColumnDefinitions.Add(CustomUIElements.CreateColumnDefinition(1, GridUnitType.Star));
            grid.ColumnDefinitions.Add(CustomUIElements.CreateColumnDefinition(30, GridUnitType.Pixel));

            ComboBox cb = CustomUIElements.CreateComboBox(0, "Add action...", Constants.action_names, 200, grid.Height, 0, 0, action_changed);
            cb.HorizontalAlignment = HorizontalAlignment.Center;
            grid.Children.Add(cb);

            Image close = CustomUIElements.CreateImageButton(14, 14, @"/imgs/close.png", button_remove_Click, null, null, null);
            Grid.SetColumn(close, 1);
            Grid.SetColumnSpan(close, 1);
            Grid.SetRow(close, 0);
            Grid.SetRowSpan(close, 1);
            grid.Children.Add(close);

            stackPanel_actions.Children.Insert(index >= 0 ? index : stackPanel_actions.Children.Count - 1, grid);

            Label rowNum = CustomUIElements.CreateLabel((stackPanel_actions.Children.Count - 1).ToString(), 11, (Brush)Application.Current.Resources["light_font_color"]);
            rowNum.Height = grid.Height;
            rowNum.Margin = new Thickness(0, margin_top, 0, 0);
            row_numbers_stackpanel.Children.Add(rowNum);
        }

        #region ui element events
        private void ClearFocus()
        {
            grid_ClearFocus.Focus();
        }

        private void button_remove_Click(object sender, MouseButtonEventArgs e)
        {
            Grid grid = (VisualTreeHelper.GetParent(sender as Image) as UIElement) as Grid;
            int index = stackPanel_actions.Children.IndexOf(grid);
            grid.Children.Clear();
            GetSelectedMacro().actionList.RemoveAt(index);
            stackPanel_actions.Children.RemoveAt(index);
            row_numbers_stackpanel.Children.RemoveAt(row_numbers_stackpanel.Children.Count - 1);
        }

        private void button_add_Click(object sender, MouseButtonEventArgs e)
        {
            AddAction();
        }

        private void action_changed(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cbSender = sender as ComboBox;
            if (cbSender.SelectedIndex == 0) return;
            string action = (cbSender.SelectedItem as ComboBoxItem).Content as string;
            Grid grid = (VisualTreeHelper.GetParent((ComboBox)sender) as UIElement) as Grid;

            // clean up grid
            grid.ColumnDefinitions.Clear();
            grid.RowDefinitions.Clear();
            while (grid.Children.Count > 1)
            {
                grid.Children.RemoveAt(0);
            }

            // for every action
            grid.ColumnDefinitions.Add(CustomUIElements.CreateColumnDefinition(l_width, GridUnitType.Pixel));

            Label labelAction = CustomUIElements.CreateLabel(action, 14, (Brush)Application.Current.Resources["accent_color"], 0, 0);

            Border backgroundBorder = CustomUIElements.CreateStandardBackgroundBorder(labelAction);
            backgroundBorder.Background = (Brush)Application.Current.Resources["light_color"];
            grid.Children.Add(backgroundBorder);

            Image close = grid.Children[0] as Image;

            ActionObject obj = null;
            bool createUI = true;
            switch (action)
            {
                case Constants.s_mouse_down:
                    {
                        (obj = new MouseDownObject()).Init(grid_ClearFocus);
                        GetSelectedMacro().actionList.Add(obj);
                    }
                    break;
                case Constants.s_mouse_up:
                    {
                        (obj = new MouseUpObject()).Init(grid_ClearFocus);
                        GetSelectedMacro().actionList.Add(obj);
                    }
                    break;
                case Constants.s_mouse_move:
                    {
                        (obj = new MouseMoveObject()).Init(grid_ClearFocus);
                        GetSelectedMacro().actionList.Add(obj);
                    }
                    break;
                case Constants.s_mouse_click:
                    {
                        (obj = new MouseClickObject()).Init(grid_ClearFocus);
                        GetSelectedMacro().actionList.Add(obj);
                    }
                    break;
                case Constants.s_key_down:
                    {
                        (obj = new KeyDownObject()).Init(grid_ClearFocus);
                        GetSelectedMacro().actionList.Add(obj);
                    }
                    break;
                case Constants.s_key_up:
                    {
                        (obj = new KeyUpObject()).Init(grid_ClearFocus);
                        GetSelectedMacro().actionList.Add(obj);
                    }
                    break;
                case Constants.s_key_click:
                    {
                        (obj = new KeyClickObject()).Init(grid_ClearFocus);
                        GetSelectedMacro().actionList.Add(obj);
                    }
                    break;
                case Constants.s_open_program:
                    {
                        (obj = new OpenProgramObject()).Init(grid_ClearFocus);
                        GetSelectedMacro().actionList.Add(obj);
                    }
                    break;
                case Constants.s_open_url:
                    {
                        (obj = new OpenURLObject()).Init(grid_ClearFocus);
                        GetSelectedMacro().actionList.Add(obj);
                    }
                    break;
                case Constants.s_close_program:
                    {
                        (obj = new CloseProgramObject()).Init(grid_ClearFocus);
                        GetSelectedMacro().actionList.Add(obj);
                    }
                    break;
                case Constants.s_delay:
                    {
                        (obj = new DelayObject()).Init(grid_ClearFocus);
                        GetSelectedMacro().actionList.Add(obj);
                    }
                    break;
                case Constants.s_type_text:
                    {
                        (obj = new TypeTextObject()).Init(grid_ClearFocus);
                        GetSelectedMacro().actionList.Add(obj);
                    }
                    break;
                case Constants.s_copy_text:
                    {
                        (obj = new CopyTextObject()).Init(grid_ClearFocus);
                        GetSelectedMacro().actionList.Add(obj);
                    }
                    break;
                default:
                    Console.WriteLine("Exception: Action not found!");
                    createUI = false;
                    break;
            }
            if (createUI)
            {
                obj.CreateUI(grid);
                CloseButtonUIFix(grid);
            }
        }
        #endregion

        #region macros
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddMacro();
        }

        private void MacroMouseDown(object sender, MouseButtonEventArgs e)
        {
            SelectMacro(GetMacro(sender));
        }

        private void SelectMacro(Macro macro)
        {
            // deselect previous
            Macro temp = GetSelectedMacro();
            if (temp != null) temp.Deselect();
            while (stackPanel_actions.Children.Count > 1)
            {
                stackPanel_actions.Children.RemoveAt(0);
            }
            row_numbers_stackpanel.Children.Clear();

            // select
            macro.Select();
            LoadActions(macro);
        }

        private void MacroMouseEnter(object sender, MouseEventArgs e)
        {
            GetMacro(sender).Highlight();
        }

        private void MacroMouseLeave(object sender, MouseEventArgs e)
        {
            GetMacro(sender).Unhighlight();
        }

        private void EditMouseDown(object sender, MouseButtonEventArgs e)
        {
            GetMacro(sender).EditName();
        }


        private void MacroNameKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                GetMacro(sender).EditNameDone();
                SortMacros();
            }
        }

        private void MacroNameLostFocus(object sender, RoutedEventArgs e)
        {
            GetMacro(sender).EditNameCanceled();
        }

        private void SortMacros()
        {
            macros.Sort((x, y) => string.Compare(x.Name, y.Name));
            stackPanel_macros.Children.Clear();
            for (int i = 0; i < macros.Count; i++)
            {
                stackPanel_macros.Children.Add(macros[i].Parent);
            }
        }

        private Macro GetMacro(object sender)
        {
            while (!(sender is Border))
            {
                sender = VisualTreeHelper.GetParent(sender as UIElement);
            }
            foreach (Macro m in macros)
            {
                if (m.Parent == sender)
                {
                    return m;
                }
            }

            return null;
        }

        private Macro GetSelectedMacro()
        {
            foreach (Macro m in macros)
            {
                if (m.isSelected())
                {
                    return m;
                }
            }

            return null;
        }

        private bool MacrosContain(string name)
        {
            foreach (Macro m in macros)
            {
                if (m.Name == name) return true;
            }

            return false;
        }

        private void AddMacro()
        {
            int count = 1;
            string newName = "Macro " + count;
            while (MacrosContain(newName))
            {
                count++;
                newName = "Macro " + count;
            }

            UIElement macroUI = CreateMacroUI(newName);
            stackPanel_macros.Children.Add(macroUI);

            Macro macro = new Macro(macroUI, newName);
            macros.Add(macro);
            SelectMacro(macro);
            SortMacros();
        }

        private UIElement CreateMacroUI(string name)
        {
            Border b = CustomUIElements.CreateBackgroundBorder((Brush)Application.Current.Resources["dark_color"], null, -1, 50, 0, new Thickness(0));
            b.MouseDown += new MouseButtonEventHandler(MacroMouseDown);
            b.MouseEnter += new MouseEventHandler(MacroMouseEnter);
            b.MouseLeave += new MouseEventHandler(MacroMouseLeave);

            Grid g = new Grid();
            g.RowDefinitions.Add(CustomUIElements.CreateRowDefinition(1, GridUnitType.Star));
            g.RowDefinitions.Add(CustomUIElements.CreateRowDefinition(1, GridUnitType.Star));

            DockPanel dp = CustomUIElements.CreateDockPanel(0, 0);

            Image edit = CustomUIElements.CreateImageButton(14, 14, @"imgs/edit.png", EditMouseDown, null, null, null);
            edit.Margin = new Thickness(0, 0, 10, 0);
            edit.VerticalAlignment = VerticalAlignment.Bottom;
            DockPanel.SetDock(edit, Dock.Right);

            TextBox tb_name = CustomUIElements.CreateTextBox((Brush)Application.Current.Resources["font_color"], new Thickness(5, 0, 5, 0), 16, MacroNameKeyDown);
            tb_name.VerticalAlignment = VerticalAlignment.Bottom;
            tb_name.VerticalContentAlignment = VerticalAlignment.Bottom;
            tb_name.CaretBrush = Brushes.White;
            tb_name.Text = name;
            tb_name.IsReadOnly = true;
            tb_name.IsHitTestVisible = false;
            tb_name.LostFocus += new RoutedEventHandler(MacroNameLostFocus);

            dp.Children.Add(edit);
            dp.Children.Add(tb_name);

            Label key = CustomUIElements.CreateLabel("Specifiy key...", 12, (Brush)Application.Current.Resources["dark_accent_color"], 0, 1);
            key.Padding = new Thickness(5, 0, 5, 0);
            key.VerticalAlignment = VerticalAlignment.Top;
            key.VerticalContentAlignment = VerticalAlignment.Top;

            g.Children.Add(dp);
            g.Children.Add(key);

            b.Child = g;

            return b;
        }

        private Grid CreateActionUIBone(string action, int index = -1)
        {
            Grid grid = CustomUIElements.CreateGrid((double)Application.Current.Resources["action_width"], (double)Application.Current.Resources["action_height"]);
            grid.Margin = new Thickness(0, margin_top, 0, 0);

            Image close = CustomUIElements.CreateImageButton(14, 14, @"/imgs/close.png", button_remove_Click, null, null, null);
            Grid.SetColumn(close, 1);
            Grid.SetColumnSpan(close, 1);
            Grid.SetRow(close, 0);
            Grid.SetRowSpan(close, 1);
            grid.Children.Add(close);

            stackPanel_actions.Children.Insert(index >= 0 ? index : stackPanel_actions.Children.Count - 1, grid);

            Label rowNum = CustomUIElements.CreateLabel((stackPanel_actions.Children.Count - 1).ToString(), 11, (Brush)Application.Current.Resources["light_font_color"]);
            rowNum.Height = grid.Height;
            rowNum.Margin = new Thickness(0, margin_top, 0, 0);
            row_numbers_stackpanel.Children.Add(rowNum);

            grid.ColumnDefinitions.Add(CustomUIElements.CreateColumnDefinition(l_width, GridUnitType.Pixel));

            Label labelAction = CustomUIElements.CreateLabel(action, 14, (Brush)Application.Current.Resources["accent_color"], 0, 0);

            Border backgroundBorder = CustomUIElements.CreateStandardBackgroundBorder(labelAction);
            backgroundBorder.Background = (Brush)Application.Current.Resources["light_color"];
            grid.Children.Add(backgroundBorder);

            return grid;
        }

        private void CloseButtonUIFix(Grid grid)
        {
            Image close = grid.Children[0] as Image;
            Grid.SetColumn(close, grid.ColumnDefinitions.Count - 1);
            Grid.SetColumnSpan(close, 1);
            Grid.SetRow(close, 0);
            Grid.SetRowSpan(close, 1);
        }

        #endregion

        #region save / load macros

        private void LoadActions(Macro macro)
        {
            for (int i = 0; i < macro.actionList.Count; i++)
            {
                var actionObj = macro.actionList[i];
                Grid grid = CreateActionUIBone(actionObj.name, -1);
                actionObj.CreateUI(grid);
                CloseButtonUIFix(grid);
            }
        }

        private void LoadMacros()
        {
            // open all saved macros and deserialize them
            string[] files = Directory.GetFiles(Constants.file_path, "*.json", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                string json = File.ReadAllText(file);
                var rawObj = JObject.Parse(json);

                string name = rawObj[Constants.s_macro_name].Value<string>();
                int hotkey = rawObj[Constants.s_macro_hotkey].Value<int>();
                Macro macro = new Macro(CreateMacroUI(name), name);
                macro.Hotkey = hotkey;
                macros.Add(macro);

                JArray lst = rawObj.SelectToken(Constants.s_action_list) as JArray;
                foreach (JObject jo in lst.Children())
                {
                    //string type = jo["type"].Value<string>();
                    JProperty jp = jo.First.Value<JProperty>();
                    switch (jp.Name)
                    {
                        case Constants.close_program:
                            {
                                var obj = jo.ToObject<CloseProgramObject>();
                                obj.Init(grid_ClearFocus);
                                macro.actionList.Add(obj);
                            }
                            break;
                        case Constants.copy_text:
                            {
                                var obj = jo.ToObject<CopyTextObject>();
                                obj.Init(grid_ClearFocus);
                                macro.actionList.Add(obj);
                            }
                            break;
                        case Constants.delay:
                            {
                                var obj = jo.ToObject<DelayObject>();
                                obj.Init(grid_ClearFocus);
                                macro.actionList.Add(obj);
                            }
                            break;
                        case Constants.key_click:
                            {
                                var obj = jo.ToObject<KeyClickObject>();
                                obj.Init(grid_ClearFocus);
                                macro.actionList.Add(obj);
                            }
                            break;
                        case Constants.key_down:
                            {
                                var obj = jo.ToObject<KeyDownObject>();
                                obj.Init(grid_ClearFocus);
                                macro.actionList.Add(obj);
                            }
                            break;
                        case Constants.key_up:
                            {
                                var obj = jo.ToObject<KeyUpObject>();
                                obj.Init(grid_ClearFocus);
                                macro.actionList.Add(obj);
                            }
                            break;
                        case Constants.mouse_click:
                            {
                                var obj = jo.ToObject<MouseClickObject>();
                                obj.Init(grid_ClearFocus);
                                macro.actionList.Add(obj);
                            }
                            break;
                        case Constants.mouse_move:
                            {
                                var obj = jo.ToObject<MouseMoveObject>();
                                obj.Init(grid_ClearFocus);
                                macro.actionList.Add(obj);
                            }
                            break;
                        case Constants.mouse_up:
                            {
                                var obj = jo.ToObject<MouseUpObject>();
                                obj.Init(grid_ClearFocus);
                                macro.actionList.Add(obj);
                            }
                            break;
                        case Constants.mouse_down:
                            {
                                var obj = jo.ToObject<MouseDownObject>();
                                obj.Init(grid_ClearFocus);
                                macro.actionList.Add(obj);
                            }
                            break;
                        case Constants.open_program:
                            {
                                var obj = jo.ToObject<OpenProgramObject>();
                                obj.Init(grid_ClearFocus);
                                macro.actionList.Add(obj);
                            }
                            break;
                        case Constants.open_url:
                            {
                                var obj = jo.ToObject<OpenURLObject>();
                                obj.Init(grid_ClearFocus);
                                macro.actionList.Add(obj);
                            }
                            break;
                        case Constants.type_text:
                            {
                                var obj = jo.ToObject<TypeTextObject>();
                                obj.Init(grid_ClearFocus);
                                macro.actionList.Add(obj);
                            }
                            break;
                        default:
                            Console.WriteLine("Action not found!");
                            break;
                    }
                }
            }

            SortMacros();
            SelectMacro(macros[0]);
        }

        private void SaveMacros()
        {
            foreach (Macro macro in macros)
            {
                using (StreamWriter file = File.CreateText(Path.Combine(Constants.file_path, macro.Name + ".json")))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Formatting = Formatting.Indented;
                    serializer.Serialize(file, macro);
                }
            }
        }
        #endregion

        private void window_main_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Console.WriteLine("Saving...");
            SaveMacros();
            Console.WriteLine("Done!");
        }
    }

}
