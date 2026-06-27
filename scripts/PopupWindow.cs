using Godot;

public partial class PopupWindow : CanvasLayer
{
    private bool _wasCancelPressed = false;

    public override void _Ready()
    {
        Button closeButton = GetNodeOrNull<Button>("Panel/CloseButton");
        if (closeButton != null)
            closeButton.Pressed += ClosePopup;
    }

    public override void _Process(double delta)
    {
        bool cancelPressed = Input.IsKeyPressed(Key.Escape);

        if (cancelPressed && !_wasCancelPressed)
            ClosePopup();

        _wasCancelPressed = cancelPressed;
    }

    private void ClosePopup()
    {
        QueueFree();
    }
}
