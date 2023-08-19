# Contact Management System

## SimpleContactsManagementSystemASP.NETCore7

This is a simple contacts management system implemented as an ASP.NET Core 7 web application using MVC:

- The application allows users to manage contacts by providing features such as adding, editing, and deleting contacts. 
- It also includes authentication functionality to secure the system.
- Each contact has a photo (image using IFormFile), name, email, and phone number
- Video Youtube 
[![ALT_TEXT](http://img.youtube.com/vi/ij6R1QpupVE/0.jpg)](https://www.youtube.com/watch?v=ij6R1QpupVE)

## Features

- **User Authentication**: Users can register and log in to the system using ASP.NET Core Identity. Authentication is required for creating, editing, or viewing contact details.

- **Contacts List**: The home and Index pages display a list of contacts with their photo (using IFormFile interface), name, email, and phone number.

- **Contact Details**: Clicking on a contact in the list opens a separate page that shows detailed information about the contact.

- **Contact Management**: Users can add new contacts, edit existing ones, and delete contacts they no longer need.

- **Data Access**: The application uses Entity Framework Core for data access, providing seamless integration with the database and efficient retrieval and manipulation of contact information.

- **Proper Routing**: The system implements proper routing for different actions and pages, ensuring intuitive navigation and easy access to specific functionalities.

- **Razor HTML Templates**: Razor HTML templates are used to render the application's pages. This templating engine enables dynamic content generation and ensures a consistent and visually pleasing user interface.

- **Basic Validation**: 
Basic validation for required fields (Name, Email, Phone) is implemented to ensure they are filled out correctly.
The Email field is validated to ensure it is in the correct email format.
The Phone field is validated using a regular expression to enforce a minimum of 10 digits, accepting the "+" character at the beginning.
Uploaded image files are validated to only allow image files with content types of "image/jpeg", "image/png", "image/gif", or "image/jpg".
The image file size is checked to ensure it does not exceed the maximum limit of 1MB.

- **Pagination**: To enhance user experience, the contact list is paginated, allowing users to navigate through a large number of contacts more easily.

- **Image Management**: When a contact is edited or deleted, the system also removes the corresponding image file associated with the contact. This ensures that outdated or unnecessary image files are not retained, optimizing storage usage.
  

## These Commands may help you in the Implementation using CLI or Visual Studio Code (VS Code)

. Install the required tools by running the following commands:

   ```
   dotnet tool install --global dotnet-ef
   dotnet tool install --global dotnet-aspnet-codegenerator
   ```

. Install the necessary packages by running the following commands:

   ```
   dotnet add package Microsoft.EntityFrameworkCore
   dotnet add package Microsoft.EntityFrameworkCore.Design
   dotnet add package Microsoft.EntityFrameworkCore.SqlServer
   dotnet add package Microsoft.EntityFrameworkCore.Tools
   dotnet add package Microsoft.AspNetCore.Identity.UI
   dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
   ```

. Make a new dotnet project:

  with individual user authentication 
  ```
  dotnet new mvc -o ContactManager -au individual
  ```
  or 
  without any authentication components
  ```
  dotnet new mvc -o ContactManager
  ```

. Build and restore the project by running the following commands:

   ```
   dotnet restore
   dotnet build
   ```

. Scaffolding the ContactsController and Contacts Views codes
  ```
  dotnet aspnet-codegenerator controller -name ContactsController -m Contact -dc ApplicationDbContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries
  ```

. Forced Scaffolding of the ContactsController and Contacts Views codes, for the second time
  ```
  dotnet aspnet-codegenerator controller -name ContactsController -m Contact -dc ApplicationDbContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries -f
  ```

. Apply the database migrations by running the following commands:

   ```
   dotnet ef migrations add Contact_mig
   dotnet ef database update
   ```

. Update the database to the initial migration "0" by running the following command:

   ```
   dotnet ef database update 0
   ```

. Removes the last added migration by running the following command:

   ```
   dotnet ef migrations remove
   ```

. Run the application with the following command:

   ```
   dotnet run
   ```

. Access the application in your web browser at `http://localhost:<yourport>`.


## Conclusion


The Simple Contact Management System provides a user-friendly solution for organizing and maintaining contacts. With its authentication system, contact list, detailed contact information, and management capabilities, users can conveniently add, edit, and delete contacts. The integration with Entity Framework Core ensures efficient data access, while proper routing and pagination enhance usability. Additionally, the system takes care of image management by removing outdated image files when contacts are edited or deleted.
