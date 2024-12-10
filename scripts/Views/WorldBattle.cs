using Godot;
using System;
using System.Collections.Generic;

public partial class WorldBattle : Node3D
{
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
	public Button ButtonClear { get; set; }
	[Export]
	public Button ButtonPause { get; set; }
	[Export]
	public Button ButtonMainMenu { get; set; }

	private List<Node3D> LoadedObjects { get; set; }

	public override void _Ready()
	{
		LoadedObjects = new List<Node3D>();
		GD.Randomize();

		var star_scene = GD.Load<PackedScene>("res://objects/"+StarObject+".tscn");
		Node3D temp_star;
		var star_holder = GetNode("StarHolder");
		for (int i = 0; i < 99; i++)
		{
			temp_star = star_scene.Instantiate<Node3D>();
			temp_star.Name = "Star_"+i.ToString();
			temp_star.Position = OnUnitSphere() * 25;
			star_holder.AddChild(temp_star);
		}

		ButtonSpawn.Pressed += SpawnIn;
		ButtonClear.Pressed += CleanUp;
		ButtonPause.Pressed += PauseBattle;
		ButtonMainMenu.Pressed += ReturnToMainMenu;

		ButtonClear.Disabled = true;
	}

	public override void _Process(double delta)
	{
	}

	public void SpawnIn()
	{
		ButtonSpawn.Disabled = true;
		ButtonClear.Disabled = false;

		var scene = GD.Load<PackedScene>("res://objects/"+TestObject+".tscn");

		LoadedObjects.Add(scene.Instantiate<Node3D>());
		LoadedObjects.Add(scene.Instantiate<Node3D>());

		LoadedObjects[0].Name = "Ship0";
		LoadedObjects[0].Position = SpawnPos0.Position;

		LoadedObjects[1].Name = "Ship1";
		LoadedObjects[1].Position = SpawnPos1.Position;

		AddChild(LoadedObjects[0]);
		AddChild(LoadedObjects[1]);
	}

	public void CleanUp()
	{
		foreach (var Item in LoadedObjects)
		{
			RemoveChild(Item);
		}
		LoadedObjects.Clear();

		ButtonSpawn.Disabled = false;
		ButtonClear.Disabled = true;
	}

	public void PauseBattle()
	{
		// TBD
	}

	public void ReturnToMainMenu()
	{
		var main_menu = GD.Load<PackedScene>("res://scenes/main_menu.tscn");
		GetTree().ChangeSceneToPacked(main_menu);
	}

	static public Vector3 OnUnitSphere()
	{
		var ret = new Vector3((float)GD.Randfn(0,1),(float)GD.Randfn(0,1),(float)GD.Randfn(0,1));
		return ret.Normalized();
	}
}
