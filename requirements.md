# Purchase Bill System — Requirements

## 1. Project Overview

A two-page web application built with Angular and ASP.NET Core Web API.

The application will provide:

1. A Login page for authenticating users through the provided external POS API.
2. A Purchase Bill page accessible only after successful authentication.

The project should demonstrate clean software engineering practices, reusable components, responsive design, strong typing, REST API integration, validation, error handling, and maintainable project structure.

---

## 2. Technology Stack

### Frontend
- Angular (latest stable version)
- TypeScript
- Tailwind CSS
- Angular Reactive Forms
- Angular Router
- HttpClient

### Backend
- ASP.NET Core Web API
- .NET 8
- C#
- Entity Framework Core

### Database
- Microsoft SQL Server

### Development Tools
- Visual Studio Code / Visual Studio
- Postman
- Git

---

# 3. Functional Requirements

## 3.1 Login

The application must provide a Login page based on the provided UI design.

### Login Fields

- Email
- Password

### Validation

The form must:
- Require the email field.
- Validate that the email has a valid email format.
- Require the password field.
- Display meaningful validation messages.
- Prevent submission when validation fails.

### Authentication

The backend must authenticate the user using the provided external POS API.

API endpoint:

`POST https://ez-staging-api.azurewebsites.net/api/External_Api/POS_Api/Invoke`

The request must follow the API specification provided by the assignment.

The login request uses the values entered in the Angular form:

- Email → `Company_Code`
- Email → `API_Body.Username`
- Password → `API_Body.Pw`

Example:

```json
{
  "API_Action": "GetLoginData",
  "Device_Id": "D001",
  "Sync_Time": "",
  "Company_Code": "<email>",
  "API_Body": {
    "Username": "<email>",
    "Pw": "<password>"
  }
}
```

### Successful Login

A successful response contains user information and a `User_Locations` collection.

The application must:

1. Verify that authentication was successful.
2. Store the authenticated application session securely.
3. Extract the user's locations.
4. Save the locations to the `Location_Details` SQL Server table.
5. Allow the authenticated user to access the Purchase Bill page.

### Failed Login

If authentication fails:

- The user must not be allowed to access the Purchase Bill page.
- A meaningful error message must be displayed.
- Loading state must end correctly.
- Sensitive information such as passwords must never be displayed or logged.

---

# 4. Location Management

The successful login response provides locations similar to:

```json
"User_Locations": [
  {
    "Location_Code": "dsfsfsdf",
    "Location_Name": "xcvC"
  },
  {
    "Location_Code": "dsffsdfs",
    "Location_Name": "dsfdsfds"
  }
]
```

These locations must be stored in SQL Server.

## Location_Details Table

Suggested fields:

| Field | Type | Description |
|---|---|---|
| Id | int | Primary key |
| Location_Code | nvarchar | Location code returned by API |
| Location_Name | nvarchar | Location name returned by API |

Additional fields may be added where technically useful.

---

# 5. Authentication and Route Protection

The application must prevent unauthenticated users from accessing the Purchase Bill page.

### Requirements

- Use an Angular route guard.
- Maintain authenticated session state.
- Redirect unauthenticated users to `/login`.
- Prevent direct navigation to `/purchase-bill` without authentication.
- Do not assume JWT unless the provided authentication API actually supplies or requires a token.
- The authentication/session approach should be based on the actual API response and assignment requirements.

---

# 6. Purchase Bill

The Purchase Bill page must be accessible only after successful login.

## 6.1 Item Field

The Item field must support autocomplete.

The available fixed items are:

- Mango
- Apple
- Banana
- Orange
- Grapes
- Kiwi
- Strawberry

The user should be able to select an item from the available values.

---

## 6.2 Batch / Location

The Batch dropdown must be populated using the `Location_Name` values stored in `Location_Details`.

Example:

- Block C
- Head Office

The selected location should retain the relationship with its corresponding `Location_Code`.

---

## 6.3 Purchase Bill Fields

The form should contain:

- Item
- Batch / Location
- Standard Cost
- Standard Price
- Quantity
- Discount
- Total Cost
- Total Selling

---

# 7. Purchase Bill Validation

The form must validate user input.

### Required Fields

- Item
- Batch / Location
- Standard Cost
- Standard Price
- Quantity
- Discount

### Numeric Validation

- Standard Cost must be zero or greater.
- Standard Price must be zero or greater.
- Quantity must be greater than zero.
- Discount must be between 0% and 100%.

Meaningful validation messages should be shown to the user.

---

# 8. Purchase Bill Calculations

## Total Cost

Formula:

