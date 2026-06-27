using Godot;

public partial class InteractableObject : Area2D
{
    [Export]
    public string DisplayName = "Объект";

    [Export]
    public string ActionText = "E - взаимодействовать";

    [Export]
    public string PopupScenePath = "";

    [Export]
    public Color HighlightColor = new Color(0.4f, 0.9f, 1.0f, 1.0f);

    private bool _playerInside = false;
    private bool _popupOpen = false;
    private bool _wasInteractPressed = false;
    private CanvasItem _visual;
    private Color _normalColor;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;

        _visual = GetNodeOrNull<CanvasItem>("Visual");
        if (_visual != null)
            _normalColor = _visual.Modulate;
    }

    public override void _Process(double delta)
    {
        if (!_playerInside)
            return;

        if (PlayerUI.Instance != null && !PlayerUI.Instance.IsActive(this))
            return;

        bool interactPressed = Input.IsKeyPressed(Key.E);

        if (!_popupOpen && interactPressed && !_wasInteractPressed)
            OpenPopup();

        _wasInteractPressed = interactPressed;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body.Name != "Player")
            return;

        _playerInside = true;
        SetHighlight(true);
        PlayerUI.Instance?.AddObject(this);
    }

    private void OnBodyExited(Node2D body)
    {
        if (body.Name != "Player")
            return;

        _playerInside = false;
        SetHighlight(false);
        PlayerUI.Instance?.RemoveObject(this);
    }

    private void SetHighlight(bool enabled)
    {
        if (_visual == null)
            return;

        _visual.Modulate = enabled ? HighlightColor : _normalColor;
    }

    private void OpenPopup()
    {
        if (string.IsNullOrWhiteSpace(PopupScenePath))
        {
            GD.PushError(DisplayName + ": PopupScenePath пустой");
            return;
        }

        PackedScene popupScene = GD.Load<PackedScene>(PopupScenePath);
        if (popupScene == null)
        {
            GD.PushError(DisplayName + ": не найден popup " + PopupScenePath);
            return;
        }

        Node popup = popupScene.Instantiate();
        GetTree().CurrentScene.AddChild(popup);

        _popupOpen = true;
        popup.TreeExited += () => _popupOpen = false;
    }
}
