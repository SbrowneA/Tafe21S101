using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SQLite;
using SQLite.Net;
using StartFinance.Models;
using Windows.UI.Popups;
using System.Diagnostics;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace StartFinance.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShoppingListPage : Page
    {
        SQLiteConnection conn; // adding an SQLite connection
        string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Findata.sqlite");

        public ShoppingListPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            /// Initializing a database
            conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);
            // Creating table
            Results();
        }

        public void Results()
        {
            conn.CreateTable<ShoppingList>();
            var query1 = conn.Table<ShoppingList>();
            ShoppingListView.ItemsSource = query1.ToList();
        }

        private async void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string AccSelection = ((ShoppingList)ShoppingListView.SelectedItem).ItemName;
                if (AccSelection == "")
                {
                    MessageDialog dialog = new MessageDialog("Not selected the Item", "Oops..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    conn.CreateTable<ShoppingList>();
                    var query1 = conn.Table<ShoppingList>();
                    var query3 = conn.Query<ShoppingList>("DELETE FROM ShoppingList WHERE ItemName ='" + AccSelection + "'");
                    ShoppingListView.ItemsSource = query1.ToList();
                }
            }
            catch (NullReferenceException)
            {
                MessageDialog dialog = new MessageDialog("Not selected the Item", "Oops..!");
                await dialog.ShowAsync();
            }
        }


        private async void AddItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int qty;
                if (ItemNameTxt.Text.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("No value entered", "Oops..!");
                    await dialog.ShowAsync();
                }
                else if (QuantityTxt.Text.ToString() == "")
                {
                    qty = 0;
                }
                else
                {
                    try
                    {
                        qty = int.Parse(PriceTxt.Text.ToString());
                        if (qty < 1) {
                            throw new Exception();
                        }
                        double TempMoney = Convert.ToDouble(PriceTxt.Text);
                        conn.CreateTable<ShoppingList>();
                        conn.Insert(new ShoppingList
                        {
                            ItemName = ItemNameTxt.Text.ToString(),
                            Price = TempMoney,
                            Quantity = qty
                        });
                        // Creating table
                        Debug.WriteLine("Item was added");
                    }
                    catch (Exception ex)
                    {
                        MessageDialog dialog = new MessageDialog("Invalid quantity was entered, please enter a valid number (1 or greater)", "Oops..!");
                        await dialog.ShowAsync();
                    }

                    Results();
                }
            }
            catch (Exception ex)
            {
                if (ex is FormatException)
                {
                    MessageDialog dialog = new MessageDialog("You need to enter a valid price for this item. ", "Oops..!");
                    Debug.WriteLine(ex);
                    await dialog.ShowAsync();
                }
                else if (ex is SQLiteException)
                {
                    MessageDialog dialog = new MessageDialog("This Item already exists, Try using a different item name or" +
                        " changing the quantity of the item you are trying to add", "Oops..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    // no idea << WDYM?
                }
            }
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Results();
        }
    }
}
