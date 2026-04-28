# Regent Health Booking System - Technical Report

## 1. Program Overview

The **Regent Health Booking System** is a C# console application that manages medical appointments for a healthcare clinic. It provides a menu-driven interface for registering patients, booking appointments, viewing summaries, and tracking system activity.

### Key Features:
- **User Authentication** - Login system with username/password validation
- **Patient Registration** - Register patients with name validation
- **Appointment Booking** - Book medical appointments with date/time validation
- **Booking Summary** - View all booked appointments with details
- **Price Statistics** - View highest and lowest cost appointments
- **Activity Log** - Track last 3 system actions
- **Booking Management** - Clear/delete bookings

---

## 2. Code Analysis by File

### 2.1 Program.cs (Main Entry Point)

#### Login Method (Lines 11-53)
```csharp
static bool Login()
```
- **Purpose**: Authenticates the user with hardcoded credentials
- **Credentials**: Username: `d`, Password: `r`
- **Process**:
  1. Prompts for username and password
  2. Validates credentials using conditional `if` statement
  3. Displays ASCII art doctor on success
  4. Returns `true` if authenticated, `false` otherwise

#### Main Method (Lines 55-177)
- **Authentication Loop**: Uses `while (!Login())` to keep user in login loop until valid credentials
- **Main Menu Loop**: Uses `while (running)` to display menu options
- **Menu Options**:
  | Option | Action | Method Called |
  |--------|--------|---------------|
  | 1 | Register Patient & Book | `CreatePatient()` + `BookAppointment()` |
  | 2 | View Booking Summary | `ViewSummary()` |
  | 3 | View Price Statistics | `ShowHighestLowest()` |
  | 4 | View Activity Log | `ShowActivityLog()` |
  | 5 | Clear Booking | `ClearBooking()` |
  | 6 | Logout | Sets `running = false` |

#### Input Validation (Lines 92-127)
- Uses **Regular Expression** to validate patient name
- Pattern: `@"^[a-zA-Z\s]+$"` - allows only letters and spaces
- Loops until valid input is provided

---

### 2.2 BookingSystem.cs (Core Business Logic)

#### Data Fields (Lines 18-42)
```csharp
private Appointment? currentAppointment;      // Current appointment being processed
private List<Patient> allPatients = new List<Patient>();    // Patient database
private Patient? currentPatient;              // Active patient in session
private List<Appointment> allAppointments = new List<Appointment>();  // All bookings
private string[] activityLog = new string[3]; // Last 3 actions (fixed-size)
private int activityCount = 0;                // Track activities
```

#### CreatePatient Method (Lines 44-73)
- **Purpose**: Register a new patient in the system
- **Process**:
  1. **Data Normalization**: Removes spaces and converts to lowercase
  2. **Security Check**: Validates that name contains only letters using `char.IsLetter()`
  3. **Object Instantiation**: Creates new `Patient` object
  4. **Persistence**: Adds to `allPatients` list and sets as `currentPatient`
  5. **Logging**: Records action in activity log

#### BookAppointment Method (Lines 76-175)
- **Purpose**: Create a new medical appointment
- **Process**:
  1. **Service Menu**: Displays 4 options with prices
     - General Consultation: £35
     - Nurse Check-up: £20
     - Blood Test: £15
     - Specialist Consultation: £60
  2. **Pricing Logic**: Switch statement assigns type and price
  3. **Date Validation**: Ensures date is in future using `DateTime.TryParse`
  4. **Time Validation**: Uses **Regex** to validate time format and business hours (09:00 - 21:00)
     - Pattern: `@"^((09|[1][0-9]|20):[0-5][0-9]|21:00)$"`
  5. **Instantiation**: Creates `Appointment` object with patient data
  6. **Storage**: Adds to `allAppointments` list

#### ViewSummary Method (Lines 186-220)
- **Purpose**: Display all booked appointments
- **Process**:
  1. Validates list is not empty
  2. Iterates through `allAppointments` using `foreach`
  3. Displays: Patient Name, Email, Service, Date, Time, Price, Category

#### ShowHighestLowest Method (Lines 227-260)
- **Purpose**: Display price statistics
- **Process**:
  1. Validates list is not empty
  2. Creates sorted copy using `List.Sort()` with lambda expression
  3. Displays all appointments sorted by price (descending)
  4. Shows highest and lowest cost

#### ShowActivityLog Method (Lines 267-283)
- **Purpose**: Display last 3 system actions
- **Process**:
  1. Iterates through `activityLog` array
  2. Displays non-empty entries

