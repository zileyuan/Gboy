using Godot;

namespace HappySlime
{
    public class GameComplete : Control
    {
        public override void _Ready()
        {
            var globals = GetNode<Globals>("/root/Globals");
            GetNode<Label>("UI/Stats/CoinsValue").Text = globals.GetCoins().ToString();
            GetNode<Label>("UI/Stats/DeathsValue").Text = globals.Deaths.ToString();
            var duration = globals.CompletedAt - globals.StartedAt;
            var minutes = duration / 60;
            var seconds = duration % 60;
            GetNode<Label>("UI/Stats/TimeValue").Text = $"{minutes}:{seconds:D2}";
        }

        public override void _Input(InputEvent @event)
        {
            base._Input(@event);
            if (@event.IsPressed())
            {
                var globals = GetNode<Globals>("/root/Globals");
                globals.BackToTitle();
            }
        }
    }
}
