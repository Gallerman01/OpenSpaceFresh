using Godot;
using System.Collections.Generic;

public partial class PlayerUI : CanvasLayer
{
    public static PlayerUI Instance { get; private set; }

    private Label _listLabel;
    private readonly List<InteractableObject> _items = new();
    private int _activeIndex = 0;

    public override void _Ready()
    {
        Instance = this;
        _listLabel = GetNode<Label>("Panel/EnvironmentListLabel");
        Refresh();
    }

    public override void _ExitTree()
    {
        if (Instance == this)
            Instance = null;
    }

    public override void _Process(double delta)
    {
        if (_items.Count > 1 && Input.IsKeyPressed(Key.Tab))
        {
            // Простая защита от слишком быстрого переключения будет добавлена позже.
            // Пока активным остается первый найденный объект.
        }
    }

    public void AddObject(InteractableObject obj)
    {
        if (!_items.Contains(obj))
            _items.Add(obj);

        if (_activeIndex >= _items.Count)
            _activeIndex = 0;

        Refresh();
    }

    public void RemoveObject(InteractableObject obj)
    {
        _items.Remove(obj);

        if (_activeIndex >= _items.Count)
            _activeIndex = 0;

        Refresh();
    }

    public bool IsActive(InteractableObject obj)
    {
        if (_items.Count == 0)
            return false;

        return _items[_activeIndex] == obj;
    }

    private void Refresh()
    {
        if (_listLabel == null)
            return;

        if (_items.Count == 0)
        {
            _listLabel.Text = "No nearby interactable objects.";
            return;
        }

        string text = "";

        for (int i = 0; i < _items.Count; i++)
        {
            string marker = i == _activeIndex ? "> " : "  ";
            text += marker + _items[i].DisplayName + ": " + _items[i].ActionText + "\n";
        }

        _listLabel.Text = text;
    }
}