#### ClearBooking Method (Lines 290+)
- **Purpose**: Delete a specific booking from the list

#### AddActivity Method (Lines 177-193)
- **Purpose**: Manage activity log (fixed-size array)
- **Process**:
  - First 3 activities: Add sequentially
  - After 3: Shift elements left and add new one at end (FIFO)

---

### 2.3 Patient.cs (Data Model)

#### Class Structure
```csharp
class Patient
{
    private string fullName;    // Private field
    private string email;       // Private field
    
    public string FullName { get { return fullName; } }  // Read-only property
    public string Email { get { return email; } }        // Read-only property
}
```

#### Constructor
- **Purpose**: Initialize new patient and generate company email
- **Email Generation**:
  1. Removes spaces: `name.Replace(" ", "")`
  2. Converts to lowercase: `.ToLower()`
  3. Appends domain: `"@regenthealth.com"`
- **Example**: "John Smith" → "johnsmith@regenthealth.com"

---

### 2.4 Appointment.cs (Data Model)

#### Class Structure
```csharp
public class Appointment
{
    // Private fields (backing fields)
    private string appointmentType;
    private DateTime appointmentDate;
    private string appointmentTime;
    private double price;
    private string classification;
    private string patientName;
    private string patientEmail;
    
    // Read-only properties
    public string AppointmentType { get { return appointmentType; } }
    public DateTime AppointmentDate { get { return appointmentDate; } }
    // ... etc
}
```

#### Constructor
- **Purpose**: Create appointment and classify price
- **Classification**: Calls `ClassifyAppointmentPrice(price)` from utility class

---

### 2.5 Classify_appointment.cs (Utility Class)

#### Static Method
```csharp
public static string ClassifyAppointmentPrice(double price)
```

#### Classification Logic
| Price Range | Classification |
|-------------|----------------|
| price < 0 | "Invalid" |
| price ≤ 20 | "Low Cost" |
| price ≤ 40 | "Standard" |
| price > 40 | "Premium" |

---

## 3. Programming Concepts Used

### OOP Principles
| Concept | Implementation |
|---------|----------------|
| **Encapsulation** | Private fields with public read-only properties |
| **Composition** | `BookingSystem` contains `Patient` and `Appointment` objects |
| **Static Class** | `ClassifyAppointment` - utility methods without instantiation |

### Data Structures
| Structure | Usage |
|-----------|-------|
| `List<T>` | Dynamic collections (`allPatients`, `allAppointments`) |
| `string[]` | Fixed-size array (`activityLog` - 3 elements) |
| `Nullable<T>` | `Appointment?` and `Patient?` can be null |

### Validation Techniques
| Technique | Usage |
|-----------|-------|
| **Regex** | Time format validation, name validation |
| **TryParse** | Date input validation |
| **Conditional Checks** | Null checks, range validation |

### Control Flow
| Pattern | Usage |
|---------|-------|
| `while` loops | Login retry, input validation |
| `switch` statement | Menu options, pricing logic |
| `foreach` iteration | Display collections |

---

## 4. Program Flow Diagram

```
┌─────────────────────────────────────┐
│         PROGRAM START               │
└─────────────────┬───────────────────┘
                  │
                  ▼
┌─────────────────────────────────────┐
│         LOGIN SCREEN                │
│   Username: d / Password: r         │
└─────────────────┬───────────────────┘
                  │ (Invalid)
                  ▼ (Valid)
┌─────────────────────────────────────┐
│         MAIN MENU                   │
│  1. Register & Book                 │
│  2. View Summary                    │
│  3. Price Statistics                │
│  4. Activity Log                    │
│  5. Clear Booking                   │
│  6. Logout                          │
└─────────────────┬───────────────────┘
                  │
        ┌─────────┼─────────┐
        ▼         ▼         ▼
    [Process based on selection]
        │         │         │
        └─────────┴─────────┘
                  │
                  ▼
         [Loop back to menu]
                  │
                  ▼ (Option 6)
┌─────────────────────────────────────┐
│         PROGRAM END                 │
└─────────────────────────────────────┘
```

---

## 4.1 Login Flow Diagram

