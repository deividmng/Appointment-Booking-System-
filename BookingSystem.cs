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
        /// 
        // Reference: https://www.w3schools.com/cs/cs_lists.php
        // Lists are used to store a dynamic collection of objects (Patients).
        private List<Patient> allPatients = new List<Patient>();

        /// <summary>
        /// A reference variable (Pointer) that tracks the "Active User" in the session.
        /// It is marked as nullable (?) because at the start of the program, 
        /// no patient is selected (it is null).
        /// </summary>
        // Reference: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/nullable-reference-types
// The '?' indicates that currentPatient can be null if no patient is currently logged in.
        private Patient? currentPatient;

        /// <summary>
        /// A collection that stores all successful bookings.
        /// It enables the system to generate reports or count the total number of appointments.
        /// </summary>
        private List<Appointment> allAppointments = new List<Appointment>();

        /// <summary>
        /// A fixed-size array that stores only the last 3 actions (Activity Log requirement).
        /// </summary>
        // Reference: https://www.w3schools.com/cs/cs_classes.php
// This method creates a new instance of the Patient class using the provided name.
        private string[] activityLog = new string[3];
        private int activityCount = 0; // Track number of activities stored
        
        public void CreatePatient(string name)
        {
            // --- 1. DATA NORMALIZATION ---
            // Remove spaces and convert to lowercase for consistency usind.Replace method to delete the spaces and .ToLower to conver to lowerCasa
            // Reference: https://www.w3schools.com/cs/cs_strings_methods.php
// Cleaning the input by removing spaces and converting to lowercase for consistency.
            string cleanName = name.Replace(" ", "").ToLower();

            // --- 2. SECURITY CHECK (LETTERS ONLY) ---
            // If the name is empty or contains non-alphabetic characters (numbers, symbols),
            // we stop the execution to prevent "Dirty Data" from entering the list. using IsNullOrWhite prevent to create a pacient without a name, .All(char.IsLetter) ensure that the var is only alfabethic chars <used this mrthods for segurity
            // Reference: https://learn.microsoft.com/en-us/dotnet/api/system.string.isnullorwhitespace

            if (string.IsNullOrWhiteSpace(cleanName) || !cleanName.All(char.IsLetter))
            {
// In cases one of then using the logical OR operator ||   ! will makes sure if at least one condition is met
                Console.WriteLine("[ERROR] Critical Error: Name must contain letters only.");
                return; // Exit the function immediately
            }

            // --- 3. OBJECT INSTANTIATION ---
            // Create the instance with the validated and cleaned name as a cleanName as a parramt, cleand of.spaces and number or error

            Patient newPatient = new Patient(cleanName);

            // --- 4. PERSISTENCE & SESSION STATE ---
            // Archive it in the global list and set as the active patient appening adding the newPatient to the allPatients "list" 
            // Reference: https://www.w3schools.com/cs/cs_lists.php
// Adding the new object to the collection and setting it as the active session patient.
            allPatients.Add(newPatient);
            currentPatient = newPatient;

            // --- 5. LOGGING ---
// Calling the metohd AddActivy, that is in charge to to save the activit to be available to display after 
            AddActivity("Registered and selected patient: " + cleanName);
        }




        /// <summary>
        /// Handles the process of booking a medical appointment.
        /// It manages user selection, price assignment, and date validation.
        /// </summary>
        public void BookAppointment()
        {

            // --- case  1 and 2 . DISPLAY SERVICE MENU  and do the booking ---
            // Presents the available options and their costs to the user.
            string type = "";
            double price = 0;
            int choice;

            while (true)
            {
                Console.WriteLine("1. General Consultation (£35)");
                Console.WriteLine("2. Nurse Check-up (£20)");
                Console.WriteLine("3. Blood Test (£15)");
                Console.WriteLine("4. Specialist Consultation (£60)");
                Console.Write("Select choice: ");

                string? input = Console.ReadLine();
                if (string.IsNullOrEmpty(input) || !int.TryParse(input, out choice))
                {
                    Console.WriteLine("Invalid selection. Please enter a number from 1 to 4.");
                    continue;
                }

                switch (choice)
                {
                    case 1: type = "General Consultation"; price = 35; break;
                    case 2: type = "Nurse Check-up"; price = 20; break;
                    case 3: type = "Blood Test"; price = 15; break;
                    case 4: type = "Specialist Consultation"; price = 60; break;
                    default:
                        Console.WriteLine("Invalid selection. Please enter a number from 1 to 4.");
                        continue;
                }

                break;
            }



            // --- 3. INPUT VALIDATION (DATE) ---
            // Ensures the date is in the correct format and not in the past.
            DateTime date;
            // https://www.w3schools.com/cs/cs_booleans.php for the bool
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
            // Ensures the time follows the HH:mm format and falls within operating hours. string? to be sure that it containt data 
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
                // Reference: https://www.w3schools.com/tools/tool_regex.php
                string openingHoursPattern = @"^((09|[1][0-9]|20):[0-5][0-9]|21:00)$";

                if (Regex.IsMatch(time, openingHoursPattern))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Patient created: {currentPatient?.FullName ?? "Unknown"}.");
                    Console.ResetColor();
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
        //! -- Rmb it is  just 3 
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
                // --- FOR LOOP ITERATION ---
// Reference: https://www.w3schools.com/cs/cs_for_loop.php
// Iterating a specific number of times to process or display data records.
                for (int i = 0; i < 2; i++)
                {
                    // as in the index start on 0 and + 1 to start to count on 1
                    activityLog[i] = activityLog[i + 1];
                }
                activityLog[2] = action;
            }
        }



        ///?----Case 2 ----
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
                // Reference: https://www.w3schools.com/cs/cs_operators.php