```text
Gross Cost = Standard Cost × Quantity

Discount Amount = Gross Cost × Discount / 100

Total Cost = Gross Cost - Discount Amount
```

Example:

```text
Standard Cost = 100
Quantity = 5
Discount = 20%

Gross Cost = 100 × 5 = 500
Discount = 500 × 20% = 100
Total Cost = 500 - 100 = 400
```

## Total Selling

Formula:

```text
Total Selling = Standard Price × Quantity
```

Example:

```text
Standard Price = 150
Quantity = 5

Total Selling = 150 × 5 = 750
```

Calculations should update automatically when relevant form values change.

---

# 9. Add Purchase Item

When the user clicks **Add**:

1. Validate the form.
2. Calculate Total Cost.
3. Calculate Total Selling.
4. Add the item to the purchase items table.
5. Clear/reset the appropriate form fields.
6. Update the Item Summary.
7. Handle API submission/errors appropriately.

---

# 10. Purchase Item Table

The added purchase items must be displayed in a table below the form.

The table should display relevant information such as:

- Item
- Batch / Location
- Standard Cost
- Standard Price
- Quantity
- Discount
- Total Cost
- Total Selling

The table should be responsive.

---

# 11. Item Summary

The Purchase Bill page must display:

### Total Items

The number of items/rows added to the table.

```text
Total Items = purchaseItems.length
```

### Total Quantity

The sum of all quantity values.

```text
Total Quantity = sum of all item quantities
```

Example:

```text
Item 1 → Quantity 5
Item 2 → Quantity 3
Item 3 → Quantity 2

Total Items = 3
Total Quantity = 10
```

---

# 12. Backend API

The ASP.NET Core backend should act as the application's API layer.

Suggested endpoints:

```text
POST /api/auth/login

GET /api/locations

POST /api/purchase-bills
```

The exact endpoint names may be adjusted during implementation.

## Responsibilities

### AuthController
- Receive login request.
- Validate request.
- Call the external POS API.
- Process authentication response.
- Save user locations.
- Return appropriate response to Angular.

### LocationController
- Retrieve locations from SQL Server where required.

### PurchaseBillController
- Receive purchase bill data.
- Validate the request.
- Perform server-side validation/calculation where appropriate.
- Persist purchase bill data if required by the final implementation.

---

# 13. Database

Use Entity Framework Core with SQL Server.

Required table:

```text
Location_Details
```

A purchase bill table may also be created if purchase bill persistence is implemented.

Suggested:

```text
Purchase_Bills
```

Possible fields:

| Field | Type |
|---|---|
| Id | int |
| Item | nvarchar |
| Location_Code | nvarchar |
| Location_Name | nvarchar |
| Standard_Cost | decimal |
| Standard_Price | decimal |
| Quantity | int |
| Discount | decimal |
| Total_Cost | decimal |
| Total_Selling | decimal |
| Created_Date | datetime |

---

# 14. Angular Architecture

The frontend should follow a component-based architecture.

Suggested structure:

```text
src/app/
├── core/
│   ├── guards/
│   │   └── auth.guard.ts
│   ├── interceptors/
│   │   └── auth.interceptor.ts
│   └── services/
│       ├── auth.service.ts
│       ├── location.service.ts
│       └── purchase-bill.service.ts
│
├── features/
│   ├── login/
│   │   └── login.component.ts
│   │
│   └── purchase-bill/
│       ├── purchase-bill.component.ts
│       ├── purchase-bill-form/
│       └── purchase-bill-table/
│
├── models/
│   ├── login-request.ts
│   ├── login-response.ts
│   ├── location.ts
│   └── purchase-item.ts
│
├── app.routes.ts
└── app.config.ts
```

The exact structure can be adjusted while maintaining separation of concerns.

---

# 15. Backend Architecture

Suggested structure:

```text
backend/
├── Controllers/
│   ├── AuthController.cs
│   ├── LocationController.cs
│   └── PurchaseBillController.cs
│
├── Services/
│   ├── AuthService.cs
│   ├── LocationService.cs
│   └── PurchaseBillService.cs
│
├── Data/
│   └── ApplicationDbContext.cs
│
├── Models/
│   ├── LocationDetail.cs
│   └── PurchaseBill.cs
│
├── DTOs/
│   ├── LoginRequest.cs
│   ├── LoginResponse.cs
│   └── PurchaseBillRequest.cs
│
└── Program.cs
```

---

# 16. Reusability

The application should use reusable components where practical.

Examples:

- Form input components
- Validation message components
- Button/loading states
- Purchase item table
- Summary section

Avoid unnecessary duplication of UI and business logic.

---

# 17. Responsive UI

The application must work correctly on:

