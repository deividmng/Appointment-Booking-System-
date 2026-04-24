using System;
using System.Collections.Generic;

namespace RegentHealthBookingSystem
{
    class BookingSystem

    
    {
        
        // Fields required for the method to function
        private Appointment? currentAppointment;
        private List<Appointment> allAppointments = new List<Appointment>();
        private List<string> activityLog = new List<string>();
        private Patient? currentPatient;
        public void CreatePatient(string name)
        {
            // Ahora C# sabe qué es currentPatient
            currentPatient = new Patient(name); 
            AddActivity("Created patient: " + name);
        }


        

        /// <summary>
        /// Handles the process of booking a medical appointment.
        /// It manages user selection, price assignment, and date validation.
        /// </summary>
        public void BookAppointment()
        {
            
            // --- 1. DISPLAY SERVICE MENU ---
            // Presents the available options and their costs to the user.
            Console.WriteLine("1. General Consultation (£35)");
            Console.WriteLine("2. Nurse Check-up (£20)");
            Console.WriteLine("3. Blood Test (£15)");
            Console.WriteLine("4. Specialist Consultation (£60)");
            Console.Write("Select choice: ");

            // Converts string input to an integer for selection logic.
            string? input = Console.ReadLine();
            if (string.IsNullOrEmpty(input) || !int.TryParse(input, out int choice))
            {
                Console.WriteLine("Invalid selection.");
                return;
            }

            string type = "";
            double price = 0;

            // --- 2. PRICING LOGIC ---
            // A switch statement is used for efficiency to assign data based on the menu choice.
            switch (choice)
            {
                case 1: type = "General Consultation"; price = 35; break;
                case 2: type = "Nurse Check-up"; price = 20; break;
                case 3: type = "Blood Test"; price = 15; break;
                case 4: type = "Specialist Consultation"; price = 60; break;
                default:
                    Console.WriteLine("Invalid selection.");
                    return;
            }

            // --- 3. INPUT VALIDATION (DATE) ---
            // Ensures the date follows the correct format and is not in the past.
            Console.Write("Enter date (yyyy-mm-dd): ");
            DateTime date;
            if (!DateTime.TryParse(Console.ReadLine(), out date) || date < DateTime.Today)
            {
                Console.WriteLine("Invalid date. Please enter a valid future date.");
                return;
            }

            // --- 4. DATA CAPTURE (TIME) ---
            Console.Write("Enter time (HH:mm): ");
            string? time = Console.ReadLine();
            if (string.IsNullOrEmpty(time))
            {
                Console.WriteLine("Invalid time.");
                return;
            }

            // --- 5. INSTANTIATION AND RECORDING ---
            // Creates a new Appointment object and adds it to the global list for reporting.
            currentAppointment = new Appointment(type, date, time, price);
            allAppointments.Add(currentAppointment);

            // Updates the activity log to track system usage.
            AddActivity("Booked " + type);
        }

        // Placeholder for the AddActivity method to avoid errors
        public void AddActivity(string action) { activityLog.Add(action); }



        ///?----Case 3 ----
        /// <summary>
        /// CASE 3: DISPLAY BOOKING SUMMARY
        /// This method retrieves and formats the stored data from the current session 
        /// to present a readable report to the user.
        /// </summary>
        public void ViewSummary()
        {
            // --- 1. NULL REFERENCE PROTECTION ---
            // Before accessing properties, we check if 'currentAppointment' exists in memory.
            // This prevents a 'NullReferenceException', which would crash the program 
            // if the user selects Option 3 before booking a service.
            if (currentAppointment == null)
            {
                Console.WriteLine("No booking record found. Please complete Case 2 first.");
                return;
            }

            // --- 2. DATA RETRIEVAL AND OUTPUT ---
            // The following lines use string concatenation to display object properties:

            // Accesses the string 'appointmentType' defined during the booking process.
            Console.WriteLine("\nAppointment: " + currentAppointment.AppointmentType);

            // .ToShortDateString() is used to format the DateTime object into a clean 
            // DD/MM/YYYY format, hiding the unnecessary time components (00:00:00).
            Console.WriteLine("Date: " + currentAppointment.AppointmentDate.ToShortDateString());

            // Displays the raw string value stored for the appointment time.
            Console.WriteLine("Time: " + currentAppointment.AppointmentTime);

            // Displays the cost with the British Pound currency symbol.
            Console.WriteLine("Price: £" + currentAppointment.Price);

            // CRITICAL LOGIC DISPLAY: Shows the result of the 'Classify_appointment' call 
            // made inside the Appointment constructor (e.g., Low Cost, Standard, or Premium).
            Console.WriteLine("Classification: " + currentAppointment.Classification);

            // --- 3. SYSTEM TRACEABILITY ---
            // Calls the internal logging method to record that the user accessed this report.
            AddActivity("Viewed summary");
        }