```
┌─────────────────────────────────────────────────────────────┐
│                    LOGIN METHOD FLOW                        │
└─────────────────────────────────────────────────────────────┘

                         ┌─────────────────┐
                         │   START Login   │
                         └────────┬────────┘
                                  │
                                  ▼
                    ┌─────────────────────────┐
                    │  Prompt: "Username: "   │
                    └────────────┬────────────┘
                                 │
                                 ▼
                    ┌─────────────────────────┐
                    │   Read Username (u)     │
                    └────────────┬────────────┘
                                 │
                                 ▼
                    ┌─────────────────────────┐
                    │  Prompt: "Password: "   │
                    └────────────┬────────────┘
                                 │
                                 ▼
                    ┌─────────────────────────┐
                    │   Read Password (p)     │
                    └────────────┬────────────┘
                                 │
                                 ▼
                         ┌───────┴───────┐
                         │  u == "d"     │
                         │  p == "r"     │
                         └───────┬───────┘
                    ┌─────────────┴─────────────┐
                    │                           │
                   YES                          NO
                    │                           │
                    ▼                           ▼
         ┌──────────────────┐    ┌──────────────────────┐
         │ Set GREEN color  │    │ Set RED color        │
         │ Display ASCII    │    │ Print "Incorrect     │
         │   Doctor Art     │    │   credentials"       │
         │ Print Welcome    │    │ Reset color          │
         │   "DR. D"        │    └──────────┬───────────┘
         │ Reset color      │               │
         └────────┬─────────┘               │
                  │                         │
                  ▼                         │
         ┌──────────────────┐               │
         │   RETURN TRUE    │               │
         └────────┬─────────┘               │
                  │                         ▼
                  │                ┌──────────────────┐
                  │                │   RETURN FALSE   │
                  │                └────────┬─────────┘
                  │                         │
                  └─────────┬───────────────┘
                            │
                            ▼
                       [END LOGIN]
```

---

## 4.2 Patient Registration Flow Diagram

```
┌─────────────────────────────────────────────────────────────┐
│                CREATEPATIENT METHOD FLOW                    │
└─────────────────────────────────────────────────────────────┘

                         ┌─────────────────┐
                         │ START           │
                         │ CreatePatient   │
                         └────────┬────────┘
                                  │
                                  ▼
                    ┌─────────────────────────┐
                    │ Step 1: Normalization   │
                    │ cleanName = name        │
                    │   .Replace(" ", "")     │
                    │   .ToLower()            │
                    └────────────┬────────────┘
                                 │
                                 ▼
                    ┌─────────────────────────┐
                    │ Step 2: Security Check  │
                    │ Is NullOrWhiteSpace?    │
                    └────────────┬────────────┘
                                 │
                                 ▼
                         ┌───────┴───────┐
                         │  Is Valid?    │
                         └───────┬───────┘
                    ┌─────────────┴─────────────┐
                    │                           │
                   YES                          NO
                    │                           │
                    ▼                           ▼
         ┌──────────────────┐    ┌──────────────────────┐
         │ Continue         │    │ Print ERROR message  │
         │                  │    │ "Name must contain   │
         │                  │    │  letters only"       │
         │                  │    └──────────┬───────────┘
         │                  │               │
         │                  │               ▼
         │                  │    ┌──────────────────┐
         │                  │    │   RETURN         │
         │                  │    │   (Exit Method)  │
         │                  │    └──────────────────┘
         └────────┬─────────┘
                  │
                  ▼
         ┌──────────────────┐
         │ Step 3: Create   │
         │ new Patient      │
         │ object           │
         └────────┬─────────┘
                  │
                  ▼
         ┌──────────────────┐
         │ Step 4: Storage  │
         │ allPatients.Add  │
         │ currentPatient = │
         │   newPatient     │
         └────────┬─────────┘
                  │
                  ▼
         ┌──────────────────┐
         │ Step 5: Logging  │
         │ AddActivity      │
         │ ("Registered     │
         │  patient: ...")  │
         └────────┬─────────┘
                  │
                  ▼
         ┌──────────────────┐
         │   END METHOD     │
         └──────────────────┘
```

---

## 4.3 Appointment Booking Flow Diagram

