using System;

namespace RegentHealthBookingSystem
{
    /// <summary>
    /// Represents a medical appointment within the system.
    /// This class encapsulates appointment details, pricing, and business logic classification.
    /// </summary>
    class Appointment
    {


        /// <summary>
        /// Purpose
        //The Classify_appointment class was designed as a Logic Controller, which can automatically classify hospital services according to the cost. Rather than manually entering information, the system analyzes the value of the price attribute and sets a status such as Low Cost, Standard, or Premium.
        /// </summary>

        /// <summary>
        /// This class serves as a utility to categorize appointments based on their financial cost.
        /// </summary>
        public class Classify_appointment
        {
            /// <summary>
            /// Takes a price as input and returns a string classification.
            /// It is 'static' so it can be called directly by the Appointment class.
            /// </summary>
            /// 
            /// 
            /// 
            /// public static string ClassifyAppointmentPrice(double price)
            // Requirements
            // • Returns: "Low Cost", "Standard", or "Premium" using if / else if / else
            // • Must validate: if price < 0 return "Invalid"
            // • Test the method with at least 4 values of your choice and show output (screenshots or
            // console output in appendix)
            // • Save as: Classify_appointment.cs
            public static string ClassifyAppointmentPrice(double price)
            {
                // Validation: price is not a negative number
                if (price < 0) return "Invalid";

                // Logic for "Low Cost": Matches the £20 Follow-up visit
                else if (price <= 20) return "Low Cost";

                // Logic for "Standard": Matches the £35 General Consultation
                else if (price <= 40) return "Standard";

                // Logic for "Premium": Matches the £60 Specialist Appointment
                else return "Premium";
            }
        }
        // --- Private Field Variables (Encapsulation) ---
        // These fields store the core data of the appointment and are 
        // hidden from external classes to ensure data security.
        private string appointmentType;
        private DateTime appointmentDate;
        private string appointmentTime;
        private double price;
        private string classification;

        // --- Public Properties (Accessors) ---
        // These provide read-only access to the private fields, allowing 
        // other parts of the program to display the data without changing it.
        public string AppointmentType { get { return appointmentType; } }
        public DateTime AppointmentDate { get { return appointmentDate; } }
        public string AppointmentTime { get { return appointmentTime; } }
        public double Price { get { return price; } }
        public string Classification { get { return classification; } }

        // --- Constructor ---
        /// <summary>
        /// Initializes a new instance of the Appointment class.
        /// This code block  is the Constructor of the Appointment class.
        /// is passing 3 parramats type as a string , date as a DateTime and time as a stringa and price as a doble as has decimal  
        /// </summary>
        /// 
        public Appointment(string type, DateTime date, string time, double price)
        {
            // Assigning parameter values to the internal private fields
            appointmentType = type;
            appointmentDate = date;
            appointmentTime = time;
            // Use 'this' to distinguish the class field from the constructor parameter.
            // It ensures the value passed (price) is stored in the object's variable (this.price).
            this.price = price;


            // BUSINESS LOGIC INTEGRATION:
            // This line calls a static method from the 'Classify_appointment' class 
            // to automatically categorize the appointment based on its cost.
            classification = Classify_appointment.ClassifyAppointmentPrice(price);
        }
    }
}