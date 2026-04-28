using RegentHealthBookingSystem;

/// <summary>
/// Usind this class encapsulates all appointment details and links them to a specific patient.
/// </summary>
public class Appointment
{
    // --- 1. PRIVATE FIELDS (Backing Fields) ---
// Using private fields to enforce Encapsulation, on this way it will prevent a external modification 
    // Creathig the vars
    private string appointmentType;
    private DateTime appointmentDate;
    private string appointmentTime;
    private double price;
    private string classification;
    // This save "store" the identity of patient for this apoiment perosnal data as well
    private string patientName; 
    private string patientEmail;

   // --- 2. PUBLIC PROPERTIES (Read-Only) using just get and not set, imp when return do in lowercase on this way it won't be availableto modificate outsite "read only" of the objet and will be available for the contrator---
    public string AppointmentType { get { return appointmentType; } }
    public DateTime AppointmentDate { get { return appointmentDate; } }
    public string AppointmentTime { get { return appointmentTime; } }
    public double Price { get { return price; } }
    public string Classification { get { return classification; } }
    // Añadimos las nuevas propiedades siguiendo tu estilo:
    public string PatientName { get { return patientName; } }
    public string PatientEmail { get { return patientEmail; } }

    // --- 3. CONSTRUCTOR ---
// This constrator is the inicialitation of the Appointment Class. <this>. keyword to distinguish between the class and local parramt, on this way it will be more clear and will reduce the error as it kwnos whit one to target
    public Appointment(string type, DateTime date, string time, double price, string pName, string pEmail)
    {
        this.appointmentType = type;
        this.appointmentDate = date;
        this.appointmentTime = time;
        this.price = price;
        this.patientName = pName;   // assingibg the name and email
        this.patientEmail = pEmail; //Calling the metohd ClassifyAppoinment and adding the method will evaluate the price on the parramt on this way will be avaliable to display after when the method will be call to saw the last 3 activitys 
        this.classification = ClassifyAppointment.ClassifyAppointmentPrice(price);
    }
}