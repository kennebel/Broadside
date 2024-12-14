using Godot;
using System;

public partial class TransitionOverlay : CanvasLayer
{
	public delegate void HalfAnimationCallback();
	public delegate void EndAnimationCallback();

	protected ColorRect Overlay { get; set; }
	protected Color StartColor { get; set; }
	protected Color EndColor { get; set; }

	protected bool IsRunning { get; set; }
	protected float RunDuration { get; set; }
	protected float ElapsedDuration { get; set; }
	protected float ElapsedDurationPercent { get { return Math.Clamp(ElapsedDuration / RunDuration, 0f, 1f); } }

	protected HalfAnimationCallback HalfCallback { get; set; }
	protected EndAnimationCallback EndCallback { get; set; }
	protected bool ReachedHalf { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Overlay = GetNode<ColorRect>("Overlay");

		StartColor = new Color(Overlay.Color, 0);
		EndColor = new Color(StartColor, 1);

		Reset();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!IsRunning) { return; }

		ElapsedDuration += (float)delta;
		//GD.Print(String.Format("Animation Elapsed: {0}", ElapsedDuration));

		if (ElapsedDurationPercent == 1f)
		{
			EndAnimation();
		}
		else if (ElapsedDurationPercent >= 0.5f && !ReachedHalf)
		{
			if (HalfCallback != null) { HalfCallback(); }
			ReachedHalf = true;
		}

		if (!ReachedHalf)
		{
			Overlay.Color = StartColor.Lerp(EndColor, ElapsedDurationPercent*2f);
			//GD.Print(String.Format("First %: {0}, {1}", ElapsedDurationPercent, ElapsedDurationPercent*2f));
		}
		else
		{
			Overlay.Color = EndColor.Lerp(StartColor, (ElapsedDurationPercent-0.5f)*2f);
			//GD.Print(String.Format("Secnd %: {0}, {1}", ElapsedDurationPercent, (ElapsedDurationPercent-0.5f)*2f));
		}
	}

	public void StartAnimation(EndAnimationCallback notifyEnd, HalfAnimationCallback notifyHalf=null, float duration=2f)
	{
		EndCallback = notifyEnd;
		HalfCallback = notifyHalf;
		RunDuration = Math.Clamp(duration, 0.1f, 30f);

		IsRunning = true;

		//GD.Print(String.Format("Colors: {0}, {1}", StartColor.ToString(), EndColor.ToString()));
	}

	protected void Reset()
	{
		IsRunning = false;
		Overlay.Color = StartColor;

		RunDuration = 0f;
		ElapsedDuration = 0f;

		HalfCallback = null;
		ReachedHalf = false;
		EndCallback = null;
	}

	public void EndAnimation()
	{
		if (EndCallback != null)
		{
			EndCallback();
		}
		Reset();
	}
}
