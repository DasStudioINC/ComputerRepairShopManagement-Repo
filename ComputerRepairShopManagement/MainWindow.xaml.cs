
using ComputerRepairShopManagement.Components;
using ComputerRepairShopManagement.Components.Enums;
using ComputerRepairShopManagement.Components.models;
using System;
using System.Data;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace ComputerRepairShopManagement
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            RepairRequestsGrid.ItemsSource = Backend.SELECT_FROM_REPAIRE_REQUEST().table.DefaultView;
        }

        private void RepairRequestsGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "CustomerID")
            {
                e.Cancel = true;
            }
        }


    }
}
