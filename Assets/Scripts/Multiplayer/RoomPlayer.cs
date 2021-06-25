using Mirror;
using UnityEngine;

public class RoomPlayer : NetworkRoomPlayer
{
	[Header("Player settings")]
	[SyncVar] public string playerName;
}