- Desktop
- Tablet
- Mobile

Tailwind CSS should be used for responsive styling.

The UI should follow the provided design as closely as practical while maintaining good usability.

---

# 18. Error Handling

The application should handle:

- Invalid login credentials
- External API errors
- Network failures
- Server errors
- Invalid form data
- Database errors
- Unexpected API responses

Users should receive meaningful messages without exposing technical details or sensitive information.

---

# 19. Loading States

Display loading indicators during asynchronous operations.

Examples:

- Login request in progress
- Loading locations
- Submitting purchase bill

Buttons should be disabled where appropriate to prevent duplicate submissions.

---

# 20. Security Requirements

- Never hard-code user passwords in the frontend.
- Never log passwords.
- Do not expose sensitive API credentials in source control.
- Use environment/configuration variables for sensitive configuration where applicable.
- Validate input on both frontend and backend.
- Protect authenticated routes.
- Use HTTPS for API communication.
- Do not store sensitive information unnecessarily in browser storage.

---

# 21. Git Requirements

The project should use Git for version control.

Commits should be meaningful and incremental.

Example commit history:

```text
chore: initialize Angular project
chore: initialize ASP.NET Core API
feat: add external login API integration
feat: add location persistence
feat: implement Angular login form
feat: add authentication route guard
feat: implement purchase bill form
feat: add purchase item calculations
feat: add purchase item table and summary
style: improve responsive layout
fix: handle login API errors
```

Do not commit:

```text
node_modules/
dist/
.env
secrets
passwords
API keys
```

---

# 22. Out of Scope

The following are not required unless specifically requested later:

- User registration/signup
- Password reset
- Admin role management
- Complex role-based access control
- OAuth/social login
- Product inventory management
- Full accounting functionality
- Advanced reporting
- Production deployment

---

# 23. Acceptance Criteria

The project is considered complete when:

- [ ] Angular application runs successfully.
- [ ] ASP.NET Core API runs successfully.
- [ ] SQL Server database is connected.
- [ ] Login form matches the provided design reasonably.
- [ ] Login validation works.
- [ ] External POS API authentication works.
- [ ] Failed authentication is handled correctly.
- [ ] Successful login establishes an authenticated session.
- [ ] User locations are extracted from the login response.
- [ ] User locations are saved to `Location_Details`.
- [ ] Purchase Bill page is protected.
- [ ] Unauthenticated users are redirected to Login.
- [ ] Item autocomplete works with the seven specified items.
- [ ] Batch/location dropdown loads locations from the database.
- [ ] Purchase Bill validation works.
- [ ] Total Cost calculation is correct.
- [ ] Total Selling calculation is correct.
- [ ] Added items appear in the table.
- [ ] Total Items updates correctly.
- [ ] Total Quantity updates correctly.
- [ ] Backend API integration works.
- [ ] Loading states are implemented.
- [ ] Error handling is implemented.
- [ ] UI is responsive.
- [ ] Code is organized using reusable components/services.
- [ ] Git history contains meaningful commits.
- [ ] Sensitive information is not committed to the repository.

---

# 24. Development Order

Recommended implementation sequence:

```text
1. Initialize Git repository
2. Set up Angular frontend
3. Set up ASP.NET Core backend
4. Configure SQL Server + EF Core
5. Implement external login API integration
6. Implement Location_Details persistence
7. Test backend login with Postman
8. Build Angular Login page
9. Connect Angular Login to backend
10. Implement authentication/session handling
11. Implement route guard
12. Build Purchase Bill form
13. Implement location dropdown
14. Implement item autocomplete
15. Implement calculations and validation
16. Implement purchase item table
17. Implement Item Summary
18. Add responsive UI refinements
19. Test complete user flow
20. Clean up code and create final Git commits
```

---

# 25. End-to-End User Flow

```text
User opens application
        ↓
      Login
        ↓
Enter Email + Password
        ↓
Angular validates form
        ↓
ASP.NET Core Login API
        ↓
External POS API
        ↓
Authentication successful?
       /      No   Yes
     ↓      ↓
 Show     Extract
 error    locations
             ↓
       Save locations
       to SQL Server
             ↓
       Create session
             ↓
      Purchase Bill
             ↓
     Select Item + Batch
             ↓
      Enter quantities,
      prices & discount
             ↓
        Calculate
             ↓
           Add
             ↓
      Display in table
             ↓
       Update Summary
```

---

## Project Goal

Build a clean, responsive, maintainable two-page Purchase Bill application that demonstrates practical Angular, ASP.NET Core, SQL Server, REST API integration, authentication, validation, database persistence, reusable components, and Git-based development.
