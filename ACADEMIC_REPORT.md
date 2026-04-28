# Regent Health Booking System - Academic Technical Report

## Executive Summary

This report provides a comprehensive technical analysis of the Regent Health Booking System, a C# console application designed for managing medical appointments. The system implements core object-oriented programming (OOP) concepts including encapsulation, data validation, collections management, and static utility methods. This document examines each component through theoretical frameworks, pseudocode representations, flow diagrams, and implementation analysis.

---

## 1. Theoretical Concepts and C# Foundations

### 1.1 Object-Oriented Programming (OOP) Fundamentals

The Regent Health Booking System employs fundamental OOP principles as documented by Microsoft (Microsoft, 2023). **Encapsulation** is achieved through the use of private fields with public properties, preventing direct external modification of internal data. In C#, this is implemented using the property getter pattern:

```csharp
// Private field (backing field)
private string fullName;

// Public property with read-only access
public string FullName { get { return fullName; } }
```

This approach follows the principle of **data hiding**, ensuring that internal state can only be accessed through controlled interfaces (Gamma et al., 1995).

### 1.2 Nullable Types and Null Reference Handling

C# nullable value types (`?`) allow value types to represent undefined values (Microsoft, 2023). In the BookingSystem class:

```csharp
private Appointment? currentAppointment;
private Patient? currentPatient;
```

These nullable references are critical for session management, as no patient or appointment exists at program initialization. The `?` operator enables the compiler to enforce null-checking, reducing NullReferenceException runtime errors.

### 1.3 Collections: List<T> vs Arrays

The system utilizes both `List<T>` (dynamic collections) and fixed-size arrays:

- **List<Patient>** and **List<Appointment>**: Dynamic collections that grow automatically
- **string[] activityLog**: Fixed-size array of 3 elements for activity tracking

According to C# documentation, `List<T>` provides O(1) add operations and O(n) search operations, making it ideal for the appointment storage requirements (Microsoft, 2023).

### 1.4 Regular Expressions (Regex) for Input Validation

The system employs Regex for strict input validation:

```csharp
bool isOnlyLetters = System.Text.RegularExpressions.Regex.IsMatch(input, @"^[a-zA-Z\s]+$");
```

This pattern ensures only alphabetic characters and spaces are accepted, preventing SQL injection-style attacks and maintaining data integrity (Friedl, 2006).

### 1.5 Static Classes and Methods

The `ClassifyAppointment` class uses the `static` modifier, meaning it can be invoked without instantiating an object:

```csharp
public static class ClassifyAppointment
{
    public static string ClassifyAppointmentPrice(double price) { ... }
}
```

Static classes are appropriate for utility functions that don't require instance state (Microsoft, 2023).

---

## 2. Program Structure and Pseudocode

### 2.1 Main Program Flow (Program.cs)

**Pseudocode:**

```
BEGIN PROGRAM
    DISPLAY "=== REGENT HEALTH BOOKING SYSTEM ==="
    
    WHILE user not authenticated:
        INPUT username
        INPUT password
        IF username == "d" AND password == "r":
            SET authenticated = true
        ELSE:
            DISPLAY "Invalid credentials"
    
    CREATE BookingSystem instance
    
    WHILE running == true:
        DISPLAY Main Menu:
            1. Register Patient & Book Appointment
            2. View Booking Summary
            3. Show Price Statistics
            4. Show Activity Log
            5. Clear Booking
            6. Exit
        
        INPUT menu choice
        
        SWITCH choice:
            CASE "1":
                INPUT patient name
                VALIDATE name (letters only, regex)
                CALL system.CreatePatient(name)
                CALL system.BookAppointment()
            CASE "2":
                CALL system.ViewSummary()
            CASE "3":
                CALL system.ShowHighestLowest()
            CASE "4":
                CALL system.ShowActivityLog()
            CASE "5":
                CALL system.ClearBooking()
            CASE "6":
                SET running = false
            DEFAULT:
                DISPLAY "Invalid option"
END PROGRAM
```

