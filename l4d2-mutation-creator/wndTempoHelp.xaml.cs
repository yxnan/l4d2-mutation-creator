﻿using System;
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
using System.Windows.Shapes;

namespace l4d2_mutation_creator
{
    /// <summary>
    /// WndTempoHelp.xaml 的交互逻辑
    /// </summary>
    public partial class WndTempoHelp : Window
    {
        public WndTempoHelp()
        {
            InitializeComponent();
        }

        private void BtnCloseHelpWindow_Click(object sender, RoutedEventArgs e)
        {
            App.wndTempoHelp.Hide();
        }
    }
}
