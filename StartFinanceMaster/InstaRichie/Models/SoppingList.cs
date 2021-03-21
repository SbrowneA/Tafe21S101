using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;

namespace StartFinance.Models
{
    class SoppingList
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [Unique]
        public string ItemName { get; set; }

        [NotNull]
        public double Price { get; set; }

        public int Quantity { get; set; }

    }
}
