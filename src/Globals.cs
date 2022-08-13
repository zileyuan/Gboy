using System;
using Godot;

namespace HappySlime
{
    public class Globals : Node
    {
        private const int BGM_IDX = 1;
        private const int SFX_IDX = 2;
        private const string ConfigPath = "user://setting.cfg";
        [Export]public int CoinCollected = 0;
        [Export]public int CoinPending = 0;
        public int Deaths = 0;
        public long StartedAt = 0;
        public long CompletedAt = 0;

        [Signal]
        public delegate void CoinsChanged();
        [Signal]
        public delegate void DirectionChanged(int direction);


        public void SetCoinPending(int value)
        {
            CoinPending = value;
            EmitSignal(nameof(CoinsChanged));
        }

        public void ReloadWorld()
        {
            Deaths += 1;
            SetCoinPending(0);
            AnimateTransitionTo(null);
        }

        public void GotoWorld(string path)
        {
            CoinCollected += CoinPending;
            SetCoinPending(0);
            AnimateTransitionTo(path);
        }

        public async void AnimateTransitionTo(string path)
        {
            var animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
            animationPlayer.PlayBackwards("fade-in");
            await ToSignal(animationPlayer, "animation_finished");
            if (path != null)
            {
                GetTree().ChangeScene(path);
            }
            else
            {
                GetTree().ReloadCurrentScene();
            }
            animationPlayer.Play("fade-in");
        }

        public bool IsBGMEnabled()
        {
            return !AudioServer.IsBusMute(BGM_IDX);
        }
        
        public void SetBGMEnabled(bool value)
        {
            AudioServer.SetBusMute(BGM_IDX, !value);
        }
        
        public bool IsSFXEnabled()
        {
            return !AudioServer.IsBusMute(SFX_IDX);
        }
        
        public void SetSFXEnabled(bool value)
        {
            AudioServer.SetBusMute(SFX_IDX, !value);
        }

        public void StartGame()
        {
            StartedAt = DateTimeOffset.Now.ToUnixTimeSeconds();
            Deaths = 0;
            CoinCollected = 0;
            SetCoinPending(0);
            GotoWorld("res://scenes/World01.tscn");
        }

        public void CompleteGame()
        {
            CompletedAt = DateTimeOffset.Now.ToUnixTimeSeconds();
            AnimateTransitionTo("res://scenes/GameComplete.tscn");
        }

        public void BackToTitle()
        {
            AnimateTransitionTo("res://scenes/TitleScreen.tscn");
        }

        public int GetCoins()
        {
            return CoinCollected + CoinPending;
        }

        public void SaveConfig()
        {
            var file = new ConfigFile();
            file.SetValue("audio", "bgm_enabled", IsBGMEnabled());
            file.SetValue("audio", "sfx_enabled", IsSFXEnabled());
            var err = file.Save(ConfigPath);
            if (err != Error.Ok)
            {
                GD.PushError($"Failed to save config: {err}");
            }
        }

        public void LoadConfig()
        {
            var file = new ConfigFile();
            var err = file.Load(ConfigPath);
            if (err == Error.Ok)
            {
                var bgm = (bool)file.GetValue("audio", "bgm_enabled", true);
                var sfx = (bool)file.GetValue("audio", "sfx_enabled", true);
                GD.Print($"bgm {bgm}");
                GD.Print($"sfx {sfx}");
                SetBGMEnabled(bgm);
                SetSFXEnabled(sfx);
            }
            else
            {
                GD.PushError($"Failed to load config: {err}");
                SetBGMEnabled(true);
                SetSFXEnabled(true);
            }
        }
    }
}
