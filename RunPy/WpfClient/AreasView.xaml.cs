﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfClient.Interfaces;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class AreasView : UserControl
    {
        private IAreasWindowViewModel _vm;

        public AreasView()
        {
            InitializeComponent();
        }

        public AreasView(IAreasWindowViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
        }
    }
}
