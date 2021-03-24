// **************************************************************************
//Start Finance - An to manage your personal finances.

//Start Finance is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.

//Start Finance is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
//GNU General Public License for more details.

//You should have received a copy of the GNU General Public License
//along with Start Finance.If not, see<http://www.gnu.org/licenses/>.
// ***************************************************************************

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
using StartFinance.Models;
using Windows.UI.Popups;
using SQLite.Net;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace StartFinance.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ContactDetailsPage : Page
    {
        SQLiteConnection conn; // adding an SQLite connection
        string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Findata.sqlite");

        public ContactDetailsPage()
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
            conn.CreateTable<Contact>();
            var query1 = conn.Table<Contact>();
            ContactDetailsView.ItemsSource = query1.ToList();
        }

        private async void AddContact_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ContactFirstName.Text.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("No value entered", "Oops..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    conn.CreateTable<Contact>();
                    conn.Insert(new Contact
                    {
                        FirstName = ContactFirstName.Text.ToString(),
                        LastName = ContactLastName.Text.ToString(),
                        CompanyName = companyName.Text.ToString(),
                        PhoneNumber = phNumber.Text.ToString(),
                    });
                    // Creating table
                    Results();
                }
            }
            catch (Exception ex)
            {
                if (ex is FormatException)
                {
                    MessageDialog dialog = new MessageDialog("You forgot to enter the Details or entered an invalid Details", "Oops..!");
                    await dialog.ShowAsync();
                }
                else if (ex is SQLiteException)
                {
                    MessageDialog dialog = new MessageDialog("Customer Name already exist, Try Different Name", "Oops..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    /// no idea
                }
            }
        }

        private async void DeleteContact_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int AccSelection = ((Contact)ContactDetailsView.SelectedItem).ID;
                if (AccSelection == 0)
                {
                    MessageDialog dialog = new MessageDialog("Not selected the Item", "Oops..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    conn.CreateTable<Contact>();
                    var query1 = conn.Table<Contact>();
                    var query3 = conn.Query<Contact>("DELETE FROM Contact WHERE ID ='" + AccSelection + "'");
                    ContactDetailsView.ItemsSource = query1.ToList();
                }
            }
            catch (NullReferenceException)
            {
                MessageDialog dialog = new MessageDialog("Not selected the Item", "Oops..!");
                await dialog.ShowAsync();
            }
        }

        private async void UpdateContact_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int AccSelection = ((Contact)ContactDetailsView.SelectedItem).ID;
                if (AccSelection == 0)
                {
                    MessageDialog dialog = new MessageDialog("Not selected the Item", "Oops..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    conn.CreateTable<Contact>();
                    Contact contact = conn.Get<Contact>(AccSelection);

                    contact.FirstName = ContactFirstName.Text.ToString();
                    contact.LastName = ContactLastName.Text.ToString();
                    contact.CompanyName = companyName.Text.ToString();
                    contact.PhoneNumber = phNumber.Text.ToString();


                    conn.Update(contact);
                }
                // Creating table
                Results();



            }
            catch (NullReferenceException)
            {
                MessageDialog dialog = new MessageDialog("Not selected the Item", "Oops..!");
                await dialog.ShowAsync();
            }
        }

        private async void ClearContact_Click(object sender, RoutedEventArgs e)
        {
            ContactFirstName.Text = "";
            ContactLastName.Text = "";
            companyName.Text = "";
            phNumber.Text = "";
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Results();
        }
    }
}