```
┌─────────────────────────────────────────────────────────────┐
│               BOOKAPPOINTMENT METHOD FLOW                   │
└─────────────────────────────────────────────────────────────┘

                         ┌─────────────────┐
                         │ START           │
                         │ BookAppointment │
                         └────────┬────────┘
                                  │
                                  ▼
                    ┌─────────────────────────┐
                    │ Display Service Menu    │
                    │ 1. General (£35)        │
                    │ 2. Nurse (£20)          │
                    │ 3. Blood Test (£15)     │
                    │ 4. Specialist (£60)     │
                    └────────────┬────────────┘
                                 │
                                 ▼
                    ┌─────────────────────────┐
                    │ Get User Choice         │
                    └────────────┬────────────┘
                                 │
                                 ▼
                    ┌─────────────────────────┐
                    │ Validate: Is Number     │
                    │     1-4?                │
                    └────────────┬────────────┘
                                 │
                                 ▼
                         ┌───────┴───────┐
                         │  Is Valid?    │
                         └───────┬───────┘
                    ┌─────────────┴─────────────┐
                    │                           │
                   YES                          NO
                    │                           │
                    ▼                           ▼
         ┌──────────────────┐    ┌──────────────────────┐
         │ Switch: Set      │    │ Print "Invalid       │
         │ type & price     │    │   selection"         │
         └────────┬─────────┘    └──────────┬───────────┘
                  │                         │
                  ▼                         ▼
         ┌──────────────────┐    ┌──────────────────┐
         │ DATE INPUT LOOP  │    │   RETURN         │
         │                  │    │   (Exit Method)  │
         │ Prompt: Enter    │    └──────────────────┘
         │   date (yyyy-mm) │
         └────────┬─────────┘
                  │
                  ▼
         ┌──────────────────┐
         │ TryParse date    │
         │ Check: date >=   │
         │   DateTime.Today │
         └────────┬─────────┘
                  │
                  ▼
                         ┌───────┐
                         │ Valid │
                         │  ?    │
                         └───┬───┘
                    ┌─────────┴─────────┐
                    │                   │
                   YES                  NO
                    │                   │
                    ▼                   ▼
         ┌──────────────────┐    ┌──────────────────┐
         │ Break loop       │    │ Error message    │
         │ Continue         │    │ Loop again       │
         └────────┬─────────┘    └────────┬─────────┘
                  │                         │
                  └─────────┬───────────────┘
                            │
                            ▼
                    ┌─────────────────────────┐
                    │ TIME INPUT LOOP         │
                    │                         │
                    │ Prompt: Enter time      │
                    │   (HH:mm) 09:00-21:00   │
                    └────────────┬────────────┘
                                 │
                                 ▼
                    ┌─────────────────────────┐
                    │ Regex validation        │
                    │ Pattern: ^((09|[1][0-9] │
                    │ |20):[0-5][0-9]|21:00)$ │
                    └────────────┬────────────┘
                                 │
                                 ▼
                         ┌───────┐
                         │ Match │
                         │  ?    │
                         └───┬───┘
                    ┌─────────┴─────────┐
                    │                   │
                   YES                  NO
                    │                   │
                    ▼                   ▼
         ┌──────────────────┐    ┌──────────────────┐
         │ Break loop       │    │ Error message    │
         │ Continue         │    │ Loop again       │
         └────────┬─────────┘    └────────┬─────────┘
                  │                         │
                  └─────────┬───────────────┘
                            │
                            ▼
                    ┌─────────────────────────┐
                    │ Get Patient Data        │
                    │ patientName =           │
                    │   currentPatient        │
                    │     ?.FullName          │
                    │ patientEmail =          │
                    │   currentPatient        │
                    │     ?.Email             │
                    └────────────┬────────────┘
                                 │
                                 ▼
                    ┌─────────────────────────┐
                    │ Create Appointment      │
                    │ new Appointment(type,   │
                    │   date, time, price,    │
                    │   name, email)          │
                    └────────────┬────────────┘
                                 │
                                 ▼
                    ┌─────────────────────────┐
                    │ Classification =        │
                    │ ClassifyAppointment     │
                    │   .Price(price)         │
                    └────────────┬────────────┘
                                 │
                                 ▼
                    ┌─────────────────────────┐
                    │ Add to allAppointments  │
                    │ list                    │
                    └────────────┬────────────┘
                                 │
                                 ▼
                    ┌─────────────────────────┐
                    │ AddActivity("Booked "   │
                    │   + type)               │
                    └────────────┬────────────┘
                                 │
                                 ▼
                    ┌─────────────────────────┐
                    │   END METHOD            │
                    └─────────────────────────┘
```

---

## 4.4 View Summary Flow Diagram

