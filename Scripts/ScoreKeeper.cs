using System;
using System.Security.Cryptography;
using Godot;

public partial class ScoreKeeper : Node2D
{
	RichTextLabel Score1;
	RichTextLabel Score2;
	public override void _Ready()
	{
		base._Ready();
		CharacterBody3D Ball = GetParent().GetNode<CharacterBody3D>("Ball");
		// Connect to its signals
		Ball.Connect("OutOfBoundsLeft", new Callable(this, nameof(OutOfBoundsLeft)));
		Ball.Connect("OutOfBoundsRight", new Callable(this, nameof(OutOfBoundsRight)));

		Score1 = GetNode<RichTextLabel>("Score1");
		Score2 = GetNode<RichTextLabel>("Score2");
		ResetScores();
	}

	private void OutOfBoundsLeft()
	{
		int score1 = Score1.Text.ToInt();
		score1++;
		Score1.Text = score1.ToString();
	}
	private void OutOfBoundsRight()
	{
		int score2 = Score2.Text.ToInt();
		score2++;
		Score2.Text = score2.ToString();
	}

	private void ResetScores()
	{
		Score1.Text = "0";
		Score2.Text = "0";
	}
}
