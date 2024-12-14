using Godot;
using System;
using TheGameData;

public partial class TheGame : Node
{
	public enum State { Transition, Menu, Battle }

	[ExportGroup("Main")]
	[Export]
	public Resource GC { get; set; }

	[Export]
	public Node3D MainMenu { get; set; }
	[Export]
	public Node3D BattleArea { get; set; }

	[ExportGroup("Menu")]
	//[Export]
	//public string InterestItemResFolder { get; set; }
	[Export(PropertyHint.Range, "-5,5,0.25")]
	public float InterestRotateSpeed { get; set; } = 2f;
	[Export(PropertyHint.Range, "0.1,15,0.1")]
	public float OverlayAnimationDuration { get; set; } = 5f;
	[Export]
	public Button ButtonBattle { get; set; }
	[Export]
	public Button ButtonOptions { get; set; }
	[Export]
	public Button ButtonExit { get; set; }

	// Main
	protected State CurrentState { get; set; }

	// Menu
	protected Node3D InterestItem { get; set; }
	protected TransitionOverlay Overlay { get; set; }
	protected CanvasLayer OverlayVisual { get; set; }

	// Overrides
	public override void _Ready()
	{
		//if (String.IsNullOrEmpty(InterestItemResFolder)) { InterestItemResFolder = "objects"; }

		// List objects from folder to get random one
		// hard coded for initial set up
		var scene = GD.Load<PackedScene>("res://objects/model_ship_a.tscn");

		InterestItem = scene.Instantiate<Node3D>();
		InterestItem.Name = "MainMenuInterestItem";
		AddChild(InterestItem);

		ButtonBattle.Pressed += ButtonBattle_Trigger;
		ButtonOptions.Pressed += ButtonOptions_Trigger;
		ButtonExit.Pressed += ButtonExit_Trigger;

		var LabelGame = GetNode<Label>("MainMenu/TopMenu/VBoxContainer/LabelGame");
		var LabelPlayer = GetNode<Label>("MainMenu/TopMenu/VBoxContainer/LabelPlayer");
		var LabelVersion = GetNode<Label>("MainMenu/TopMenu/VBoxContainer/LabelVersion");
		Overlay = GetNode<TransitionOverlay>("TransitionOverlay");
		OverlayVisual = GetNode<CanvasLayer>("TransitionOverlay");

		if (GC is GameConfig gc)
		{
			LabelGame.Text = GameConfig.GameName;
			LabelPlayer.Text = String.Format("Player: {0}", String.IsNullOrEmpty(gc.PlayerName) ? "<setup in Options>" : gc.PlayerName);
			LabelVersion.Text = String.Format("Version: {0}", GameConfig.GameVersion);
		}

		CurrentState = State.Menu;
	}

	public override void _Process(double delta)
	{
		if (CurrentState == State.Menu)
		{
			if (InterestItem != null)
			{
				InterestItem.RotateObjectLocal(Vector3.Up, (float)delta * InterestRotateSpeed);
			}
		}
		else if (CurrentState == State.Transition)
		{

		}
	}

	public override void _Notification(int what)
	{
		if (what == NotificationWMCloseRequest)
			GetTree().Quit(); // default behavior
	}

	// Menu
	public void ButtonBattle_Trigger()
	{
		OverlayVisual.Visible = true;
		Overlay.StartAnimation(OverlayAnimationEnd, OverlayAnimationHalf, OverlayAnimationDuration);

		//var world_battle = GD.Load<PackedScene>("res://scenes/world_battle.tscn");
		//GetTree().ChangeSceneToPacked(world_battle);
	}

	public void ButtonOptions_Trigger()
	{

	}

	public void ButtonExit_Trigger()
	{
		GetTree().Root.PropagateNotification((int)NotificationWMCloseRequest);
	}

	public void OverlayAnimationHalf()
	{
		if (MainMenu.Visible)
		{
			// Hide main menu
			MainMenu.Visible = false;
			
			// Activate Battle
		}
		else
		{
			// Deactivate Battle

			// Show main menu
			MainMenu.Visible = true;
		}
	}

	public void OverlayAnimationEnd()
	{
		OverlayVisual.Visible = false;
	}
}