```
┌─────────────────────────────────────────────────────────────┐
│                 VIEWSUMMARY METHOD FLOW                     │
└─────────────────────────────────────────────────────────────┘

                         ┌─────────────────┐
                         │ START           │
                         │ ViewSummary     │
                         └────────┬────────┘
                                  │
                                  ▼
                    ┌─────────────────────────┐
                    │ Check: allAppointments  │
                    │          .Count == 0    │
                    └────────────┬────────────┘
                                 │
                                 ▼
                         ┌───────┴───────┐
                         │  Is Empty?    │
                         └───────┬───────┘
                    ┌─────────────┴─────────────┐
                    │                           │
                   YES                          NO
                    │                           │
                    ▼                           ▼
         ┌──────────────────┐    ┌──────────────────────┐
         │ Print "No booking│    │ Print "==========    │
         │  records found"  │    │  BOOKING SUMMARY     │
         │                  │    │  =========="         │
         │ AddActivity      │    └──────────┬───────────┘
         │ ("Empty")        │               │
         └────────┬─────────┘               │
                  │                         ▼
                  │            ┌─────────────────────────┐
                  │            │ Initialize counter = 1 │
                  │            └────────────┬────────────┘
                  │                         │
                  │                         ▼
                  │            ┌─────────────────────────┐
                  │            │ FOR EACH appointment    │
                  │            │   in allAppointments    │
                  │            └────────────┬────────────┘
                  │                         │
                  │                         ▼
                  │            ┌─────────────────────────┐
                  │            │ Print "--- Booking #N ---"
                  │            │ Print Patient Name      │
                  │            │ Print Email             │
                  │            │ Print Service Type      │
                  │            │ Print Date              │
                  │            │ Print Time              │
                  │            │ Print Price             │
                  │            │ Print Classification    │
                  │            └────────────┬────────────┘
                  │                         │
                  │                         ▼
                  │            ┌─────────────────────────┐
                  │            │ counter++               │
                  │            └────────────┬────────────┘
                  │                         │
                  │                         ▼
                  │                         ┌───────┐
                  │                         │ More? │──YES──► Loop
                  │                         └───┬───┘
                  │                             │ NO
                  │                             ▼
                  │            ┌─────────────────────────┐
                  │            │ Print "=============="  │
                  │            └────────────┬────────────┘
                  │                             │
                  │                             ▼
                  │            ┌─────────────────────────┐
                  │            │ AddActivity("Viewed     │
                  │            │   summary")            │
                  │            └────────────┬────────────┘
                  │                             │
                  ▼                             ▼
         ┌──────────────────┐    ┌─────────────────────────┐
         │   RETURN         │    │      END METHOD         │
         │   (Exit)         │    └─────────────────────────┘
         └──────────────────┘
```

---

## 4.5 Price Statistics Flow Diagram

```
┌─────────────────────────────────────────────────────────────┐
│             SHOWHIGHESTLOWEST METHOD FLOW                   │
└─────────────────────────────────────────────────────────────┘

                         ┌─────────────────┐
                         │ START           │
                         │ ShowHighest     │
                         │     Lowest      │
                         └────────┬────────┘
                                  │
                                  ▼
                    ┌─────────────────────────┐
                    │ Check: allAppointments  │
                    │          .Count == 0    │
                    └────────────┬────────────┘
                                 │
                                 ▼
                         ┌───────┴───────┐
                         │  Is Empty?    │
                         └───────┬───────┘
                    ┌─────────────┴─────────────┐
                    │                           │
                   YES                          NO
                    │                           │
                    ▼                           ▼
         ┌──────────────────┐    ┌──────────────────────┐
         │ Print "No data   │    │ Create sorted copy   │
         │  available"      │    │ var sorted = new     │
         │                  │    │   List<Appointment>  │
         │ RETURN           │    │   (allAppointments)  │
         └──────────────────┘    └──────────┬───────────┘
                                             │
                                             ▼
                                  ┌─────────────────────────┐
                                  │ Sort descending        │
                                  │ sorted.Sort((a,b) =>   │
                                  │   b.Price.CompareTo    │
                                  │   (a.Price))           │
                                  └────────────┬────────────┘
                                               │
                                               ▼
                                  ┌─────────────────────────┐
                                  │ Print "=========="      │
                                  │ Print "PRICE STATISTICS"│
                                  │ Print "All Appointments"│
                                  │ Print "(Highest to      │
                                  │   Lowest)"             │
                                  └────────────┬────────────┘
                                               │
                                               ▼
                                  ┌─────────────────────────┐
                                  │ FOR EACH a in sorted   │
                                  └────────────┬────────────┘
                                               │
                                               ▼
                                  ┌─────────────────────────┐
                                  │ Print £{a.Price}        │
                                  │   - {a.AppointmentType}│
                                  │   Name: ({a.PatientName})│
                                  └────────────┬────────────┘
                                               │
                                               ▼
                                               ┌───────┐
                                               │ More? │──YES──► Loop
                                               └───┬───┘
                                                   │ NO
                                                   ▼
                                  ┌─────────────────────────┐
                                  │ Print "Highest Cost:   │
                                  │   £{sorted[0].Price}"  │
                                  │ Print "Lowest Cost:    │
                                  │   £{sorted[last].Price}"│
                                  └────────────┬────────────┘
                                               │
                                               ▼
                                  ┌─────────────────────────┐
                                  │      END METHOD         │
                                  └─────────────────────────┘
```

