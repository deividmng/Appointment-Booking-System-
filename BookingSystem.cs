using System;
using System.Collections.Generic;
using System.Text.RegularExpressions; // Required for Regex

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
        /// A fixed-size array that stores only the last 3 actions (Activity Log requirement).
        /// </summary>
        private string[] activityLog = new string[3];
        private int activityCount = 0; // Track number of activities stored
        public void CreatePatient(string name)
        {
            // --- 1. DATA NORMALIZATION ---
            // Remove spaces and convert to lowercase for consistency
            string cleanName = name.Replace(" ", "").ToLower();

            // --- 2. SECURITY CHECK (LETTERS ONLY) ---
            // If the name is empty or contains non-alphabetic characters (numbers, symbols),
            // we stop the execution to prevent "Dirty Data" from entering the list.
            if (string.IsNullOrWhiteSpace(cleanName) || !cleanName.All(char.IsLetter))
            {
                Console.WriteLine("[ERROR] Critical Error: Name must contain letters only.");
                return; // Exit the function immediately
            }

            // --- 3. OBJECT INSTANTIATION ---
            // Create the instance with the validated and cleaned name
            Patient newPatient = new Patient(cleanName);

            // --- 4. PERSISTENCE & SESSION STATE ---
            // Archive it in the global list and set as the active patient
            allPatients.Add(newPatient);
            currentPatient = newPatient;

            // --- 5. LOGGING ---
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
            // Ensures the date is in the correct format and not in the past.
            DateTime date;
            while (true)
            {
                Console.Write("Enter date (yyyy-mm-dd): ");
                string? dateInput = Console.ReadLine();

                if (DateTime.TryParse(dateInput, out date) && date >= DateTime.Today)
                {
                    break; // Valid future date provided
                }
                Console.WriteLine("Invalid date. Please use YYYY-MM-DD (e.g., 2026-05-20) and ensure it's not a past date.");
            }

            // --- 4. DATA CAPTURE (STRICT BUSINESS HOURS: 09:00 - 21:00) ---
            // Ensures the time follows the HH:mm format and falls within operating hours.
            string? time = "";
            while (true)
            {
                Console.Write("Enter time (HH:mm) [Opening hours: 09:00 - 21:00]: ");
                time = Console.ReadLine() ?? "";

                // Regex Pattern Logic:
                // ^                 : Start of string
                // (09|[1][0-9]|20)  : Matches 09, 10-19, or 20 as hours
                // :[0-5][0-9]       : Matches any minute from :00 to :59
                // |21:00            : Specifically allows the closing time of 21:00
                // $                 : End of string
                string openingHoursPattern = @"^((09|[1][0-9]|20):[0-5][0-9]|21:00)$";

                if (Regex.IsMatch(time, openingHoursPattern))
                {
                    break; // Valid format and within business hours
                }

                Console.WriteLine("Invalid input! Please enter a time between 09:00 and 21:00 in HH:mm format.");
            }

            // --- 5. INSTANTIATION AND RECORDING ---
            // Creates a new Appointment object and adds it to the global list for reporting.
            string patientName = currentPatient?.FullName ?? "Unknown";
            string patientEmail = currentPatient?.Email ?? "unknown@regenthealth.com";
            currentAppointment = new Appointment(type, date, time, price, patientName, patientEmail);
            allAppointments.Add(currentAppointment);

            // Updates the activity log to track system usage.
            AddActivity("Booked " + type);
        }

        // Placeholder for the AddActivity method to avoid errors
        public void AddActivity(string action)
        {
            if (activityCount < 3)
            {
                activityLog[activityCount] = action;
                activityCount++;
            }
            else
            {
                // Shift all elements left and add new one at the end
                for (int i = 0; i < 2; i++)
                {
                    activityLog[i] = activityLog[i + 1];
                }
                activityLog[2] = action;
            }
        }



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
                Console.WriteLine($"£{a.Price} - {a.AppointmentType} Name: ({a.PatientName})");
            }

            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"Highest Cost: £{sorted[0].Price} - {sorted[0].AppointmentType}");
            Console.WriteLine($"Lowest Cost: £{sorted[sorted.Count - 1].Price} - {sorted[sorted.Count - 1].AppointmentType}");
            Console.WriteLine("======================================");
        }





        /// <summary>
        /// CASE 5: ACTIVITY LOG VIEWER
        /// Displays the last 3 actions performed in the system.
        /// </summary>
        public void ShowActivityLog()
        {
            Console.WriteLine("\n--- Activity Log (Last 3 Actions) ---");

            if (activityCount == 0)
            {
                Console.WriteLine("No activities recorded yet.");
                return;
            }

            for (int i = 0; i < activityCount; i++)
            {
                if (!string.IsNullOrEmpty(activityLog[i]))
                {
                    Console.WriteLine($"- {activityLog[i]}");
                }
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




