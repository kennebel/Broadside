using Godot;
using System;
using System.Collections.Generic;

public partial class world_battle : Node3D
{
	[Export]
	public string TestObject { get; set; }
	[Export]
	public Node3D SpawnPos0 { get; set; }
	[Export]
	public Node3D SpawnPos1 { get; set; }
	[Export]
	public string StarObject { get; set; }

	private List<Node3D> LoadedObjects { get; set; }

	// Called when the node enters the scene tree for the first time.
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

		// Temp
		SpawnIn();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void SpawnIn()
	{
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
	}

	static public Vector3 OnUnitSphere()
	{
		var ret = new Vector3((float)GD.Randfn(0,1),(float)GD.Randfn(0,1),(float)GD.Randfn(0,1));
		return ret.Normalized();
	}
}