        //?----Case 4 ----

        /// <summary>
        /// CASE 4: STATISTICS REPORT
        /// Analyzes all booked appointments to identify the most and least expensive services.
        /// This method uses a linear search algorithm to compare costs.
        /// </summary>
        public void ShowHighestLowest()
        {
            // --- 1. COLLECTION VALIDATION ---
            // Checks if the list is empty to avoid errors when trying to access index [0].
            if (allAppointments.Count == 0)
            {
                Console.WriteLine("No data available. Please book appointments first.");
                return;
            }

            // --- 2. ALGORITHM INITIALIZATION ---
            // We assume the first appointment in the list is both the highest and lowest 
            // to start the comparison.
            Appointment high = allAppointments[0];
            Appointment low = allAppointments[0];

            // --- 3. LINEAR SEARCH LOGIC (Iteration) ---
            // The foreach loop traverses the entire collection of appointments.
            foreach (var a in allAppointments)
            {
                // Condition to find the maximum value
                if (a.Price > high.Price)
                {
                    high = a; // Updates the high variable if a more expensive price is found
                }

                // Condition to find the minimum value
                if (a.Price < low.Price)
                {
                    low = a; // Updates the low variable if a cheaper price is found
                }
            }

            // --- 4. OUTPUT DISPLAY ---
            // Prints the final results based on the comparisons made in the loop.
            Console.WriteLine("\n--- Price Statistics ---");
            Console.WriteLine("Highest Cost Service: " + high.AppointmentType + " (£" + high.Price + ")");
            Console.WriteLine("Lowest Cost Service: " + low.AppointmentType + " (£" + low.Price + ")");
        }






/// <summary>
/// CASE 5 ACTIVITY LOG VIEWER
/// This method processes the activity history stored in a fixed-size array.
/// It implements logic to handle empty states, providing professional feedback to the user.
/// </summary>
public void ShowActivityLog()
{
    // --- 1. UI HEADER ---
    // Prints a clear section header to separate the log from previous menu options.
    Console.WriteLine("\n--- Activity Log ---");


    // --- 2. BOOLEAN FLAG INITIALIZATION ---
    // 'hasActivity' acts as a sentinel variable. It tracks whether the array 
    // contains any valid strings. This is essential for handling the "Empty State".
    bool hasActivity = false;

    // --- 3. ARRAY TRAVERSAL (FOREACH LOOP) ---
    // The loop iterates through each element 'a' within the 'activityLog' array.
    foreach (string a in activityLog)
    {
        // --- 4. NULL/EMPTY STRING VALIDATION ---
        // Checks if the current element 'a' contains actual text data.
        // Array slots are often null or empty upon system startup.
        if (!string.IsNullOrEmpty(a))
        {
            // If data is found, print it with a bullet point for better readability.
            Console.WriteLine("- " + a);
            
            // Set the flag to true to indicate the system found at least one record.
            hasActivity = true; 
        }
    }

    // --- 5. CONDITIONAL FEEDBACK ---
    // If the loop completes and 'hasActivity' remains false, it means the array 
    // was entirely empty. The system then provides a fallback message.
    if (!hasActivity)
    {
        Console.WriteLine("System Status: No activities recorded in the current session.");
    }
}

    
    }
    
}


/// 
///


