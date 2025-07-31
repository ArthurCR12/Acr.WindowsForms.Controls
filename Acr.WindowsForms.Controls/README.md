# AcrControls - Custom WinForms Controls


**AcrControls** is a lightweight library of customizable Windows Forms controls built for modern .NET applications.

Developed by **Arthur Cabral**, this package aims to improve user experience and developer productivity

## Features

### Customizable TextBox
- **Input Type**: Like All, Integer, Decimal, Letter, Alphanumeric
- **Dynamic Styling**: Changes background color on `Enter` and `Leave` events for better user interaction.
- **Enhanced Navigation**: Pressing the `Enter` key moves focus to the next control, improving form usability.
- **Text Selection**: Automatically selects all text when the control gains focus.
- **Date Validation**: Supports validation for dates in `dd/MM/yyyy` format.
- **Label Integration**: Displays a customizable label title above the control.

### Label Helper
- **Flexible Positioning**: Adds a helper label above or below any control to provide context or instructions.
- **Versatile Usage**: Compatible with any Windows Forms control.
- **Customizable Messages**: Supports different message types (e.g., Warning, Info).
- **Usage Example**:
  ```csharp
  LabelHelper.CreateLabel(myTextBox, "Required field", MessageType.Warning);

### Notification Helper ->
- User Notifications: Displays messages in the bottom-right corner of the screen.
- Message Types: Supports Success, Error, Warning, and Information notifications.
- **Usage Example**:
  ```csharp
  NotificationHelper.Show("Operation completed successfully", NotificationType.Success);
  ```



	
    
	



