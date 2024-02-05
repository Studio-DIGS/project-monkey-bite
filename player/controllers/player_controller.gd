class_name PlayerController
extends Node

# Must declare `class_name Player` in script attached to player node
var player: Player
@export var is_active = false

func _ready():
	# `as` keyword will return null if owner is not `Player` type
	# This prevents us from accidentally having a player state in a scene other than `Player.tscn`
	player = owner as Player
	assert(player != null)
