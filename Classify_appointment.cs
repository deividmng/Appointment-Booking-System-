using System;

namespace RegentHealthBookingSystem
{
    /// <summary>
    /// Appointment Utility Function (No OOP) The purpose of this class is for Utility Method based the cost "price" pased as a parramt when the objet is crated maintaining data without out manual intervetion 
    /// This is a static method without using any classes or objects.
    /// </summary>
    public static class on this way the appointment constratur can acces to it 


ClassifyAppointment
    {
        /// <summary>
        /// Classifies appointment price into categories.
        /// </summary>
        /// <param name="price">The price of the appointment</param>
        /// <returns>"Low Cost", "Standard", "Premium", or "Invalid"</returns>
        public static string ClassifyAppointmentPrice(double price)
        {
            // Validation: if price < 0 return "Invalid"
            if (price < 0)
            {
                return "Invalid";
            }
            // Low Cost: price <= 20
            else if (price <= 20)
            {
                return "Low Cost";
            }
            // Standard: price <= 40
            else if (price <= 40)
            {
                return "Standard";
            }
            // Premium: price > 40
            else
            {
                return "Premium";
            }
        }

        // Note: To test this method, run the main program and book appointments.
        // The classification will be displayed in the booking summary.
    }
}