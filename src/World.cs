using Godot;

namespace Gboy
{
    public class World : Node2D
    {
        public override void _Ready()
        {
            base._Ready();
            GetNode("CanvasLayer/VirtualJoystick").Connect("analogic_chage", this, nameof(MoveTest));
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
            
            var packedScene = ResourceLoader.Load("res://scenes/HUD.tscn") as PackedScene;
            AddChild(packedScene.Instance());
            
            packedScene = ResourceLoader.Load("res://scenes/Coin.tscn") as PackedScene;
            InstanceTiles("coin", packedScene);
        }
        
        public void MoveTest(Vector2 mov)
        {
            GD.Print($"MoveTest {mov}");
            var player = GetNode<Player>("Player");
            player._velocity = new Vector2(mov.x * 350, player._velocity.y);
        }

        public void InstanceTiles(string name, PackedScene packedScene)
        {
            var tilemap = GetNode<TileMap>("TileMap");
            var id = tilemap.TileSet.FindTileByName(name);
            if (id != -1)
            {
                foreach (Vector2 pos in tilemap.GetUsedCellsById(id))
                {
                    var node = packedScene.Instance<Coin>();
                    tilemap.AddChild(node);
                    node.Position = tilemap.MapToWorld(pos) + tilemap.CellSize / 2;
                    tilemap.SetCellv(pos, -1);
                }
            }
        }

        // public override void _Input(InputEvent @event)
        // {
        //     base._Input(@event);
        //     if (@event.IsActionPressed("ui_cancel"))
        //     {
        //         // var busIndex = 2;
        //         // AudioServer.SetBusMute(busIndex, !AudioServer.IsBusMute(busIndex));
        //         var globals = GetNode<Globals>("/root/Globals");
        //         globals.BackTitle();
        //     }
        // }
    }
}