### 2.2 BookingSystem Core Methods

**CreatePatient() Pseudocode:**

```
BEGIN CreatePatient(name)
    SET cleanName = name.Replace(" ", "").ToLower()
    
    IF string.IsNullOrWhiteSpace(cleanName) OR NOT cleanName.All(char.IsLetter):
        DISPLAY "[ERROR] Name must contain letters only"
        RETURN
    
    CREATE newPatient = new Patient(cleanName)
    allPatients.Add(newPatient)
    currentPatient = newPatient
    AddActivity("Registered and selected patient: " + cleanName)
END CreatePatient
```

**BookAppointment() Pseudocode:**

```
BEGIN BookAppointment()
    DISPLAY service menu (4 options with prices)
    INPUT choice
    
    SWITCH choice:
        CASE 1: type = "General Consultation", price = 35
        CASE 2: type = "Nurse Check-up", price = 20
        CASE 3: type = "Blood Test", price = 15
        CASE 4: type = "Specialist Consultation", price = 60
        DEFAULT: DISPLAY error, RETURN
    
    WHILE true:
        INPUT date (yyyy-mm-dd format)
        IF DateTime.TryParse(dateInput, out date) AND date >= DateTime.Today:
            BREAK
        ELSE:
            DISPLAY "Invalid date"
    
    WHILE true:
        INPUT time (HH:mm format)
        SET pattern = @"^((09|[1][0-9]|20):[0-5][0-9]|21:00)$"
        IF Regex.IsMatch(time, pattern):
            BREAK
        ELSE:
            DISPLAY "Invalid time - must be between 09:00 and 21:00"
    
    SET patientName = currentPatient.FullName
    SET patientEmail = currentPatient.Email
    CREATE currentAppointment = new Appointment(type, date, time, price, patientName, patientEmail)
    allAppointments.Add(currentAppointment)
    AddActivity("Booked " + type)
END BookAppointment
```

---

## 3. Flow Diagrams

### 3.1 Main Program Flow

```
┌─────────────────────────────────────────┐
│         PROGRAM START                   │
└─────────────────┬───────────────────────┘
                  │
                  ▼
┌─────────────────────────────────────────┐
│     Display Authentication Prompt       │
└─────────────────┬───────────────────────┘
                  │
                  ▼
         ┌────────────────┐
         │ Input Login    │
         └────┬───────────┘
              │
      ┌───────┴───────┐
      ▼               ▼
┌─────────┐     ┌─────────────┐
│ Valid?  │─NO─►│ Re-prompt   │
└────┬────┘     └─────────────┘
     │YES
     ▼
┌─────────────────────────────────────────┐
│       Display Main Menu (6 options)     │
└─────────────────┬───────────────────────┘
                  │
                  ▼
         ┌────────────────┐
         │ Input Choice   │
         └────┬───────────┘
              │
    ┌─────────┴─────────┐
    │                    │
    ▼                    ▼
┌───────┐          ┌──────────┐
│ 1-5   │          │    6     │
└───┬───┘          └────┬─────┘
    │                   │
    ▼                   ▼
┌─────────────┐   ┌─────────────┐
│ Execute     │   │ Exit Loop   │
│ Corresponding│  │ Program End │
│ Function    │   └─────────────┘
└─────────────┘
```

### 3.2 Patient Registration Flow

```
┌─────────────────────────────────────────┐
│      INPUT Patient Name                 │
└─────────────────┬───────────────────────┘
                  │
                  ▼
┌─────────────────────────────────────────┐
│  Apply Regex: ^[a-zA-Z\s]+$             │
└─────────────────┬───────────────────────┘
                  │
         ┌────────┴────────┐
         ▼                 ▼
    ┌─────────┐      ┌─────────────┐
    │ Valid   │─NO──►│ Display     │
    └────┬────┘      │ Error       │
         │YES        └─────────────┘
         ▼
┌─────────────────────────────────────────┐
│  Create Patient Object                  │
│  - fullName = cleanName                 │
│  - email = cleanName + @regenthealth.com│
└─────────────────┬───────────────────────┘
                  │
                  ▼
┌─────────────────────────────────────────┐
│  Add to allPatients List                │
│  Set currentPatient = newPatient        │
│  Log Activity                           │
└─────────────────────────────────────────┘
```

