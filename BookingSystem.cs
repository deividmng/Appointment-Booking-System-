using System;
using System.Collections.Generic;

namespace RegentHealthBookingSystem
{
    class BookingSystem


    {

        // This variable holds the specific appointment currently being processed.
        // The '?' means it can be null if no appointment has been created yet.
        private Appointment? currentAppointment;

        // Fields required for the method to function
        // --- DATA STORAGE & STATE MANAGEMENT ---

        /// <summary>
        /// This List acts as the central database of the system.
        /// It stores every Patient object created during the current session.
        /// Unlike a fixed array, a List can grow dynamically as more patients are registered.
        /// </summary>
        private List<Patient> allPatients = new List<Patient>();

        /// <summary>
        /// A reference variable (Pointer) that tracks the "Active User" in the session.
        /// It is marked as nullable (?) because at the start of the program, 
        /// no patient is selected (it is null).
        /// </summary>
        private Patient? currentPatient;

        /// <summary>
        /// A collection that stores all successful bookings.
        /// It enables the system to generate reports or count the total number of appointments.
        /// </summary>
        private List<Appointment> allAppointments = new List<Appointment>();

        /// <summary>
        /// A dynamic string collection that records every significant system event.
        /// It provides transparency and allows the user to see a chronological history 
        /// of their actions (Audit Trail).
        /// </summary>
        private List<string> activityLog = new List<string>();
        public void CreatePatient(string name)
        {
            // Clean the name: remove spaces and convert to lowercase
            string cleanName = name.Replace(" ", "").ToLower();

            // 1. Create the instance with cleaned name
            Patient newPatient = new Patient(cleanName);

            // 2. Archive it in the global list (Database)
            allPatients.Add(newPatient);

            // 3. Set as the active patient for immediate booking
            currentPatient = newPatient;

            // 4. Record the history
            AddActivity("Registered and selected patient: " + cleanName);
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
            currentAppointment = new Appointment(type, date, time, price, currentPatient?.FullName, currentPatient?.Email);
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
            // 1. VALIDATION: Check if the list is empty
            if (allAppointments.Count == 0)
            {
                Console.WriteLine("No booking records found.");
                AddActivity("Checked summary (Empty)");
                return;
            }

            Console.WriteLine("\n========== BOOKING SUMMARY ==========");

            // 2. ITERATION: Loop through every appointment in the list
            int count = 1;
            foreach (var appointment in allAppointments)
            {
                Console.WriteLine($"\n--- Booking #{count} ---");
                Console.WriteLine("Patient: " + (appointment.PatientName ?? "N/A"));
                Console.WriteLine("Email: " + (appointment.PatientEmail ?? "N/A"));
                Console.WriteLine("Service: " + appointment.AppointmentType);
                Console.WriteLine("Date: " + appointment.AppointmentDate.ToShortDateString());
                Console.WriteLine("Time: " + appointment.AppointmentTime);
                Console.WriteLine("Price: £" + appointment.Price);
                Console.WriteLine("Category: " + appointment.Classification);
                count++;
            }

            Console.WriteLine("\n======================================");

            // 3. LOGGING
            AddActivity("Viewed booking summary");
        }


        //?----Case 3 ----

        /// <summary>
        /// CASE 4: STATISTICS REPORT
        /// Displays all appointments sorted from highest to lowest cost.
        /// </summary>
        public void ShowHighestLowest()
        {
            // --- 1. COLLECTION VALIDATION ---
            if (allAppointments.Count == 0)
            {
                Console.WriteLine("No data available. Please book appointments first.");
                return;
            }

            // --- 2. SORT BY PRICE (DESCENDING) ---
            // Create a sorted copy to not modify original order
            var sorted = new List<Appointment>(allAppointments);
            sorted.Sort((a, b) => b.Price.CompareTo(a.Price));

            // --- 3. OUTPUT: DISPLAY ALL SORTED ---
            Console.WriteLine("\n========== PRICE STATISTICS ==========");
            Console.WriteLine("\nAll Appointments (Highest to Lowest):");
            Console.WriteLine("----------------------------------------");

            foreach (var a in sorted)
            {
                Console.WriteLine($"£{a.Price} - {a.AppointmentType} ({a.PatientName})");
            }

            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"Highest Cost: £{sorted[0].Price} - {sorted[0].AppointmentType}");
            Console.WriteLine($"Lowest Cost: £{sorted[sorted.Count - 1].Price} - {sorted[sorted.Count - 1].AppointmentType}");
            Console.WriteLine("======================================");
        }





        /// <summary>
        /// CASE 4 ACTIVITY LOG VIEWER
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


        /// <summary>
        /// CASE 6: CLEAR BOOKING
        /// Allows user to select which booking to delete from the list.
        /// </summary>
        public void ClearBooking()
        {
            // --- 1. COLLECTION STATE VALIDATION ---
            if (allAppointments.Count == 0)
            {
                Console.WriteLine("No bookings to clear.");
                return;
            }

            // --- 2. DYNAMIC MENU GENERATION ---
            Console.WriteLine("\n--- Select Booking to Delete ---");
            for (int i = 0; i < allAppointments.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {allAppointments[i].PatientName} - {allAppointments[i].AppointmentType} (£{allAppointments[i].Price})");
            }
            Console.Write("Enter number to delete (0 to cancel): ");

            // --- 3. INPUT PARSING ---
            string? input = Console.ReadLine();
            if (string.IsNullOrEmpty(input) || !int.TryParse(input, out int choice))
            {
                Console.WriteLine("Invalid selection.");
                return;
            }

            // --- 4. ESCAPE CLAUSE ---
            if (choice == 0)
            {
                Console.WriteLine("Operation cancelled.");
                return;
            }

            // --- 5. BOUNDARY CHECKING AND REMOVAL ---
            if (choice < 1 || choice > allAppointments.Count)
            {
                Console.WriteLine("Invalid selection.");
                return;
            }

            // Store reference before deletion
            var removed = allAppointments[choice - 1];

            // Remove the appointment
            allAppointments.RemoveAt(choice - 1);

            // --- 6. LOGGING ---
            AddActivity($"Deleted booking: {removed.AppointmentType} for {removed.PatientName}");

            // --- 7. FEEDBACK ---
            Console.WriteLine($"Booking #{choice} has been deleted successfully.");
        }
    }

}


/// 
///


