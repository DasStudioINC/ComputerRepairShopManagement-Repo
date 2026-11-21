
using ComputerRepairShopManagement.Components;
using Microsoft.Data.SqlClient;
using System;
using System.Windows;

namespace ComputerRepairShopManagement
{
    public partial class MainWindow : Window
    {
        private readonly string[] columns =
        {
            "CustomerId",
            "FullName",
            "Phone",
            "Email"
        };

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                CustomersGrid.ItemsSource =
                    Backend.SELECT_FROM_CUSTOMER(columns);
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Database Error ({ex.Number}): {ex.Message}");
                throw;
            }
        }
    }
}