### 3.3 Appointment Booking Flow

```
┌─────────────────────────────────────────┐
│      Display Service Menu               │
│      (4 options with prices)            │
└─────────────────┬───────────────────────┘
                  │
                  ▼
         ┌────────────────┐
         │ Input Choice   │
         └────┬───────────┘
              │
      ┌───────┴───────┐
      ▼               ▼
┌─────────┐     ┌─────────────┐
│ Valid?  │─NO─►│ Error &     │
└────┬────┘     │ Return      │
     │YES        └─────────────┘
     ▼
┌─────────────────────────────────────────┐
│  INPUT Date (yyyy-mm-dd)                │
│  Validate: TryParse + future date       │
└─────────────────┬───────────────────────┘
                  │
         ┌────────┴────────┐
         ▼                 ▼
    ┌─────────┐      ┌─────────────┐
    │ Valid   │─NO──►│ Re-prompt   │
    └────┬────┘      └─────────────┘
         │YES
         ▼
┌─────────────────────────────────────────┐
│  INPUT Time (HH:mm)                     │
│  Regex: 09:00-20:59 OR 21:00            │
└─────────────────┬───────────────────────┘
                  │
         ┌────────┴────────┐
         ▼                 ▼
    ┌─────────┐      ┌─────────────┐
    │ Valid   │─NO──►│ Re-prompt   │
    └────┬────┘      └─────────────┘
         │YES
         ▼
┌─────────────────────────────────────────┐
│  Create Appointment Object              │
│  - Call ClassifyAppointmentPrice()      │
│  - Add to allAppointments List          │
│  - Log Activity                         │
└─────────────────────────────────────────┘
```

### 3.4 Price Classification Flow

```
┌─────────────────────────────────────────┐
│      INPUT Price (double)               │
└─────────────────┬───────────────────────┘
                  │
                  ▼
         ┌────────┴────────┐
         ▼                 ▼
    ┌─────────┐      ┌─────────────┐
    │ price   │─YES─►│ Return      │
    │ < 0?    │      │ "Invalid"   │
    └────┬────┘      └─────────────┘
         │NO
         ▼
    ┌─────────┐      ┌─────────────┐
    │ price   │─YES─►│ Return      │
    │ <= 20?  │      │ "Low Cost"  │
    └────┬────┘      └─────────────┘
         │NO
         ▼
    ┌─────────┐      ┌─────────────┐
    │ price   │─YES─►│ Return      │
    │ <= 40?  │      │ "Standard"  │
    └────┬────┘      └─────────────┘
         │NO
         ▼
    ┌─────────────────────────────────┐
    │ Return "Premium"                │
    └─────────────────────────────────┘
```

---

## 4. Implementation Descriptions

### 4.1 Patient Class Implementation

The Patient class demonstrates **encapsulation** through private fields and read-only properties. The constructor automatically generates a corporate email address using string manipulation:

```csharp
email = name.Replace(" ", "").ToLower() + "@regenthealth.com";
```

This implementation ensures consistent email formatting across all patient records without requiring manual email input.

### 4.2 Appointment Class Implementation

The Appointment class integrates with the static `ClassifyAppointment` utility during construction:

```csharp
this.classification = ClassifyAppointment.ClassifyAppointmentPrice(price);
```

This **composition** pattern allows price classification logic to be reused without inheritance complexity.

### 4.3 BookingSystem Class Implementation

The BookingSystem class serves as the **facade** for the entire application, coordinating between Patient, Appointment, and the user interface. Key implementation features include:

- **Activity Log**: Fixed-size array with FIFO (First-In-First-Out) behavior using array shifting
- **Date Validation**: Uses `DateTime.TryParse()` for safe parsing and compares against `DateTime.Today`
- **Time Validation**: Complex Regex pattern ensures business hours compliance (09:00-21:00)

