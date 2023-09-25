# Boilerplate to get autocomplete and type checks when coding the player states
# Without this, we'd have to run the game every time to check for errors the compiler should see
class_name PlayerState
extends State

# Must declare `class_name Player` in script attached to player node
var player: Player

func _ready():
	# `as` keyword will return null if owner is not `Player` type
	# This prevents us from accidentally having a player state in a scene other than `Player.tscn`
	player = owner as Player
	assert(player != null)
