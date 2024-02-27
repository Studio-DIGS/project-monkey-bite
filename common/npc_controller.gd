class_name NPCController
extends Node

# Must declare `class_name Actor` in script attached to player node
var actor: Actor

func _ready():
	# `as` keyword will return null if owner is not Actor` type
	# This prevents us from accidentally having a player state in a scene other than `Player.tscn`
	actor = owner as Actor
	assert(actor != null)

