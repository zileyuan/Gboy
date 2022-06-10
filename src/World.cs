using Godot;

namespace Gboy
{
    public class World : Node2D
    {
        public override void _Ready()
        {
            base._Ready();
            var tilemap = GetNode<TileMap>("TileMap");
            var camera = GetNode<Camera2D>("Player/Camera2D");
            var rect = tilemap.GetUsedRect();
 
            var bounds = new Rect2(
                rect.Position.x * tilemap.CellSize.x,
                rect.Position.y * tilemap.CellSize.y,
                rect.Size.x * tilemap.CellSize.x,
                rect.Size.y * tilemap.CellSize.y
            );
            camera.LimitBottom = (int)(bounds.End.y);
            camera.LimitLeft = (int)(bounds.Position.x);
            camera.LimitRight = (int)(bounds.End.x);
            camera.ForceUpdateScroll();
            camera.ResetSmoothing();

            var shape = new RectangleShape2D();
            shape.Extents = new Vector2(bounds.Size.x / 2, tilemap.CellSize.y / 2);
            GetNode<CollisionShape2D>("Hitbox/CollisionShape2D").Shape = shape;
            GetNode<Hitbox>("Hitbox").Position = new Vector2(
                bounds.Position.x + bounds.Size.x / 2,
                bounds.End.y + tilemap.CellSize.y * 2);

            var packedScene = GD.Load<PackedScene>("res://scenes/Coin.tscn");
            InstanceTiles("coin", packedScene);

            packedScene = GD.Load<PackedScene>("res://scenes/FlyBlack.tscn");
            InstanceTiles("fly_black", packedScene);

            packedScene = GD.Load<PackedScene>("res://scenes/FlyYellow.tscn");
            InstanceTiles("fly_yellow", packedScene);

            packedScene = GD.Load<PackedScene>("res://scenes/SlimeGreen.tscn");
            InstanceTiles("slime_green", packedScene);

            packedScene = GD.Load<PackedScene>("res://scenes/SlimeBlue.tscn");
            InstanceTiles("slime_blue", packedScene);

            if (OS.HasFeature("mobile"))
            {
                packedScene = GD.Load<PackedScene>("res://scenes/Controller.tscn");
                AddChild(packedScene.Instance());
            }

            packedScene = GD.Load<PackedScene>("res://scenes/HUD.tscn");
            AddChild(packedScene.Instance());
        }

        public void InstanceTiles(string name, PackedScene packedScene)
        {
            var tilemap = GetNode<TileMap>("TileMap");
            var id = tilemap.TileSet.FindTileByName(name);
            if (id != -1)
            {
                foreach (Vector2 pos in tilemap.GetUsedCellsById(id))
                {
                    var node = packedScene.Instance<Node2D>();
                    tilemap.AddChild(node);
                    node.Position = tilemap.MapToWorld(pos) + tilemap.CellSize / 2;
                    tilemap.SetCellv(pos, -1);
                }
            }
        }
    }
}