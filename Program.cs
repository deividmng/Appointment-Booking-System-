
using System;

namespace RegentHealthBookingSystem
{
    class Program
    {
        /// <summary>
        /// Handles the user autentication cheking the credentials Username and Password 
        /// and assing in to the variable U for user and P for password
        /// Returns true if credentials matching eachother and false in case they are no equal.
        /// </summary>

        // This is a method definition that returns a true/false value (bool),
        static bool Login()
        {
            // The user to enter their username 
            Console.Write("Username: ");
            // Geting user input and store it in variable 'u'
            string u = Console.ReadLine();
            // Geting user the password
            Console.Write("Password: ");
            // Geting  user input and store "assing" it in variable 'p'
            string p = Console.ReadLine();

            // Conditional statement if  to validate credentials of the user 
            if (u == "d" && p == "r")
            {
                // If both match, display success message
                Console.WriteLine("Login successful.");
                return true;
            }
            // If credentials do not match, display an error message
            Console.WriteLine("Incorrect credentials.");
            return false;
        }

        /// <summary>
        /// Handles the user autentication cheking the credentials Username and Password 
        /// and assing in to the variable U for user and P for password
        /// Returns true if credentials matching eachother and false in case they are no equal.
        /// </summary>
        static void Main()
        {
            // Initialize the BookingSystem object to manage patients and appointments
            BookingSystem system = new BookingSystem();

            // Authentication: The '!' (NOT) operator keeps the user in a loop on this way I will be display the menu
            // until the Login method returns true.
            while (!Login()) { }

            // Boolean to control the lifecycle of the main menu loop on this way the user can close the menu
            bool running = true;

            // Main application loop: stays active until the user selects 'Logout number 7 will close the menu'
            while (running)
            {
                // Display the user interface menu to show the  diferents option 
                Console.WriteLine("\n--- Regent Health Menu ---");
                Console.WriteLine("1. Enter Patient Details");
                Console.WriteLine("2. Book Appointment");
                Console.WriteLine("3. View Booking Summary");
                Console.WriteLine("4. View Highest & Lowest Cost");
                Console.WriteLine("5. View Activity Log");
                Console.WriteLine("6. Clear Booking");
                Console.WriteLine("7. Logout");
                Console.Write("Select option: ");

                // Switch statement to handle the user's menu choice is better to use switch as we have many optios 
                switch (Console.ReadLine())
                {
                    case "1":
                        // 1. Pedimos el nombre al usuario
                        Console.Write("Enter full name: ");
                        string name = Console.ReadLine();

                        // 2. Usamos tu validación (¡esto es muy bueno!)
                        if (string.IsNullOrWhiteSpace(name) || !IsValidName(name))
                        {
                            Console.WriteLine("Invalid name. Please use letters only.");
                            break;
                        }

                        // 3. LA LLAMADA CLAVE: Enviamos el nombre al método del sistema
                        // Esto conecta tu menú con la lógica que guarda al paciente.
                        system.CreatePatient(name);

                        Console.WriteLine("Patient created successfully.");
                        break;
                    case "2":
                        // Placeholder for booking logic
                        system.BookAppointment();
                        break;

                    case "3":
                        // Placeholder to display the current booking summary
                        system.ViewSummary();
                        break;

                    case "4":
                        // Placeholder to display statistics on booking costs
                        system.ShowHighestLowest();
                        break;

                    case "5":
                        // Placeholder to display the last 3 user actions
                        system.ShowActivityLog();
                        break;

                    case "6":
                        // Resets the current booking data
                        // system.ClearBooking();
                        Console.WriteLine("Booking cleared.");
                        break;

                    case "7":
                        // Termination logic:" exit the loop" close close menu
                        Console.WriteLine("Logged out.");
                        running = false;
                        break;

                    default:
                        // Error handling for any input that doesn't match options 1-7
                        Console.WriteLine("Invalid option. Please chose for any of the options");
                        break;
                }
            }
        }

        static bool IsValidName(string name)
        {
            foreach (char c in name)
                if (!char.IsLetter(c) && c != ' ')
                    return false;
            return true;
        }
    }
}
