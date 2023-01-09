using System;

namespace LabaDb2
{
    class View
    {

        private static Controller controller;

        static void Main(string[] args)
        {
            Config config;
            if ((config = Config.Read(out string error)) == null)
            {
                Console.WriteLine("Error, while reading database configuration.\n" + error);
                Console.ReadLine();
                return;
            }

            DB_helper dB_Helper = new DB_helper(config);
            if (!dB_Helper.Connect(out string errorc))
            {
                Console.WriteLine("Error, while connecting to database.\n" + errorc);
                Console.ReadLine();
                return;
            }


            Console.WriteLine("Connecting to db.");

            controller = new Controller(dB_Helper);

            while(true)
                Menu();

        }

        static void Menu()
        {
            ShowMenu();
            MainMenu();
        }

        static void ShowMenu()
        {
            Console.WriteLine("Press '1' to print table.");
            Console.WriteLine("Press '2' to insert record in table.");
            Console.WriteLine("Press '3' to update record in db.");
            Console.WriteLine("Press '4' to delete record from table.");
            Console.WriteLine("Press '5' to search db.");
            Console.WriteLine("Press '6' to add randomized records db.");
        }

        static void MainMenu()
        {
            Model db;
            if ((db = controller.Fetch()) == null)
            {
                Console.WriteLine("Error, while fetching info.");
                return;
            }

            try
            {
                switch ((char)Console.ReadKey().Key)
                {
                    case '1':
                        {
                            Console.WriteLine("\nEnter table name:");
                            string table = Console.ReadLine();

                            switch (table)
                            {
                                case "Offers":
                                    foreach (var offer in db.offers)
                                        Console.WriteLine($"title=\'{offer.Title}\', delivery_time=\'{offer.DeliveryTime}\', stock={offer.Stock}, price={offer.Price} offer_id={offer.OfferId}");
                                    break;
                                case "Orders":
                                    foreach (var order in db.orders)
                                        Console.WriteLine($"datetime=\'{order.Date}\', offer_id={order.OfferId}, user_id={order.UserId}, order_id={order.OrderId}");
                                    break;
                                case "Phone_numbers":
                                    foreach (var phone_Number in db.phonenumbers)
                                        Console.WriteLine($"country=\'{phone_Number.Country}\', user_id={phone_Number.UserId}, phone_id={phone_Number.PhoneId}");
                                    break;
                                case "Support_agents":
                                    foreach (var support_Agent in db.supportagents)
                                        Console.WriteLine($"username={support_Agent.Username}, name={support_Agent.Name}, surname={support_Agent.Surname}, support_agent_id={support_Agent.SupportAgentId}");
                                    break;
                                case "Tickets":
                                    foreach (var ticket in db.tickets)
                                        Console.WriteLine($"title={ticket.Title}, category={ticket.Category}, main_part={ticket.MainPart}, user_id={ticket.UserId}, support_agent_id={ticket.SupportAgentId}, ticket_id={ticket.TicketId}");
                                    break;
                                case "Users":
                                    foreach (var user in db.users)
                                        Console.WriteLine($"username={user.Username}, name={user.Name}, surname={user.Surname}, country={user.Country}, prefered_language={user.PreferedLanguage}, email={user.Email}, user_id={user.UserId}");
                                    break;
                                default:
                                    Console.WriteLine("Wrong table name.");
                                    return;
                            }
                        }
                        break;
                    case '2':
                        {
                            Console.WriteLine("\nEnter table name and object:");
                            string[] table = Console.ReadLine().Split(' ');

                            object obj = null;

                            switch (table[0])
                            {
                                case "Offers":
                                    obj = new Model.Offer(table[1], Convert.ToDateTime(table[2]), Convert.ToInt32(table[3]), Convert.ToInt32(table[4]), Convert.ToInt32(table[5]));
                                    break;
                                case "Orders":
                                    obj = new Model.Order(Convert.ToDateTime(table[1]), Convert.ToInt32(table[2]), Convert.ToInt32(table[3]), Convert.ToInt32(table[4]));
                                    break;
                                case "Phone_numbers":
                                    obj = new Model.Phone_number(Convert.ToInt32(table[1]), table[2], Convert.ToInt32(table[3]));
                                    break;
                                case "Support_agents":
                                    obj = new Model.Support_agent(Convert.ToInt32(table[1]), table[2], table[3], table[4]);
                                    break;
                                case "Tickets":
                                    obj = new Model.Ticket(Convert.ToInt32(table[1]), table[2], table[3], table[4], Convert.ToInt32(table[5]), Convert.ToInt32(table[6]));
                                    break;
                                case "Users":
                                    foreach (var user in db.users)
                                        obj = new Model.User(Convert.ToInt32(table[1]), table[2], table[3], table[4], table[5], table[6], table[7]);
                                    break;
                                default:
                                    Console.WriteLine("Wrong table name.");
                                    return;
                            }

                            if (!controller.InsertRecord(table[0], obj))
                            {
                                Console.WriteLine("Error, while inserting info.");
                                return;
                            }
                            else
                                Console.WriteLine("Record added.");
                        }
                        break;
                    case '3':
                        {
                            Console.WriteLine("\nEnter table name and object:");
                            string[] table = Console.ReadLine().Split(' ');

                            object obj = null;

                            switch (table[0])
                            {
                                case "Offers":
                                    obj = new Model.Offer(table[1], Convert.ToDateTime(table[2]), Convert.ToInt32(table[3]), Convert.ToInt32(table[4]), Convert.ToInt32(table[5]));
                                    break;
                                case "Orders":
                                    obj = new Model.Order(Convert.ToDateTime(table[1]), Convert.ToInt32(table[2]), Convert.ToInt32(table[3]), Convert.ToInt32(table[4]));
                                    break;
                                case "Phone_numbers":
                                    obj = new Model.Phone_number(Convert.ToInt32(table[1]), table[2], Convert.ToInt32(table[3]));
                                    break;
                                case "Support_agents":
                                    obj = new Model.Support_agent(Convert.ToInt32(table[1]), table[2], table[3], table[4]);
                                    break;
                                case "Tickets":
                                    obj = new Model.Ticket(Convert.ToInt32(table[1]), table[2], table[3], table[4], Convert.ToInt32(table[5]), Convert.ToInt32(table[6]));
                                    break;
                                case "Users":
                                    foreach (var user in db.users)
                                        obj = new Model.User(Convert.ToInt32(table[1]), table[2], table[3], table[4], table[5], table[6], table[7]);
                                    break;
                                default:
                                    Console.WriteLine("Wrong table name.");
                                    return;
                            }

                            if (!controller.UpdateRecord(table[0], obj))
                            {
                                Console.WriteLine("Error, while updating info.");
                                return;
                            }
                            else
                                Console.WriteLine("Record updated.");
                        }
                        break;
                    case '4':
                        {
                            Console.WriteLine("\nEnter table name and key, value:");
                            string[] table = Console.ReadLine().Split(' ');

                            if (controller.DeleteRecord(table[0], table[1], table[2]) <= 0)
                            {
                                Console.WriteLine("Error, while deleting info.");
                                return;
                            }
                            else
                                Console.WriteLine("Record deleted.");
                        }
                        break;
                    case '5':
                        {
                            Console.WriteLine("\nSearch in SearchOffersOrdersUsers = 1.");
                            Console.WriteLine("Search in SearchOffersOrders = 2.");
                            Console.WriteLine("Search in SearchUsersTickets = 3.");

                            char method = Console.ReadKey().KeyChar;

                            switch (method)
                            {
                                case '1':
                                    Console.WriteLine("\nEnter price, offerid, userid.");
                                    string[] values1 = Console.ReadLine().Split(' ');

                                    int price = Convert.ToInt32(values1[0]), offerid = Convert.ToInt32(values1[1]), userid = Convert.ToInt32(values1[2]);

                                    foreach (string s in controller.SearchOffersOrdersUsers(price, offerid, userid))
                                        Console.WriteLine(s);

                                    break;
                                case '2':
                                    Console.WriteLine("\nEnter title, starttime, endtime.");
                                    string[] values2 = Console.ReadLine().Split(' ');

                                    DateTime starttime = Convert.ToDateTime(values2[1]), endtime = Convert.ToDateTime(values2[2]);

                                    foreach (string s in controller.SearchOffersOrders(values2[0], starttime, endtime))
                                        Console.WriteLine(s);

                                    break;
                                case '3':
                                    Console.WriteLine("\nEnter supportagentid.");
                                    int values3 = Convert.ToInt32(Console.ReadLine());

                                    foreach (string s in controller.SearchUsersTickets(values3))
                                        Console.WriteLine(s);

                                    break;
                                default:
                                    Console.WriteLine("Wrong search method key.");
                                    return;
                            }

                        }
                        break;
                    case '6':
                        {
                            Console.WriteLine("\nEnter table name and count of records.");
                            Randomizer randomizer = new Randomizer(controller);

                            string[] table = Console.ReadLine().Split(' ');

                            switch (table[0])
                            {
                                case "Offers":
                                case "Orders":
                                case "Phone_numbers":
                                case "Support_agents":
                                case "Tickets":
                                case "Users":
                                    if (!randomizer.Randomize(table[0], Convert.ToInt32(table[1])))
                                    {
                                        Console.WriteLine("Error, while randomizing.");
                                        return;
                                    }
                                    else
                                        Console.WriteLine("Records added.");
                                    break;
                                default:
                                    Console.WriteLine("Wrong table name.");
                                    return;
                            }
                        }
                        break;
                    default:
                        Console.WriteLine("Wrong symbol was entered , pls enter 1 - 6.");
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error! " + e.Message);
                return;
            }
        }
    }
}
