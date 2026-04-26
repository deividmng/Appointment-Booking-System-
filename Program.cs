
using System;

namespace RegentHealthBookingSystem
{
    class Program
    {
        /// <summary>
        /// Handles the user autentication cheking the credentials Username and Password 
        /// and assing in to the variable U for user and P for password
        /// Returns true if credentials matching eachother and false in case they are no equal and in case it mach boolea will come true.
        /// </summary>

        // This is a method definition that returns a true/false value (bool),
        static bool Login()
        {
            // The user to enter their username 
            Console.Write("Username: ");
            // Geting user input and store it in variable 'u'
            string? u = Console.ReadLine();
            // Geting user the password
            Console.Write("Password: ");
            // Geting  user input and store "assing" it in variable 'p'
            string? p = Console.ReadLine();


            // Conditional statement if  to validate credentials of the user using a if stamente 
            if (u == "d" && p == "r")
            {
                Console.ForegroundColor = ConsoleColor.Green;

                // ASCII Art Doctor
                Console.WriteLine(@"
            [Doctor]
            \O /
            /|   
            / \  
    ");

                Console.WriteLine("========================================");
                Console.WriteLine($"  WELCOME, DR. {u.ToUpper()}");
                Console.WriteLine("  System Access: GRANTED");
                Console.WriteLine("========================================");
                Console.ResetColor();
                return true;
            }
            // If credentials do not match eachother,  will display an error message and the loop will keep in a loop 
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Incorrect credentials.");
            Console.ResetColor();
            return false;
        }

        static void Main()
        {
            // Initialize the BookingSystem object to manage patients and appointments
            BookingSystem system = new BookingSystem();

            // Authentication: The '!' (NOT) operator keeps the user in a loop on this way I will be display the menu
            // until the Login method returns true.
            while (!Login()) { }

            // Using thios Boolean is controling the lifecycle of the main menu loop on this way the user can close the menu and it make the avalaite to close the program using on case 6
            bool running = true;

            // Main application loop: stays active until the user selects 'Logout number 7 will close the menu'
            while (running)
            {
                // Display the user interface menu to show the  diferents option 
                Console.WriteLine("\n--- Regent Health Menu ---");
                Console.WriteLine("1. Enter Patient Details & Book");
                Console.WriteLine("2. View Booking Summary");
                Console.WriteLine("3. View Highest & Lowest Cost");
                Console.WriteLine("4. View Activity Log");
                Console.WriteLine("5. Clear Booking");
                Console.WriteLine("6. Logout");
                Console.Write("Select option: ");
                Console.ResetColor();

                // Switch statement to handle the user's menu choice is better to use switch as we have many optios 
                switch (Console.ReadLine())
                {
                    // in cas the user chose one will call the methon CreatePatient
                    case "1":
                        // here I use while true to keep looping on this way if the user makes a mistake will keep loping until it get the data correcly 
                        while (true)
                        {
                            Console.WriteLine("\n--- Registration ---");
                            Console.Write("Enter name (Letters only): ");
                            // gettting the input for the user and assing on input var as a string 
                            string input = Console.ReadLine() ?? "";

                            // The function checks the validity of the input string based on Regular Expression.
                            // Validations: Allows only alphabets (upper or lower case) and space character in the input.
                            //bool isOnlyLetters = System.Text.RegularExpressions.Regex.IsMatch(input, @"^[a-zA-Z\s]+$");
                            // static bool IsValidName(string name)
                            // {
                            //     foreach (char c in name)
                            //         if (!char.IsLetter(c) && c != ' ')
                            //             return false;
                            //     return true;
                            // in step of using this for is beter to user regularEprion
                            // }
                            bool isOnlyLetters = System.Text.RegularExpressions.Regex.IsMatch(input, @"^[a-zA-Z\s]+$");

                            // 2. CHECKING: If it's not empty AND matches the pattern
                            // Validation: Check that the 'isOnlyLetters' regular expression validation passed.
                            // Both must be true for patient registration to continue usind add on the parrams &
                            if (!string.IsNullOrWhiteSpace(input) && isOnlyLetters)
                            {
                                // Calling the Method CreatePatient and passing the data that we get for the user as a var input
                                system.CreatePatient(input);
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine(">> SUCCESS: Patient registered.");
                                Console.ResetColor();
                                break;
                            }
                            else
                            {
                                // If both credentials are wrong, this code snippet will execute 
                                // till the user gives the right information.
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("[ERROR] Use letters only. No numbers or symbols allowed.");
                                Console.ResetColor();
                            }
                        }

                        // This runs after the break and will display the menu of the BookAppointment 
                        system.BookAppointment();
                        break;

                    case "2":
                        // Placeholder to display the current booking summary
                        system.ViewSummary();
                        break;

                    case "3":
                        // Placeholder to display statistics on booking costs
                        system.ShowHighestLowest();
                        break;

                    case "4":
                        // Placeholder to display the last 3 user actions
                        system.ShowActivityLog();
                        break;

                    case "5":
                        // Resets the current booking data
                        system.ClearBooking();
                        Console.WriteLine("Booking cleared.");
                        break;

                    case "6":
                        // Termination logic:" exit the loop" close close menu

                        running = false;
                        break;

                    default:
                        // Error handling for any input that doesn't match options 1-7
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid option. Please chose for any of the options");
                        Console.ResetColor();
                        break;
                }
            }
        }
    }
}
