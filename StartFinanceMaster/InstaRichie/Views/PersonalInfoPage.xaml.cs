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
using System.Text.RegularExpressions;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace StartFinance.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PersonalInfoPage : Page
    {
        SQLiteConnection conn; // adding an SQLite connection
        string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Findata.sqlite");
        //Date of Birth constant pattern
        const string MAIL_PATTEN = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";

        public PersonalInfoPage()
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
            conn.CreateTable<PersonalInfo>();
            var query1 = conn.Table<PersonalInfo>();
            PersonalInfoView.ItemsSource = query1.ToList();
        }

        private async void AddPersonalInfo_Click(object sender, RoutedEventArgs e)
        {
            string
            fname = _Fname.Text.ToString(),
              lname = _Lname.Text.ToString(),
              dob = _Dob.Text.ToString(),
              gender = _Gender.SelectionBoxItem.ToString(),
              mail = _Email.Text.ToString(),
              mobile = _Mphone.Text;
            if (fname == "")
            {
                var dialogMessage = new MessageDialog("First name cannot be left blank.");
                await dialogMessage.ShowAsync();
                _Fname.Focus(FocusState.Programmatic);
                return;
            }
            else if (lname == "")
            {
                var dialogMessage = new MessageDialog("Last name cannot be left blank.");
                await dialogMessage.ShowAsync();
                _Lname.Focus(FocusState.Programmatic);
                return;
            }
            else if (mobile == "")
            {
                var dialogMessage = new MessageDialog("Mobile number cannot be left blank.");
                await dialogMessage.ShowAsync();
                _Mphone.Focus(FocusState.Programmatic);
                return;
            }
            else if (mail == "")
            {
                var dialogMessage = new MessageDialog("Email address cannot be left blank.");
                await dialogMessage.ShowAsync();
                _Email.Focus(FocusState.Programmatic);
                return;
            }
            else if (dob == "")
            {
                var dialogMessage = new MessageDialog("Birthday cannot be left blank.");
                await dialogMessage.ShowAsync();
                _Dob.Focus(FocusState.Programmatic);
                return;
            }
            foreach (char c in fname)
            {
                if (!Char.IsLetter(c))
                {
                    var dialogMessage = new MessageDialog("First name can only contain letters.");
                    await dialogMessage.ShowAsync();
                    _Fname.Focus(FocusState.Programmatic);
                    _Fname.SelectAll();
                    return;
                }
            }
            foreach (char z in lname)
            {
                if (!Char.IsLetter(z))
                {
                    var dialogMessage = new MessageDialog("Last name can only contain letters.");
                    await dialogMessage.ShowAsync();
                    _Lname.Focus(FocusState.Programmatic);
                    _Lname.SelectAll();
                    return;
                }
            }
            if (!Regex.IsMatch(mail, MAIL_PATTEN))
            {
                var dialogMessage = new MessageDialog("Invalid email address.");
                await dialogMessage.ShowAsync();
                _Email.Focus(FocusState.Programmatic);
                _Email.SelectAll();
                return;
            }
            try
            {
                DateTime tryDob = DateTime.Parse(dob);
            }
            catch
            {
                var dialogMessage = new MessageDialog("Invalid birthday.");
                await dialogMessage.ShowAsync();
                _Dob.Focus(FocusState.Programmatic);
                _Dob.SelectAll();
                return;
            }
            try
            {
                long v = long.Parse(mobile);
            }
            catch
            {
                var dialogMessage = new MessageDialog("Mobile number must be numeric only.");
                await dialogMessage.ShowAsync();
                _Mphone.Focus(FocusState.Programmatic);
                _Mphone.SelectAll();
                return;
            }
            //Insert Data after validation
            try
            {
                conn.Insert(new PersonalInfo()
                {
                    FirstName = fname,
                    LastName = lname,
                    DoB = dob,
                    Gender = gender,
                    Email = mail,
                    MobilePhone = mobile
                });
            }
            catch
            {
                var dialogMessage = new MessageDialog("Email address or mobile phone already exists, try using a different one.");
                await dialogMessage.ShowAsync();
                return;
            }
            //update listView
            Results();
            //after clicking the button and passing, reset all
            ResetPersonalInfo();
        }
        //Method to reset all textboxes
        private void ResetPersonalInfo()
        {
            _Fname.Text = "";
            _Lname.Text = "";
            _Dob.Text = "";
            _Email.Text = "";
            _Mphone.Text = "";
            _Gender.SelectedIndex = 0;
        }
        //Method to edit entries
        private async void EditItem_Click(object sender, RoutedEventArgs e)
        {
            string
            fname = _Fname.Text.ToString(),
              lname = _Lname.Text.ToString(),
              dob = _Dob.Text.ToString(),
              gender = _Gender.SelectionBoxItem.ToString(),
              mail = _Email.Text.ToString(),
              mobile = _Mphone.Text;
            if (fname == "")
            {
                var dialogMessage = new MessageDialog("First name cannot be left blank.");
                await dialogMessage.ShowAsync();
                _Fname.Focus(FocusState.Programmatic);
                return;
            }
            else if (lname == "")
            {
                var dialogMessage = new MessageDialog("Last name cannot be left blank.");
                await dialogMessage.ShowAsync();
                _Lname.Focus(FocusState.Programmatic);
                return;
            }
            else if (mobile == "")
            {
                var dialogMessage = new MessageDialog("Mobile number cannot be left blank.");
                await dialogMessage.ShowAsync();
                _Mphone.Focus(FocusState.Programmatic);
                return;
            }
            else if (mail == "")
            {
                var dialogMessage = new MessageDialog("Email address cannot be left blank.");
                await dialogMessage.ShowAsync();
                _Email.Focus(FocusState.Programmatic);
                return;
            }
            else if (dob == "")
            {
                var dialogMessage = new MessageDialog("Birthday cannot be left blank.");
                await dialogMessage.ShowAsync();
                _Dob.Focus(FocusState.Programmatic);
                return;
            }
            foreach (char c in fname)
            {
                if (!Char.IsLetter(c))
                {
                    var dialogMessage = new MessageDialog("First name can only contain letters.");
                    await dialogMessage.ShowAsync();
                    _Fname.Focus(FocusState.Programmatic);
                    _Fname.SelectAll();
                    return;
                }
            }
            foreach (char z in lname)
            {
                if (!Char.IsLetter(z))
                {
                    var dialogMessage = new MessageDialog("Last name can only contain letters.");
                    await dialogMessage.ShowAsync();
                    _Lname.Focus(FocusState.Programmatic);
                    _Lname.SelectAll();
                    return;
                }
            }
            if (!Regex.IsMatch(mail, MAIL_PATTEN))
            {
                var dialogMessage = new MessageDialog("Invalid email address.");
                await dialogMessage.ShowAsync();
                _Email.Focus(FocusState.Programmatic);
                _Email.SelectAll();
                return;
            }
            try
            {
                DateTime tryDob = DateTime.Parse(dob);
            }
            catch
            {
                var dialogMessage = new MessageDialog("Invalid birthday.");
                await dialogMessage.ShowAsync();
                _Dob.Focus(FocusState.Programmatic);
                _Dob.SelectAll();
                return;
            }
            try
            {
                long.Parse(_Mphone.Text);
            }
            catch
            {
                var dialogMessage = new MessageDialog("Mobile number must be numeric only.");
                await dialogMessage.ShowAsync();
                _Mphone.Focus(FocusState.Programmatic);
                _Mphone.SelectAll();
                return;
            }

            //null
            if (PersonalInfoView.SelectedItem == null)
            {
                var dialogMessage = new MessageDialog("No item selected.");
                await dialogMessage.ShowAsync();
                return;
            }
            else
            {
                try
                {
                    int AccSelection = ((PersonalInfo)PersonalInfoView.SelectedItem).ID;
                    conn.CreateTable<PersonalInfo>();
                    var table = conn.Table<PersonalInfo>();
                    PersonalInfo updateQuerry = conn.Get<PersonalInfo>(AccSelection);
                    updateQuerry.FirstName = fname;
                    updateQuerry.LastName = lname;
                    updateQuerry.DoB = dob;
                    updateQuerry.Email = mail;
                    updateQuerry.MobilePhone = mobile;
                    updateQuerry.Gender = gender;
                    conn.Update(updateQuerry);
                    PersonalInfoView.ItemsSource = table.ToList();
                }
                catch
                {
                    var dialogMessage = new MessageDialog("Email address or mobile phone already exists, try using a different one.");
                    await dialogMessage.ShowAsync();
                    return;
                }
                ResetPersonalInfo();
            }
        }

        private async void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (PersonalInfoView.SelectedItem == null)
                {
                    MessageDialog dialog = new MessageDialog("No item selected.", "Oops..!");
                    await dialog.ShowAsync();
                    return;
                }
                else
                {
                    int AccSelection = ((PersonalInfo)PersonalInfoView.SelectedItem).ID;
                    conn.CreateTable<PersonalInfo>();
                    var query1 = conn.Table<PersonalInfo>();
                    var query3 = conn.Query<PersonalInfo>("DELETE FROM PersonalInfo WHERE ID ='" + AccSelection + "'");
                    PersonalInfoView.ItemsSource = query1.ToList();
                    ResetPersonalInfo();
                }
            }
            catch (NullReferenceException)
            {
                MessageDialog dialog = new MessageDialog("Not selected the Item", "Oops..!");
                await dialog.ShowAsync();
            }
        }

        private async void List_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (PersonalInfoView.SelectedItem == null)
                {
                    return;
                }
                else
                {
                    int AccSelection = ((PersonalInfo)PersonalInfoView.SelectedItem).ID;
                    var table = conn.Table<PersonalInfo>();
                    PersonalInfo objectInfo = conn.Get<PersonalInfo>(AccSelection);
                    _Fname.Text = objectInfo.FirstName;
                    _Lname.Text = objectInfo.LastName;
                    _Dob.Text = objectInfo.DoB;
                    _Email.Text = objectInfo.Email;
                    _Mphone.Text = objectInfo.MobilePhone;
                    _Gender.SelectedItem = objectInfo.Gender;
                }
            }
            catch (NullReferenceException)
            {
                MessageDialog dialog = new MessageDialog("No item selected", "Oops..!");
                await dialog.ShowAsync();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Results();
        }
    }
}