---

## 4.6 Activity Log Flow Diagram

```
┌─────────────────────────────────────────────────────────────┐
│               SHOWACTIVITYLOG METHOD FLOW                   │
└─────────────────────────────────────────────────────────────┘

                         ┌─────────────────┐
                         │ START           │
                         │ ShowActivityLog │
                         └────────┬────────┘
                                  │
                                  ▼
                    ┌─────────────────────────┐
                    │ Print "--- Activity Log │
                    │   (Last 3 Actions) ---" │
                    └────────────┬────────────┘
                                 │
                                 ▼
                    ┌─────────────────────────┐
                    │ Check: activityCount    │
                    │          == 0           │
                    └────────────┬────────────┘
                                 │
                                 ▼
                         ┌───────┴───────┐
                         │  Is Zero?     │
                         └───────┬───────┘
                    ┌─────────────┴─────────────┐
                    │                           │
                   YES                          NO
                    │                           │
                    ▼                           ▼
         ┌──────────────────┐    ┌──────────────────────┐
         │ Print "No        │    │ FOR i = 0 to         │
         │  activities      │    │   activityCount-1    │
         │  recorded yet"   │    └──────────┬───────────┘
         │                  │               │
         │ RETURN           │               ▼
         └──────────────────┘    ┌─────────────────────────┐
                                 │ Check: activityLog[i]   │
                                 │   != null && != ""      │
                                 └────────────┬────────────┘
                                              │
                                              ▼
                                      ┌───────┴───────┐
                                      │  Is Valid?    │
                                      └───────┬───────┘
                                 ┌─────────────┴─────────────┐
                                 │                           │
                                YES                          NO
                                 │                           │
                                 ▼                           ▼
                      ┌──────────────────┐    ┌──────────────────┐
                      │ Print "- {activity│    │ Skip (continue)  │
                      │   Log[i]}"       │    │                  │
                      └────────┬─────────┘    └────────┬─────────┘
                               │                       │
                               └─────────┬─────────────┘
                                         │
                                         ▼
                               ┌─────────────────────────┐
                               │      END METHOD         │
                               └─────────────────────────┘
```

---

## 4.7 Add Activity (Logging) Flow Diagram

```
┌─────────────────────────────────────────────────────────────┐
│                 ADDACTIVITY METHOD FLOW                     │
└─────────────────────────────────────────────────────────────┘

                         ┌─────────────────┐
                         │ START           │
                         │ AddActivity     │
                         │   (action)      │
                         └────────┬────────┘
                                  │
                                  ▼
                    ┌─────────────────────────┐
                    │ Check: activityCount    │
                    │          < 3            │
                    └────────────┬────────────┘
                                 │
                                 ▼
                         ┌───────┴───────┐
                         │  Less than 3? │
                         └───────┬───────┘
                    ┌─────────────┴─────────────┐
                    │                           │
                   YES                          NO
                    │                           │
                    ▼                           ▼
         ┌──────────────────┐    ┌──────────────────────┐
         │ activityLog[     │    │ FOR i = 0 to 1       │
         │   activityCount] │    │   activityLog[i] =   │
         │   = action       │    │   activityLog[i+1]   │
         │                  │    │ (shift left)         │
         │ activityCount++  │    └──────────┬───────────┘
         └────────┬─────────┘               │
                  │                         ▼
                  │            ┌─────────────────────────┐
                  │            │ activityLog[2] = action│
                  │            │ (add at end)           │
                  │            └────────────┬────────────┘
                  │                         │
                  ▼                         ▼
         ┌──────────────────┐    ┌─────────────────────────┐
         │      END         │    │       END               │
         └──────────────────┘    └─────────────────────────┘


┌─────────────────────────────────────────────────────────────┐
│              ACTIVITY LOG ARRAY VISUALIZATION               │
└─────────────────────────────────────────────────────────────┘

Initial State (Empty):
┌─────────┬─────────┬─────────┐
│   [0]   │   [1]   │   [2]   │
│  null   │  null   │  null   │
└─────────┴─────────┴─────────┘
activityCount = 0

After 1st Activity ("Registered patient john"):
┌─────────┬─────────┬─────────┐
│   [0]   │   [1]   │   [2]   │
│   "A"   │  null   │  null   │
└─────────┴─────────┴─────────┘
activityCount = 1

After 2nd Activity ("Booked General Consultation"):
┌─────────┬─────────┬─────────┐
│   [0]   │   [1]   │   [2]   │
│   "A"   │   "B"   │  null   │
└─────────┴─────────┴─────────┘
activityCount = 2

After 3rd Activity ("Viewed summary"):
┌─────────┬─────────┬─────────┐
│   [0]   │   [1]   │   [2]   │
│   "A"   │   "B"   │   "C"   │
└─────────┴─────────┴─────────┘
activityCount = 3 (FULL)

After 4th Activity ("Booked Blood Test"):
  Step 1: Shift left
┌─────────┬─────────┬─────────┐
│   [0]   │   [1]   │   [2]   │
│   "B"   │   "C"   │   "C"   │  ← "C" duplicated temporarily
└─────────┴─────────┴─────────┘

  Step 2: Add new at end
┌─────────┬─────────┬─────────┐
│   [0]   │   [1]   │   [2]   │
│   "B"   │   "C"   │   "D"   │  ← FIFO: First In, First Out
└─────────┴─────────┴─────────┘
activityCount = 3 (still full)
```

