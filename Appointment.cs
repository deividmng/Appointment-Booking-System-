public class Appointment
{
    // --- 1. PRIVATE FIELDS (Backing Fields) ---
    private string appointmentType;
    private DateTime appointmentDate;
    private string appointmentTime;
    private double price;
    private string classification;
    // Añadimos los nuevos campos privados:
    private string patientName; 
    private string patientEmail;

    // --- 2. PUBLIC PROPERTIES (Read-Only) ---
    public string AppointmentType { get { return appointmentType; } }
    public DateTime AppointmentDate { get { return appointmentDate; } }
    public string AppointmentTime { get { return appointmentTime; } }
    public double Price { get { return price; } }
    public string Classification { get { return classification; } }
    // Añadimos las nuevas propiedades siguiendo tu estilo:
    public string PatientName { get { return patientName; } }
    public string PatientEmail { get { return patientEmail; } }

    // --- 3. CONSTRUCTOR ---
    public Appointment(string type, DateTime date, string time, double price, string pName, string pEmail)
    {
        this.appointmentType = type;
        this.appointmentDate = date;
        this.appointmentTime = time;
        this.price = price;
        this.patientName = pName;   // Asignamos el nombre
        this.patientEmail = pEmail; // Asignamos el email
        this.classification = Classify_appointment(price);
    }

    private string Classify_appointment(double price)
    {
        if (price < 20) return "Low Cost";
        if (price <= 50) return "Standard";
        return "Premium";
    }
}