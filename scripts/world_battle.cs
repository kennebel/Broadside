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

	private List<Node3D> LoadedObjects { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		LoadedObjects = new List<Node3D>();

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
}
