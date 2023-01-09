using System;
using System.Collections.Generic;
using Npgsql;

namespace LabaDb2
{
    class Controller
    {
        DB_helper database;

        public Controller(DB_helper dB_Helper)
        {
            database = dB_Helper;
        }

        public Model Fetch()
        {
            Model model = new Model();

            try
            {

                foreach (var offer in ReadRecords("Offers"))
                    model.offers.Add((Model.Offer)offer);

                foreach (var order in ReadRecords("Orders"))
                    model.orders.Add((Model.Order)order);

                foreach (var phonenumber in ReadRecords("Phone_numbers"))
                    model.phonenumbers.Add((Model.Phone_number)phonenumber);

                foreach (var supportagent in ReadRecords("Support_agents"))
                    model.supportagents.Add((Model.Support_agent)supportagent);

                foreach (var ticket in ReadRecords("Tickets"))
                    model.tickets.Add((Model.Ticket)ticket);

                foreach (var user in ReadRecords("Users"))
                    model.users.Add((Model.User)user);

            }
            catch
            {
                return null;
            }

            return model;
        }

        public List<object> ReadRecords(string table)
        {
            string request = $"SELECT * FROM public.\"{table}\"";

            NpgsqlDataReader response;
            if ((response = database.StrResponseCommand(request, out string error)) == null)
            {
                Console.WriteLine("Error, while reading info from table.\n" + error);
                return null;
            }

            List<object> records = new List<object>();

            try
            {
                switch (table)
                {
                    case "Offers":
                        while (response.Read())
                            records.Add(new Model.Offer(response.GetString(0), response.GetDateTime(1)
                                , response.GetInt32(2), response.GetInt32(3), response.GetInt32(4)));
                        break;
                    case "Orders":
                        while (response.Read())
                            records.Add(new Model.Order(response.GetDateTime(0), response.GetInt32(1)
                                , response.GetInt32(2), response.GetInt32(3)));
                        break;
                    case "Phone_numbers":
                        while (response.Read())
                            records.Add(new Model.Phone_number(response.GetInt32(0), response.GetString(1)
                                , response.GetInt32(2)));
                        break;
                    case "Support_agents":
                        while (response.Read())
                            records.Add(new Model.Support_agent(response.GetInt32(0), response.GetString(1)
                                , response.GetString(2), response.GetString(3)));
                        break;
                    case "Tickets":
                        while (response.Read())
                            records.Add(new Model.Ticket(response.GetInt32(0), response.GetString(1)
                                , response.GetString(2), response.GetString(3), response.GetInt32(4), response.GetInt32(5)));
                        break;
                    case "Users":
                        while (response.Read())
                            records.Add(new Model.User(response.GetInt32(0), response.GetString(1), response.GetString(2), response.GetString(3)
                                , response.GetString(4), response.GetString(5), response.GetString(6)));
                        break;
                }

                response.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error, while parsing data.\n" + e.Message);
                return null;
            }

            return records;
        }

        public int DeleteRecord(string table, string keyname, string keyvalue)
        {
            string request = $"DELETE FROM public.\"{table}\" WHERE {keyname}={keyvalue};";

            return database.IntResponseCommand(request, out string error);
        }

        public bool UpdateRecord(string table, object data)
        {

            string request = $"UPDATE public.\"{table}\" ";

            switch (table)
            {
                case "Offers":
                    Model.Offer offer = data as Model.Offer;
                    request += $"SET title=\'{offer.Title}\', delivery_time=\'{offer.DeliveryTime}\', stock={offer.Stock}, price={offer.Price}" +
                        $" WHERE offer_id={offer.OfferId};";
                    break;
                case "Orders":
                    Model.Order order = data as Model.Order;
                    request += $"SET datetime=\'{order.Date}\', offer_id={order.OfferId}, user_id={order.UserId} WHERE order_id={order.OrderId};";
                    break;
                case "Phone_numbers":
                    Model.Phone_number phone_Number = data as Model.Phone_number;
                    request += $"SET country=\'{phone_Number.Country}\', user_id={phone_Number.UserId} WHERE phone_id={phone_Number.PhoneId};";
                    break;
                case "Support_agents":
                    Model.Support_agent support_Agent = data as Model.Support_agent;
                    request += $"SET username=\'{support_Agent.Username}\', name=\'{support_Agent.Name}\', surname=\'{support_Agent.Surname}\' WHERE support_agent_id={support_Agent.SupportAgentId};";
                    break;
                case "Tickets":
                    Model.Ticket ticket = data as Model.Ticket;
                    request += $"SET title=\'{ticket.Title}\', category=\'{ticket.Category}\', main_part=\'{ticket.MainPart}\', user_id={ticket.UserId}, support_agent_id={ticket.SupportAgentId} WHERE ticket_id={ticket.TicketId};";
                    break;
                case "Users":
                    Model.User user = data as Model.User;
                    request += $"SET username=\'{user.Username}\', name=\'{user.Name}\', surname=\'{user.Surname}\', country=\'{user.Country}\', prefered_language=\'{user.PreferedLanguage}\', email=\'{user.Email}\' WHERE user_id={user.UserId};";
                    break;
            }

            if (database.IntResponseCommand(request, out string error) == -1)
            {
                Console.WriteLine("Error while inserting data.\n" + error);
                return false;
            }
            else
                return true;
        }

