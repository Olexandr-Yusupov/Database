using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabaDb2
{
    class Randomizer
    {
        Controller controller;
        private static Random random = new Random();


        public Randomizer(Controller controller)
        {
            this.controller = controller;
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static int PickRandomId(List<int> list)
        {
            int value;
            do
            {
                value = random.Next(1000000000);
            }
            while (list.Contains(value));

            return value;
        }

        public static int PickRandomContainedId(List<int> list)
        {
            return list[random.Next(0, list.Count)];
        }

        public bool Randomize(string tablename, int count)
        {
            Model model;
            for (int i = 0; i < count; i++)
            {
                model = controller.Fetch();
                switch (tablename)
                {
                    case "Offers":
                        if (!controller.InsertRecord(tablename, new Model.Offer(RandomString(10), DateTime.Now, random.Next(20), random.Next(20), 
                            PickRandomId(model.offers.Select(o => o.OfferId).ToList()))))
                            return false;
                        break;
                    case "Orders":
                        if (!controller.InsertRecord(tablename, new Model.Order(DateTime.Now, PickRandomId(model.orders.Select(o => o.OrderId).ToList()),
                            PickRandomContainedId(model.offers.Select(o => o.OfferId).ToList()), PickRandomContainedId(model.users.Select(o => o.UserId).ToList()))))
                            return false;
                        break;
                    case "Phone_numbers":
                        if (!controller.InsertRecord(tablename, new Model.Phone_number(PickRandomId(model.phonenumbers.Select(o => o.PhoneId).ToList()), 
                            RandomString(10), PickRandomContainedId(model.users.Select(o => o.UserId).ToList()))))
                            return false;
                        break;
                    case "Support_agents":
                        if (!controller.InsertRecord(tablename, new Model.Support_agent(PickRandomId(model.supportagents.Select(o => o.SupportAgentId).ToList()), 
                            RandomString(10), RandomString(10), RandomString(10))))
                            return false;
                        break;
                    case "Tickets":
                        if (!controller.InsertRecord(tablename, new Model.Ticket(PickRandomId(model.tickets.Select(o => o.TicketId).ToList()), 
                            RandomString(10), RandomString(10), RandomString(10), PickRandomContainedId(model.users.Select(o => o.UserId).ToList()), PickRandomContainedId(model.supportagents.Select(o => o.SupportAgentId).ToList()))))
                            return false;
                        break;
                    case "Users":
                        if (!controller.InsertRecord(tablename, new Model.User(PickRandomId(model.users.Select(o => o.UserId).ToList()), 
                            RandomString(10), RandomString(10), RandomString(10), RandomString(10), RandomString(10), RandomString(10))))
                            return false;
                        break;
                }
            }
            return true;
        }
    }
}