// Providing a default value ("N/A") if the patient name is null to prevent display errors.
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

// Sorting the list in descending order based on the 'Price' property using a comparison delegate.
//? This metod the Sort is to comparate the price up to the botton, os this way will comes the Highest to the lowest 
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
        /// CASE 4: ACTIVITY LOG VIEWER
        /// Displays the last 3 actions performed in the system.
        /// </summary>
        public void ShowActivityLog()
        {
            Console.WriteLine("\n--- Activity Log (Last 3 Actions) ---");
            //! this is not need as one act
            // if (activityCount == 0)
            // {
            //     Console.WriteLine("No activities recorded yet.");
            //     return;
            // }

            for (int i = 0; i < activityCount; i++)
            {
                if (!string.IsNullOrEmpty(activityLog[i]))
                {
                    Console.WriteLine($"- {activityLog[i]}");
                }
            }
        }


        /// <summary>
        /// CASE 5: CLEAR BOOKING
        /// Allows user to select which booking to delete from the list.
        /// </summary>
        public void ClearBooking()
        {
            // --- 1. COLLECTION STATE VALIDATION --- IT prevent the erron in case the list is empty 
            if (allAppointments.Count == 0)
            {
                Console.WriteLine("No bookings to clear.");
                return;
            }

            // --- 2. DYNAMIC MENU GENERATION --- 
            // --- Using lool to display the data
            Console.WriteLine("\n--- Select Booking to Delete ---");
            for (int i = 0; i < allAppointments.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {allAppointments[i].PatientName} - {allAppointments[i].AppointmentType} (£{allAppointments[i].Price})");
            }
            Console.Write("Enter number to delete (0 to cancel): ");

            // --- 3. INPUT PARSING ---
            string? input = Console.ReadLine();
            // Validates that the input is a valid integer to avoid "FormatException"
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
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid selection.");
                Console.ResetColor();
                return;
            }

            // Store reference before deletion
            // Delete the specific object
            var removed = allAppointments[choice - 1];

            // Remove the appointment
            allAppointments.RemoveAt(choice - 1);

            // --- 6. LOGGING ---
            // --- Callign the methid AddActivity to keep the acciont
            
            
            AddActivity($"Deleted booking: {removed.AppointmentType} for {removed.PatientName}");

            // --- 7. FEEDBACK ---
            // --- Giving to the user a noticication as the choice as been delete 
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Booking #{choice} has been deleted successfully.");
            Console.ResetColor();
        }
    }

}


/// 
///




