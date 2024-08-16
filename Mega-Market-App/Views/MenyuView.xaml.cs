using Mega_Market_App.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mega_Market_App.Views;

public partial class MenyuView : Page
{
    public MenyuView()
    {
        InitializeComponent();
    }
    private void Themes_Click(object sender, RoutedEventArgs e)
    {
        if (Themes.IsChecked == true)
            ThemesController.SetTheme(ThemesController.ThemeTypes.Dark);
        else
            ThemesController.SetTheme(ThemesController.ThemeTypes.Light);
    }
}
