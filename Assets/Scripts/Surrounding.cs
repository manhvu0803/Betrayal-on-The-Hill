using UnityEngine;

public class Surrounding
{
	public enum State
	{
		Door, Wall, Empty
	}

	public State North { get; private set; } = State.Wall;
	public State East  { get; private set; } = State.Wall;
	public State South { get; private set; } = State.Wall;
	public State West  { get; private set; } = State.Wall;

	public int DoorCount { get; private set; }

	public Surrounding(Vector2Int position, Board board)
	{
		SetSurrounding(position.x, position.y, board);
		CheckSurroundingDoors();
	}

	private void SetSurrounding(int x, int y, Board board)
	{
		if (y >= board.Height - 1 || board.TileAt(x, y + 1) == null)
			North = State.Empty;
		else if (board.TileAt(x, y + 1).GetDoors().south)
			North = State.Door;

		if (x >= board.Width - 1 || board.TileAt(x + 1, y) == null)
			East = State.Empty;
		else if (board.TileAt(x + 1, y).GetDoors().west)
			East = State.Door;

		if (y <= 0 || board.TileAt(x, y - 1) == null)
			South = State.Empty;
		else if (board.TileAt(x, y - 1).GetDoors().north)
			South = State.Door;

		if (x <= 0 || board.TileAt(x - 1, y) == null)
			West = State.Empty;
		else if (board.TileAt(x - 1, y).GetDoors().east)
			West = State.Door;
	}

	private void CheckSurroundingDoors()
	{
		UpdateDoorCount(North);
		UpdateDoorCount(East);
		UpdateDoorCount(South);
		UpdateDoorCount(West);
	}

	private void UpdateDoorCount(State state)
	{
		if (state == State.Door)
			DoorCount += 1;
	}

	public float NextValidRotation(Tile tile, int direction = 1)
	{
		int maxDoor = -1;
		int angle = 0;
		Doors doors = tile.GetDoors();

		if (direction >= 0) 
			direction = 1;
		else 
			direction = -1;

		for (int i = 0; i < 4; ++i) 
		{
			int cnt = 0;
			if ((doors.north && (North == State.Door)) || (!doors.north && (North == State.Wall))) ++cnt;
			if ((doors.east  && (East  == State.Door)) || (!doors.east  && (East  == State.Wall))) ++cnt;
			if ((doors.south && (South == State.Door)) || (!doors.south && (South == State.Wall))) ++cnt;
			if ((doors.west  && (West  == State.Door)) || (!doors.west  && (West  == State.Wall))) ++cnt;

			// Check more than or equal door count so that the next valid-but-not-better rotation is not skipped
			if (maxDoor <= cnt) {
				maxDoor = cnt;
				// This rotate direction is the opposite of transform.eulerAngles.y, which hold the real rotation
				angle = -i * direction * 90;
			}

			doors = doors.AfterRotate(direction * 90);
		}

		return angle;
	}

	public override string ToString()
	{
		return $"n:{North} e:{East} s:{South} w:{West}";
	}
}
