
using ComputerRepairShopManagement.Components;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ComputerRepairShopManagement
{
    public partial class MainWindow : Window
    {
      
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            single.ItemsSource = new Customer[]
{
    Backend.SELECT_FROM_CUSTOMER_WHERE_CUSTOMER_ID(1).customer
};
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCustomers();
        }

        private void LoadCustomers()
        {

            CustomersGrid.ItemsSource = Backend.SELECT_FROM_CUSTOMER().table.DefaultView;
        }

        private void InsertCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FullNameInput.Text) ||
                string.IsNullOrWhiteSpace(PhoneInput.Text) ||
                string.IsNullOrWhiteSpace(EmailInput.Text))
            {
                MessageBox.Show("Fill in all customer fields.");
                return;
            }



            Backend.INSERT_INTO_CUSTOMER(
                FullNameInput.Text,
                PhoneInput.Text,
                EmailInput.Text
            );

           

            LoadCustomers();

            FullNameInput.Clear();
            PhoneInput.Clear();
            EmailInput.Clear();

           
        }

        private void UpdateCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FullNameInput.Text) ||
                string.IsNullOrWhiteSpace(PhoneInput.Text) ||
                string.IsNullOrWhiteSpace(EmailInput.Text))
            {
                MessageBox.Show("Fill in all customer fields.");
                return;
            }



            var inserted = Backend.UPDATE_CUSTOMER_DETAILS(
                3,
                FullNameInput.Text,
                PhoneInput.Text,
                EmailInput.Text
            );

            if (inserted.after == null)
            {
                return;
            }

            LoadCustomers();

            FullNameInput.Clear();
            PhoneInput.Clear();
            EmailInput.Clear();

            MessageBox.Show(
                $"Customer successfully inserted!\nCustomer ID: {inserted.after.CustomerId}"
            );
        }

        private void DeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            var inserted = Backend.DELETE_CUSTOMER_WHERE_CUSTOMER_ID(6);
            LoadCustomers();
            MessageBox.Show(
                $"Customer successfully deleted!\nCustomer ID: {inserted.CustomerId}"
            );
        }

        private void CustomersGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "CustomerID")
            {
                e.Cancel = true;
            }
        }

        private void gensummary(object sender, RoutedEventArgs e)
        {
            //Summary.write(Backend.SELECT_FROM_REPAIRE_REQUEST_WHERE_REPAIR_ID(), Backend.SELECT_FROM_CUSTOMER_WHERE_CUSTOMER_ID());
        }
    }
}
