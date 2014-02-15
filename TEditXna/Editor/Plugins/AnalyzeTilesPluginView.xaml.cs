using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TEditXna.Editor.Plugins
{
    /// <summary>
    /// Interaction logic for AnalyzeTilesPluginView.xaml
    /// </summary>
    public partial class AnalyzeTilesPluginView : Window
    {
        string results;
        public AnalyzeTilesPluginView(string results)
        {
            InitializeComponent();
            this.results = results;
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            this.Results.Text = this.results;
        }
    }
}