### 4.4 Input Validation Strategy

The system implements **defense in depth** with multiple validation layers:

1. **Regex validation** for name format (letters only)
2. **TryParse** for numeric conversions
3. **Null checking** with `string.IsNullOrWhiteSpace()`
4. **Boundary checking** for array indices and menu options

---

## 5. Code Analysis and Design Patterns

### 5.1 Authentication System

The login system uses hardcoded credentials ("d" and "r") for demonstration purposes. In production, this would integrate with a proper authentication service.

### 5.2 Menu-Driven Interface

The switch statement in Program.cs provides clear menu navigation:

```csharp
switch (choice)
{
    case "1": ... break;
    case "2": ... break;
    // etc.
}
```

This pattern is appropriate for console applications with finite option sets.

### 5.3 Error Handling

The system uses **graceful degradation** with error messages rather than exceptions for common validation failures, maintaining user experience while preventing invalid data entry.

---

## 6. Conclusions and Recommendations

The Regent Health Booking System successfully demonstrates core C# programming concepts including:

- Object-oriented design with proper encapsulation
- Collection management (List<T> and arrays)
- Input validation using Regex and TryParse
- Static utility methods for business logic
- Session state management with nullable types

**Recommendations for Enhancement:**

1. Replace hardcoded authentication with database-backed credentials
2. Add persistent storage (file or database) for appointments
3. Implement logging framework instead of console output
4. Add unit tests for validation methods
5. Consider async/await for potential future GUI implementation

---

## References

- Gamma, E., Helm, R., Johnson, R., & Vlissides, J. (1995). *Design Patterns: Elements of Reusable Object-Oriented Software*. Addison-Wesley.
- Friedl, J. E. F. (2006). *Mastering Regular Expressions* (3rd ed.). O'Reilly Media.
- Microsoft. (2023). *C# Programming Guide*. Microsoft Docs. https://docs.microsoft.com/en-us/dotnet/csharp/

---

## Appendix: Class Diagram

```
┌─────────────────────────────────────────────┐
│           Regent Health System              │
├─────────────────────────────────────────────┤
│                                             │
│  ┌──────────────┐       ┌──────────────┐   │
│  │   Patient    │       │  Appointment │   │
│  ├──────────────┤       ├──────────────┤   │
│  │ - fullName   │       │ - type       │   │
│  │ - email      │       │ - date       │   │
│  ├──────────────┤       │ - time       │   │
│  │ + FullName   │       │ - price      │   │
│  │ + Email      │       │ - classification│ │
│  └──────────────┘       │ - patientName│   │
│                         │ - patientEmail│   │
│  ┌──────────────┐       ├──────────────┤   │
│  │ClassifyAppt  │       │ + Appointment│   │
│  ├──────────────┤       └──────┬───────┘   │
│  │ + Classify  │◄──────────────┘           │
│  │   Appointment│                            │
│  │   Price()    │                            │
│  └──────────────┘                            │
│                                             │
│  ┌──────────────────────────────────────┐   │
│  │         BookingSystem                │   │
│  ├──────────────────────────────────────┤   │
│  │ - currentAppointment: Appointment?   │   │
│  │ - allPatients: List<Patient>         │   │
│  │ - allAppointments: List<Appointment> │   │
│  │ - activityLog: string[3]             │   │
│  ├──────────────────────────────────────┤   │
│  │ + CreatePatient(name)                │   │
│  │ + BookAppointment()                  │   │
│  │ + ViewSummary()                      │   │
│  │ + ShowHighestLowest()                │   │
│  │ + ShowActivityLog()                  │   │
│  │ + ClearBooking()                     │   │
│  │ + AddActivity(action)                │   │
│  └──────────────────────────────────────┘   │
│                                             │
└─────────────────────────────────────────────┘
```

---

*Report generated for academic evaluation purposes. Total word count: approximately 2000 words.*