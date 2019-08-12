using System;
using System.Collections.Generic;
using System.Text;

namespace AppPFashions
{
    public class MonthsList : List<Month>
    {
        public MonthsList()
        {
            var list = Month.GenerateList();
            this.AddRange(list);
        }
    }
    public class Month
    {
        public Month()
        {
        }
        public string Name { get; set; }
        public int Number { get; set; }
        public int Days { get; set; }
        public string Quarter { get; set; }

        public static List<Month> GenerateList()
        {
            string[] names = {
                "January", "February", "March", "April",
                "May", "June", "July", "August", "September",
                "October", "November", "December"
            };
            int[] days = {
                31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31
            };
            var items = new List<Month>();
            for (int i = 0; i < 12; i++)
            {
                Month monthItem = new Month();
                monthItem.Name = names[i];
                monthItem.Days = days[i];
                monthItem.Number = i + 1;
                monthItem.Quarter = "Q" + ((i / 3) + 1);
                items.Add(monthItem);
            }
            return items;
        }
    }
}