---

## 4.8 Classification Flow Diagram

```
┌─────────────────────────────────────────────────────────────┐
│         CLASSIFYAPPOINTMENTPRICE METHOD FLOW                │
└─────────────────────────────────────────────────────────────┘

                         ┌─────────────────┐
                         │ START           │
                         │ ClassifyAppointment│
                         │   Price(price)  │
                         └────────┬────────┘
                                  │
                                  ▼
                    ┌─────────────────────────┐
                    │ Check: price < 0        │
                    └────────────┬────────────┘
                                 │
                                 ▼
                         ┌───────┴───────┐
                         │ price < 0?    │
                         └───────┬───────┘
                    ┌─────────────┴─────────────┐
                    │                           │
                   YES                          NO
                    │                           │
                    ▼                           ▼
         ┌──────────────────┐    ┌──────────────────────┐
         │ RETURN "Invalid" │    │ Check: price <= 20   │
         └──────────────────┘    └──────────┬───────────┘
                                             │
                                             ▼
                                     ┌───────┴───────┐
                                     │ price <= 20?  │
                                     └───────┬───────┘
                                ┌─────────────┴─────────────┐
                                │                           │
                               YES                          NO
                                │                           │
                                ▼                           ▼
                     ┌──────────────────┐    ┌──────────────────────┐
                     │ RETURN "Low Cost"│    │ Check: price <= 40   │
                     └──────────────────┘    └──────────┬───────────┘
                                                        │
                                                        ▼
                                                ┌───────┴───────┐
                                                │ price <= 40?  │
                                                └───────┬───────┘
                                           ┌─────────────┴─────────────┐
                                           │                           │
                                          YES                          NO
                                           │                           │
                                           ▼                           ▼
                                ┌──────────────────┐    ┌──────────────────────┐
                                │ RETURN "Standard"│    │ RETURN "Premium"    │
                                └──────────────────┘    └──────────────────────┘


┌─────────────────────────────────────────────────────────────┐
│              CLASSIFICATION EXAMPLES TABLE                  │
└─────────────────────────────────────────────────────────────┘

┌──────────────────┬─────────────────┬────────────────────────┐
│     Price        │   Classification│   Service Example      │
├──────────────────┼─────────────────┼────────────────────────┤
│      -10         │    Invalid      │   (error case)         │
│       0          │   Low Cost      │   -                    │
│      15          │   Low Cost      │   Blood Test (£15)     │
│      20          │   Low Cost      │   Nurse Check-up (£20) │
│      25          │   Standard      │   -                    │
│      35          │   Standard      │   General Consult (£35)│
│      40          │   Standard      │   -                    │
│      45          │   Premium       │   -                    │
│      60          │   Premium       │   Specialist (£60)     │
│     100          │   Premium       │   -                    │
└──────────────────┴─────────────────┴────────────────────────┘
```

---

## 4.9 Patient Object Creation Flow Diagram

