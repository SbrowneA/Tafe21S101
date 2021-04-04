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
                ShoppingList slectedItem = ((ShoppingList)ShoppingListView.SelectedItem);
                if (slectedItem == null)
                {
                    throw new NullReferenceException();
                }
                else
                {
                    conn.CreateTable<ShoppingList>();
                    conn.Delete(slectedItem);
                    var query1 = conn.Table<ShoppingList>();
                    ShoppingListView.ItemsSource = query1.ToList();
                }
            }
            catch (NullReferenceException)
            {
                MessageDialog dialog = new MessageDialog("No item selected", "Oops..!");
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
                    MessageDialog dialog = new MessageDialog("No item name entered", "Oops..!");
                    await dialog.ShowAsync();
                }
                else if (QuantityTxt.Text.ToString() == "")
                {
                    qty = 0;
                    QuantityTxt.Text = "0";
                    throw new ArgumentNullException();
                }
                else
                {
                    try
                    {
                        qty = int.Parse(QuantityTxt.Text);
                        if (qty < 1)
                        {
                            throw new ArgumentNullException();
                        }
                        double TempMoney = double.Parse(PriceTxt.Text);
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
                    catch (ArgumentNullException)
                    {
                        MessageDialog dialog = new MessageDialog("Invalid quantity was entered, please enter a valid number (1 or greater)", "Oops..!");
                        await dialog.ShowAsync();
                    }

                    Results();
                }
            }
            catch (FormatException)
            {
                MessageDialog dialog = new MessageDialog("You need to enter a valid price for this item. ", "Oops..!");
                await dialog.ShowAsync();
            }
            catch (SQLiteException)
            {
                MessageDialog dialog = new MessageDialog("This Item already exists, Try using a different item name or" +
                    " changing the quantity of the item you are trying to add", "Oops..!");
                await dialog.ShowAsync();
            }
        }

        private void clearBtn_Click(object sender, RoutedEventArgs e)
        {
            ItemNameTxt.Text = "";
            PriceTxt.Text = "";
            QuantityTxt.Text = "";
            ShoppingListView.SelectedItem = null;
        }

        private async void UpdateItem_Click(object sender, RoutedEventArgs e)
        {
            //Get selected value
            try
            {
                ShoppingList slectedItem = ((ShoppingList)ShoppingListView.SelectedItem);
                //Check data is valid
                if (slectedItem == null)
                {
                    throw new NullReferenceException();
                }
                else
                {
                    int qty;
                    if (ItemNameTxt.Text.ToString() == "")
                    {
                        MessageDialog dialog = new MessageDialog("No name entered", "Oops..!");
                        await dialog.ShowAsync();
                    }
                    else if (QuantityTxt.Text.ToString() == "")
                    {
                        qty = 0;
                        QuantityTxt.Text = "0";
                        throw new ArgumentNullException();
                    }

                    qty = int.Parse(PriceTxt.Text.ToString());
                    if (qty < 1)
                    {
                        throw new Exception();
                    }
                    double TempMoney = Convert.ToDouble(PriceTxt.Text);
                    //update values
                    slectedItem.ItemName = ItemNameTxt.Text.ToString();
                    slectedItem.Price = double.Parse(PriceTxt.Text);
                    slectedItem.Quantity = int.Parse(QuantityTxt.Text);
                    conn.CreateTable<ShoppingList>();
                    conn.Update(slectedItem);
                }
            }
            catch (NullReferenceException)
            {
                MessageDialog dialog = new MessageDialog("You have not selected the an Item", "Oops..!");
                await dialog.ShowAsync();
            }
            catch (SQLiteException)
            {
                MessageDialog dialog = new MessageDialog("This Item already exists, Try using a different item name or" +
                    " changing the quantity of the item you are trying to add", "Oops..!");
                await dialog.ShowAsync();
            }
            catch (FormatException)
            {
                MessageDialog dialog = new MessageDialog("You need to enter a valid price for this item. ", "Oops..!");
                await dialog.ShowAsync();
            }
            catch (ArgumentNullException)
            {
                //Debug.WriteLine(ex);
                MessageDialog dialog = new MessageDialog("Invalid quantity was entered, please enter a valid number (1 or greater)", "Oops..!");
                await dialog.ShowAsync();
            }
            Results();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Results();
        }
    }
}
