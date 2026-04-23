namespace RegentHealthBookingSystem
{
    /// <summary>
    /// This class  "Patient"is for storage and formatting patient of data.
    /// </summary>
    class Patient
    {
        // These are  variables private to protect data.
        private string fullName;
        private string email;

        // --- Properties ---
        // These allow other classes to read the data without and not be avalible to modify it directly.
        public string FullName { get { return fullName; } }
        public string Email { get { return email; } }

        // --- The Constructor ---
        /// <summary>
        /// Initializes a new patient and automatically generates a company email.
        /// </summary>
        public Patient(string name)
        {
            // Store the original name provided during registration
            fullName = name;

            
            // Replace(" ", ""): Removes any spaces between names.
            // ToLower(): Converts all characters to lowercase In case the user use upercase.
            // Appending the required corporate domain = + "@regenthealth.com".
            email = name.Replace(" ", "").ToLower() + "@regenthealth.com";
        }
    }
}