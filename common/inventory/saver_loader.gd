extends Node

const SAVE_GAME_PATH = "user://gamesave.tres"

func save_game():
	var gameSave = GameSave.new()
	gameSave.inv = InventoryManager.inv
	ResourceSaver.save(gameSave, SAVE_GAME_PATH)

func load_savedgame():	
	var gameSave : GameSave = load(SAVE_GAME_PATH) as GameSave
	print(gameSave.inv.sword.name)
	print(gameSave.inv.bigPassives[0].name)
	print(gameSave.inv.angelAbility.name)
	InventoryManager.inv = gameSave.inv
	
