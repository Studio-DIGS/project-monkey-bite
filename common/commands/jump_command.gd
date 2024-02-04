class_name JumpCommand
extends Command

func execute(actor: Actor, data: Object = null) -> void:
	actor.jump()
