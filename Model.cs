using System;
using System.Collections.Generic;

namespace LabaDb2
{
    public class Model
    {
        public static List<string> names = new List<string>()
        {
            "Offers",
            "Orders",
            "Phone_numbers",
            "Support_agents",
            "Tickets",
            "Users"
        };

        public List<Offer> offers = new List<Offer>();
        public List<Order> orders = new List<Order>();
        public List<Phone_number> phonenumbers = new List<Phone_number>();
        public List<Support_agent> supportagents = new List<Support_agent>();
        public List<Ticket> tickets = new List<Ticket>();
        public List<User> users = new List<User>();


        public class Offer
        {
            public string Title { get; }
            public DateTime DeliveryTime { get; }
            public int Stock { get; }
            public int Price { get; }
            public int OfferId { get; }

            public Offer(string title, DateTime deliverytime, int stock, int price, int offerid)
            {
                Title = title;
                DeliveryTime = deliverytime;
                Stock = stock;
                Price = price;
                OfferId = offerid;
            }
        }

        public class Order
        {
            public DateTime Date { get; set; }
            public int OrderId { get; set; }
            public int OfferId { get; set; }
            public int UserId { get; set; }

            public Order(DateTime date, int orderid, int offerid, int userid)
            {
                Date = date;
                OrderId = orderid;
                OfferId = offerid;
                UserId = userid;
            }
        }

        public class Phone_number
        {
            public int PhoneId { get; set; }
            public string Country { get; set; }
            public int UserId { get; set; }

            public Phone_number(int phoneid, string country, int userid)
            {
                PhoneId = phoneid;
                Country = country;
                UserId = userid;
            }
        }

        public class Support_agent
        {
            public int SupportAgentId { get; set; }
            public string Username { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }

            public Support_agent(int supportagentid, string username, string name, string surname)
            {
                SupportAgentId = supportagentid;
                Username = username;
                Name = name;
                Surname = surname;
            }
        }

        public class Ticket
        {
            public int TicketId { get; set; }
            public string Title { get; set; }
            public string Category { get; set; }
            public string MainPart { get; set; }
            public int UserId { get; set; }
            public int SupportAgentId { get; set; }

            public Ticket(int ticketid, string title, string category, string mainpart, int userid, int supportagentid)
            {
                TicketId = ticketid;
                Title = title;
                Category = category;
                MainPart = mainpart;
                UserId = userid;
                SupportAgentId = supportagentid;
            }
        }

        public class User
        {
            public int UserId { get; set; }
            public string Username { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public string Country { get; set; }
            public string PreferedLanguage { get; set; }
            public string Email { get; set; }

            public User(int userid, string username, string name, string surname, string country, string preferedlanguage, string email)
            {
                UserId = userid;
                Username = username;
                Name = name;
                Surname = surname;
                Country = country;
                PreferedLanguage = preferedlanguage;
                Email = email;
            }
        }
    }
}
