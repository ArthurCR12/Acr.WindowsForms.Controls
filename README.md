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

### Modal (AcrModal)
- **Background dimming**: `DimBackground` (default `true`, global via `AcrModal.DefaultDimBackground`) darkens the owner window while the modal is open. Adjust with `OverlayOpacity` / `OverlayColor`.
- `Esc` closes the modal, `Enter` triggers the primary button.
- **Usage Example**:
  ```csharp
  AcrModal.ShowInfo(this, "Saved!");                         // with dark overlay
  AcrModal.ShowConfirm(this, "Delete?", dimBackground: false); // without overlay

  using var modal = new AcrModal("Title", "Message") { OverlayOpacity = 0.6 };
  modal.SetButtons(("Cancel", DialogResult.Cancel, false), ("OK", DialogResult.OK, true));
  var result = modal.ShowModal(this);
  ```

### Button (AcrButton)
- `Variant`: Primary, Secondary, Outline, Ghost, Danger, Success.
- `IsLoading` shows a spinner (with `LoadingText`) and blocks clicks.
- Smooth anti-aliased rounded corners, hover/pressed states and focus cue.

### Notifications
- `NotificationHelper.Success/Error/Warning/Info(message, title)`, custom duration and `NotificationHelper.PlaySound`.
- Timer pauses while the mouse is over the notification; progress bar shows remaining time; click to dismiss.

### Other improvements
- **AcrTextBox**: `BorderColor`, `BorderHoverColor`, `BorderFocusColor`, `BorderErrorColor`; red border while a validation error is shown (`HasError`); the designer Font is no longer overwritten.
- **AcrDataGridView**: hover row highlight (`HighlightHoverRow`, `HoverRowColor`), `EmptyText` when there are no rows, double buffering, full-row selection.
- **AcrCard**: `TitleColor`, `AccentColor` stripe, `BorderWidth`, `HoverEffect` and `CardClick` event.
- **AcrDropdownMenu**: keyboard navigation (↑ ↓ Enter Esc), separators (`"-"`), `DisabledIndexes`, scroll with `MaxVisibleItems`, stays inside the screen, drop shadow, `Closed` event.
- **AcrAccordion**: expand/collapse animation (`Animate`), `CollapseSiblings` (only one open at a time), header colors, keyboard support (Enter/Space) and `Expand()/Collapse()/Toggle()`.

# Preview TextBox
![Preview TextBox](https://github.com/user-attachments/assets/1f543042-08bd-48de-84f3-8c600c20decb)



