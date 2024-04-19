class_name InventoryVis
extends Node

@onready var swordTextLabel = $UICanvasGroup/SwordText
@onready var bigPassiveTextLabel = $UICanvasGroup/BigPassiveText
@onready var angelAbilityTextLabel = $UICanvasGroup/AngelAbilityText
@onready var health_bar = $UICanvasGroup/HealthBar
@onready var weapon_sprite = $UICanvasGroup/WeaponHud/WeaponSprite
@onready var fist_sprite: CompressedTexture2D = preload("res://assets/UI/fist.png")

func _ready():
#	swordTextLabel.clear()
#	bigPassiveTextLabel.clear()
#	angelAbilityTextLabel.clear()
#	updateAllText()
	pass

func update_health(hp_percent: float):
	health_bar.value = hp_percent

func update_weapon(sprite: CompressedTexture2D = fist_sprite):
	weapon_sprite.texture = sprite

func updateAllText():
	updateAngelAbilityVis()
	updateBigPassiveVis()
	updateSwordVis()

# Sword

func updateSwordVis():
	swordTextLabel.clear()
	swordTextLabel.append_text(InventoryManager.getSword().name)

# Big Passives
	
func updateBigPassiveVis():
	if (InventoryManager.getBigPassives().size() <= 0):
		return
	bigPassiveTextLabel.clear()
	for bigPassive in InventoryManager.getBigPassives():
		bigPassiveTextLabel.append_text(bigPassive.name)

# Angel Abilites
	
func updateAngelAbilityVis():
	angelAbilityTextLabel.clear()
	angelAbilityTextLabel.append_text(InventoryManager.getAngelAbility().name)
