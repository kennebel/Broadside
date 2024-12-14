using Godot;
using System;
using TheGameData;


public partial class TheGame : Node
{
	public enum State { Transition, Menu, Battle }

	[Export]
	public Resource GC { get; set; }

	[Export]
	public ColorRect Overlay { get; set; }
	[Export]
	public Node3D MainMenu { get; set; }
	[Export]
	public Node3D BattleArea { get; set; }

	//[Export]
	//public string InterestItemResFolder { get; set; }
	[Export]
	public float InterestRotate
	{
		get { return _InterestRotate; }
		set { _InterestRotate = Math.Clamp(value, -25f, 25f); }
	}
	[Export]
	public Button ButtonBattle { get; set; }
	[Export]
	public Button ButtonOptions { get; set; }
	[Export]
	public Button ButtonExit { get; set; }

	private float _InterestRotate = 5f;
	protected Node3D InterestItem { get; set; }

	protected State CurrentState { get; set; }

	public override void _Ready()
	{
		//if (String.IsNullOrEmpty(InterestItemResFolder)) { InterestItemResFolder = "objects"; }

		// List objects from folder to get random one
		// hard coded for initial set up
		var scene = GD.Load<PackedScene>("res://objects/model_ship_a.tscn");

		InterestItem = scene.Instantiate<Node3D>();
		InterestItem.Name = "MainMenuInterestItem";
		AddChild(InterestItem);

		ButtonBattle.Pressed += LoadBattle;
		//ButtonOptions.Pressed += ???;
		ButtonExit.Pressed += ButtonExit_Trigger;

		var LabelGame = GetNode<Label>("MainMenu/TopMenu/VBoxContainer/LabelGame");
		var LabelPlayer = GetNode<Label>("MainMenu/TopMenu/VBoxContainer/LabelPlayer");
		var LabelVersion = GetNode<Label>("MainMenu/TopMenu/VBoxContainer/LabelVersion");

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
				InterestItem.RotateObjectLocal(Vector3.Up, (float)delta * InterestRotate);
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

	public void LoadBattle()
	{
		
		//var world_battle = GD.Load<PackedScene>("res://scenes/world_battle.tscn");
		//GetTree().ChangeSceneToPacked(world_battle);
	}

	public void ButtonExit_Trigger()
	{
		GetTree().Root.PropagateNotification((int)NotificationWMCloseRequest);
	}
}