        public bool InsertRecord(string table, object data)
        {
            string request = $"INSERT into public.\"{table}\"";

            switch (table)
            {
                case "Offers":
                    Model.Offer offer = data as Model.Offer;
                    request += $" (title, delivery_time, stock, price, offer_id) VALUES " +
                        $"(\'{offer.Title}\', \'{offer.DeliveryTime}\', {offer.Stock}, {offer.Price}, {offer.OfferId})";
                    break;
                case "Orders":
                    Model.Order order = data as Model.Order;
                    request += $" (datetime, order_id, offer_id, user_id) VALUES " +
                        $"(\'{order.Date}\', {order.OrderId}, {order.OfferId}, {order.UserId})";
                    break;
                case "Phone_numbers":
                    Model.Phone_number phone_Number = data as Model.Phone_number;
                    request += $" (phone_id, country, user_id) VALUES " +
                        $"({phone_Number.PhoneId}, \'{phone_Number.Country}\', {phone_Number.UserId})";
                    break;
                case "Support_agents":
                    Model.Support_agent support_Agent = data as Model.Support_agent;
                    request += $" (support_agent_id, username, name, surname) VALUES " +
                        $"({support_Agent.SupportAgentId}, \'{support_Agent.Username}\', \'{support_Agent.Name}\', \'{support_Agent.Surname}\')";
                    break;
                case "Tickets":
                    Model.Ticket ticket = data as Model.Ticket;
                    request += $" (ticket_id, title, category, main_part, user_id, support_agent_id) VALUES " +
                        $"({ticket.TicketId}, \'{ticket.Title}\', \'{ticket.Category}\', \'{ticket.MainPart}\', {ticket.UserId}, {ticket.SupportAgentId})";
                    break;
                case "Users":
                    Model.User user = data as Model.User;
                    request += $" (user_id, username, name, surname, country, prefered_language, email) VALUES " +
                        $"({user.UserId}, \'{user.Username}\', \'{user.Name}\', \'{user.Surname}\', \'{user.Country}\', \'{user.PreferedLanguage}\', \'{user.Email}\')";
                    break;
            }


            if (database.IntResponseCommand(request, out string error) == -1)
            {
                Console.WriteLine("Error while inserting data" + error);
                return false;
            }
            else
                return true;
        }

        public List<string> SearchOffersOrdersUsers(int price, int offerid, int userid)
        {
            string request = $"select one.title, one.stock, one.offer_id, three.name, three.surname, three.country, three.email from public.\"Offers\" as one inner join public.\"Orders\" as two on one.\"offer_id\" = two.\"offer_id\" inner join public.\"Users\" as three on " +
            $"three.\"user_id\"=two.\"user_id\" where one.price<{price} and two.offer_id<{offerid} and three.user_id<{userid}";

            Console.WriteLine(request);

            List<string> str = new List<string>();
            NpgsqlDataReader response;
            if ((response = database.StrResponseCommand(request, out string error)) == null)
            {
                Console.WriteLine("Error, while reading info from table.\n" + error);
                return str;
            }
            else
            {
                while (response.Read())
                    str.Add($"{response.GetString(0)}, {response.GetInt32(1)}, {response.GetInt32(2)}, {response.GetString(3)}, {response.GetString(4)}, {response.GetString(5)}, {response.GetString(6)}");
            }
            response.Close();

            return str;
        }

        public List<string> SearchOffersOrders(string title, DateTime starttime, DateTime endtime)
        {
            string request = $"select  one.price, one.title, two.user_id, two.order_id  from public.\"Offers\" as one inner join public.\"Orders\" as two on one.\"offer_id\" = two.\"offer_id\" where one.title LIKE \'{title}\' and two.datetime BETWEEN \'{starttime}\' AND \'{endtime}\'";

            List<string> str = new List<string>();
            NpgsqlDataReader response;
            if ((response = database.StrResponseCommand(request, out string error)) == null)
            {
                Console.WriteLine("Error, while reading info from table.\n" + error);
                return str;
            }
            else
            {
                while (response.Read())
                    str.Add($"{response.GetInt32(0)}, {response.GetString(1)}, {response.GetInt32(2)}, {response.GetInt32(3)}");
            }
            response.Close();

            return str;
        }

        public List<string> SearchUsersTickets(int supportagentid)
        {
            string request = $"select one.user_id, one.name, one.surname, two.title, two.ticket_id from public.\"Users\" as one inner join public.\"Tickets\" as two on one.\"user_id\" = two.\"user_id\" where two.support_agent_id = {supportagentid}";

            List<string> str = new List<string>();
            NpgsqlDataReader response;
            if ((response = database.StrResponseCommand(request, out string error)) == null)
            {
                Console.WriteLine("Error, while reading info from table.\n" + error);
                return str;
            }
            else
            {
                while (response.Read())
                    str.Add($"{response.GetInt32(0)}, {response.GetString(1)}, {response.GetString(2)}, {response.GetString(3)}, {response.GetInt32(4)}");
            }
            response.Close();

            return str;
        }
    }
}
