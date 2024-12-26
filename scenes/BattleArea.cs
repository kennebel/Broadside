using Godot;
using System;
using System.Collections.Generic;

public partial class BattleArea : Node3D
{
	[Export]
	public TheGame TG { get; set; }
	[Export]
	public string TestObject { get; set; }
	[Export]
	public Node3D SpawnPos0 { get; set; }
	[Export]
	public Node3D SpawnPos1 { get; set; }
	[Export]
	public string StarObject { get; set; }

	[Export]
	public Button ButtonSpawn { get; set; }
	[Export]
	public Button ButtonSpawnTurrets { get; set; }
	[Export]
	public Button ButtonClear { get; set; }
	[Export]
	public Button ButtonPause { get; set; }
	[Export]
	public Button ButtonMainMenu { get; set; }

	protected List<Node3D> LoadedObjects { get; set; }
	protected Node3D StarHolder { get; set; }
	protected Node3D ObjectHolder { get; set; }
	protected Transform3D CameraBattleStart { get; set; }
	protected Node3D CameraRig { get; set; }

	public override void _Ready()
	{
		StarHolder = GetNode<Node3D>("StarHolder");
		ObjectHolder = GetNode<Node3D>("ObjectHolder");
		CameraBattleStart = GetNode<Node3D>("AdminHolder/CameraBattleStart").Transform;

		LoadedObjects = new List<Node3D>();
		GD.Randomize();

		ButtonSpawn.Pressed += SpawnIn;
		ButtonSpawnTurrets.Pressed += SpawnInTurrets;
		ButtonClear.Pressed += ClearObjects;
		ButtonPause.Pressed += PauseBattle;
		ButtonMainMenu.Pressed += ReturnToMainMenu;
	}

	public override void _Process(double delta)
	{
	}

	public void BattleLoad(Node3D cameraRig)
	{
		CameraRig = cameraRig;

		CreateStarBackdrop();
		CameraRig.Transform = CameraBattleStart;
	}

	public void BattleDone()
	{
		ClearAll();
	}

	public void SpawnIn()
	{
		ButtonSpawn.Disabled = true;
		ButtonSpawnTurrets.Disabled = false;
		ButtonClear.Disabled = false;

		var scene = GD.Load<PackedScene>("res://objects/"+TestObject+".tscn");

		LoadedObjects.Add(scene.Instantiate<Node3D>());
		LoadedObjects.Add(scene.Instantiate<Node3D>());

		LoadedObjects[0].Name = "Ship0";
		LoadedObjects[0].Position = SpawnPos0.Position;

		LoadedObjects[1].Name = "Ship1";
		LoadedObjects[1].Position = SpawnPos1.Position;

		ObjectHolder.AddChild(LoadedObjects[0]);
		ObjectHolder.AddChild(LoadedObjects[1]);
	}

	public void SpawnInTurrets()
	{
		var turretRocketFull = GD.Load<PackedScene>("res://objects/weapons/turret_rocket_full.tscn");
		var turretRocketSide = GD.Load<PackedScene>("res://objects/weapons/turret_rocket_side.tscn");
	}

	public void CreateStarBackdrop()
	{
		var star_scene = GD.Load<PackedScene>("res://objects/"+StarObject+".tscn");
		Node3D temp_star;
		for (int i = 0; i < 99; i++)
		{
			temp_star = star_scene.Instantiate<Node3D>();
			temp_star.Name = "Star_"+i.ToString();
			temp_star.Position = OnUnitSphere() * 50;
			StarHolder.AddChild(temp_star);
		}
	}

	public void ClearObjects()
	{
		var children = ObjectHolder.GetChildren();
		foreach (Node child in children)
		{
			child.Free();
		}
		LoadedObjects.Clear();

		ButtonSpawn.Disabled = false;
		ButtonSpawnTurrets.Disabled = true;
		ButtonClear.Disabled = true;
	}

	public void ClearStars()
	{
		var children = StarHolder.GetChildren();
		foreach (Node child in children)
		{
			child.Free();
		}
	}

	public void ClearAll()
	{
		ClearObjects();
		ClearStars();
	}

	public void PauseBattle()
	{
		// TBD
	}

	public void ReturnToMainMenu()
	{
		//ClearObjects();
		TG.BattleMainMenu_Trigger();
	}

	static public Vector3 OnUnitSphere()
	{
		var ret = new Vector3((float)GD.Randfn(0,1),(float)GD.Randfn(0,1),(float)GD.Randfn(0,1));
		return ret.Normalized();
	}
}
