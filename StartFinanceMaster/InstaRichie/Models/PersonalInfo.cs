// **************************************************************************
//Start Finance - An to manage your personal finances.
//Copyright(C) 2016  Jijo Bose

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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;

namespace StartFinance.Models
{
    public class PersonalInfo
    {
        [PrimaryKey,AutoIncrement]
        public int ID { get; set; }

        //TODO -- Personal Info(e.g.PersonalID, FirstName, LastName, DOB, Gender, Email Address, MobilePhone)
        [NotNull]
        public string FirstName { get; set; }

        [NotNull]
        public string LastName { get; set; }

        [NotNull]
        public string DoB { get; set; }

        [NotNull]
        public string Gender { get; set; }

        [NotNull, Unique]
        public string Email { get; set; }

        [NotNull, Unique]
        public string MobilePhone { get; set; }

    }
}