```
┌─────────────────────────────────────────────────────────────┐
│                  PATIENT CONSTRUCTOR FLOW                   │
└─────────────────────────────────────────────────────────────┘

                         ┌─────────────────┐
                         │ START           │
                         │ new Patient     │
                         │   ("johnsmith") │
                         └────────┬────────┘
                                  │
                                  ▼
                    ┌─────────────────────────┐
                    │ Store name in fullName  │
                    │ fullName = "johnsmith"  │
                    └────────────┬────────────┘
                                 │
                                 ▼
                    ┌─────────────────────────┐
                    │ Generate Email:         │
                    │ email = name            │
                    │   .Replace(" ", "")     │
                    │   .ToLower()            │
                    │   + "@regenthealth.com" │
                    └────────────┬────────────┘
                                 │
                                 ▼
                    ┌─────────────────────────┐
                    │ Example:                │
                    │ "John Smith" →          │
                    │ "johnsmith@regenthealth │
                    │  .com"                  │
                    └────────────┬────────────┘
                                 │
                                 ▼
                    ┌─────────────────────────┐
                    │      END CONSTRUCTOR    │
                    └─────────────────────────┘


┌─────────────────────────────────────────────────────────────┐
│              EMAIL GENERATION EXAMPLES                      │
└─────────────────────────────────────────────────────────────┘

Input Name          Process                    Generated Email
────────────────────────────────────────────────────────────────
"John Smith"    →  Replace(" ", "") → "JohnSmith"
                 →  ToLower()       → "johnsmith"
                 →  + "@regent..."  → "johnsmith@regenthealth.com"

"MARIA Garcia"  →  Replace(" ", "") → "Mariagarcia"
                 →  ToLower()       → "mariagarcia"
                 →  + "@regent..."  → "mariagarcia@regenthealth.com"

"Alice"         →  Replace(" ", "") → "Alice"
                 →  ToLower()       → "alice"
                 →  + "@regent..."  → "alice@regenthealth.com"

"Dr John Doe"   →  Replace(" ", "") → "DrJohnDoe"
                 →  ToLower()       → "drjohndoe"
                 →  + "@regent..."  → "drjohndoe@regenthealth.com"
```

---

## 4.10 Main Menu Loop Flow Diagram

```
┌─────────────────────────────────────────────────────────────┐
│                    MAIN MENU LOOP FLOW                      │
└─────────────────────────────────────────────────────────────┘

                         ┌─────────────────┐
                         │ START MAIN      │
                         └────────┬────────┘
                                  │
                                  ▼
                    ┌─────────────────────────┐
                    │ Create BookingSystem    │
                    │   instance              │
                    └────────────┬────────────┘
                                 │
                                 ▼
                    ┌─────────────────────────┐
                    │ while (!Login()) { }    │
                    │                         │
                    │ Loop until valid        │
                    │ credentials             │
                    └────────────┬────────────┘
                                 │
                                 ▼
                    ┌─────────────────────────┐
                    │ bool running = true     │
                    └────────────┬────────────┘
                                 │
                                 ▼
                         ┌───────┴───────┐
                         │  running?     │
                         └───────┬───────┘
                    ┌─────────────┴─────────────┐
                    │                           │
                   YES                          NO
                    │                           │
                    ▼                           ▼
         ┌──────────────────┐    ┌──────────────────────┐
         │ Display Menu     │    │      END PROGRAM     │
         │                  │    └──────────────────────┘
         │ 1. Register      │
         │ 2. Summary       │
         │ 3. Statistics    │
         │ 4. Activity      │
         │ 5. Clear         │
         │ 6. Logout        │
         └────────┬─────────┘
                  │
                  ▼
         ┌──────────────────┐
         │ Get user input   │
         └────────┬─────────┘
                  │
                  ▼
         ┌──────────────────┐
         │ Switch(input)    │
         └────────┬─────────┘
                  │
        ┌─────────┼─────────┼─────────┐
        ▼         ▼         ▼         ▼
      case 1   case 2   case 3   case 4
        │         │         │         │
        ▼         ▼         ▼         ▼
    CreatePatient  View    Show     Show
    +BookAppointment Summary Highest Activity
        │         │         │      Log
        └─────────┼─────────┼────────┘
                  │         │
                  ▼         ▼
              case 5    case 6
                │         │
                ▼         ▼
           Clear      running = false
           Booking    (exit loop)
                │         │
                └────┬────┘
                     │
                     ▼
              ┌──────┴──────┐
              │   default   │
              │ Invalid     │
              │ option      │
              └──────┬──────┘
                     │
                     ▼
              [Loop back to while(running)]
                     │
                     └────────────────────────► (YES from running?)
                                                   │
                                                   ▼
                                            [END PROGRAM]
```

---

## 5. Summary

This is a well-structured console application that demonstrates:
- ✅ **Object-Oriented Programming** with proper encapsulation
- ✅ **Data validation** using Regex and TryParse
- ✅ **Collection management** with Lists and arrays
- ✅ **Activity logging** with fixed-size buffer
- ✅ **Menu-driven interface** with switch statement
- ✅ **Input sanitization** for security

The program successfully manages a medical booking system with patient registration, appointment scheduling, and reporting capabilities.