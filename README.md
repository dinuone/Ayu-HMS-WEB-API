# Harendra Ayurveda - HMS Web API

Ayu HMS Web API is a robust and scalable backend solution for the Ayurveda Hospital Management System (HMS). Built with **.NET 8**, it provides secure and efficient APIs to manage hospital operations, patient records, appointments, and more.

## 🚀 Features
- 🏥 **Patient Management**: Register, update, and manage patient records.
- 📅 **Appointment Scheduling**: Book, update, and cancel appointments.
- 👨‍⚕️ **Doctor & Staff Management**: Manage hospital staff and doctor availability.
- 📜 **Medical Records**: Store and retrieve patient history and reports.
- 🔐 **Authentication & Authorization**: Secure access using **JWT-based authentication**.
- 📊 **Dashboard API**: Retrieve real-time statistics for hospital administration.
- 📦 **Inventory Management**: Track medicines and medical supplies.

## 🛠️ Technologies Used
- **.NET 8** - Modern and high-performance web API framework.
- **Entity Framework Core** - ORM for database management.
- **MySQL** - Relational database for storing hospital data.
- **JWT Authentication** - Secure authentication and authorization.
- **Swagger (OpenAPI)** - API documentation and testing.

## 📥 Installation & Setup

### **Prerequisites**
Ensure you have the following installed on your system:
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [MySQL Server](https://dev.mysql.com/downloads/)
- [Git](https://git-scm.com/)
- [Postman](https://www.postman.com/) (Optional, for testing APIs)

### **Clone the Repository**
```sh
git clone https://github.com/dinuone/Ayu-HMS-WebAPI.git
cd Ayu-HMS-WebAPI
```

### **Configure Database Connection**
Modify `appsettings.json` with your MySQL credentials:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=ayu_hms;User=root;Password=your_password;"
}
```

The API should now be running at: `https://localhost:5001` or `http://localhost:5000`

## 📄 API Documentation
The API endpoints can be explored using Swagger:
- Navigate to: [`https://localhost:5001/swagger`](https://localhost:5001/swagger)

## 🔑 Authentication
This API uses **JWT-based authentication**. To access secured endpoints:
1. Obtain a **JWT token** by logging in.
2. Use the token in the `Authorization` header:
   ```sh
   Authorization: Bearer YOUR_TOKEN
   ```

## 🛡️ License
This project is proprietary and is intended for **client use only**. Unauthorized distribution or modification is not permitted.

---
🌟 **Ayu HMS Web API** – Powering modern Ayurvedic hospital management with .NET 8!


