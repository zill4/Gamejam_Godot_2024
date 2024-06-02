using Godot;
using System;

public partial class PopupBubble : Node3D
{
    private Sprite3D[] _sprites;
    private const float SpriteScaleFactor = 0.25f; // Adjust this value to make the sprites smaller or larger

    public override void _Ready()
    {
        _sprites = new Sprite3D[4];
        _sprites[0] = GetNode<Sprite3D>("CupSprite");
        _sprites[1] = GetNode<Sprite3D>("MilkSprite");
        _sprites[2] = GetNode<Sprite3D>("SyrupSprite");
        _sprites[3] = GetNode<Sprite3D>("EspressoSprite");

        Hide(); // Hide the bubble initially
    }

    public void ShowBubble()
    {
        Show();
    }

    public void HideBubble()
    {
        Hide();
    }

    public void SetSprite(int index, Texture2D texture)
    {
        if (index >= 0 && index < _sprites.Length)
        {
            _sprites[index].Texture = texture;
            _sprites[index].Visible = true;
			_sprites[index].Scale = new Vector3(SpriteScaleFactor, SpriteScaleFactor, SpriteScaleFactor); // Set the scale
        }
    }

    public void Clear()
    {
        foreach (var sprite in _sprites)
        {
            sprite.Texture = null;
            sprite.Visible = false;
        }
    }
}
