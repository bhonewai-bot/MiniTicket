# 🎟️ MiniTicket

**Type:** Web API – Ticket Management System  
**Backend:** ASP.NET Core Web API  
**Database:** MSSQL

---

## 📌 Overview

MiniTicket is a simple ticket system where:

- Users can register and log in
- Users can create tickets and comment on them
- Admins can view all tickets, update ticket status, and reply to users
- Authentication is handled using cookie-based sessions
- Authorization is handled using user roles (User / Admin)

---

## 🔐 Authentication

- Login creates a session stored in the database
- Session ID is saved in an HttpOnly cookie
- Custom middleware validates the session on every request

---

## 👤 User Roles

### User

- Create tickets
- View own tickets
- Comment on own tickets

### Admin

- View all tickets
- Update ticket status
- Reply to any ticket

---

## 🛠️ Tech Notes

- Uses Entity Framework Core
- Passwords are stored using hashed passwords
- Role and UserId are injected via middleware into HttpContext.Items
- No JWT used (session + cookies only)