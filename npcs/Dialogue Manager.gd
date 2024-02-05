extends Node3D

#Class Name Dialogue Manager
#class_name dialogue

var playerIsDialogueReady: bool = false
var dialogueIsOpen: bool = false
var dialogueArray
var textIndex

var regularStored
var questStored
var endStored

var state: String = "regularDialogue"
var dialogueItemReference
var dialogueTextReference

#Button after every run is "R" which pretends to reset game
#Press "T" to complete some checkmarker for quest to be active

# Called when the node enters the scene tree for the first time.
func _ready():
	dialogueItemReference = $"../CanvasLayer/Dialogue Items"
	dialogueTextReference = $"../CanvasLayer/Dialogue Items/NPC Text"
	dialogueItemReference.visible = false
	textIndex = 0


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta):
	if (Input.is_action_just_pressed("interact")): #Press E, player is talking to NPC
		interactNPC()
	if (Input.is_action_just_pressed("continue")): #Press enter to flip to the next text within the DialogueArray
		continueDialogue()
		
func interactNPC(): #Function Logic for talking interacting with NPCs
	if dialogueIsOpen:
		print("Close Dialogue")
		dialogueItemReference.visible = false
		dialogueIsOpen = false
		textIndex = 0 #Not necessary
	elif playerIsDialogueReady and !dialogueIsOpen:
		print("Open Dialogue")
		dialogueItemReference.visible = true
		dialogueIsOpen = true

func continueDialogue(): #Function Logic for continuing dialogue for NPCS
	if (textIndex < dialogueArray.size()):
		dialogueTextReference.text = dialogueArray[textIndex]
		textIndex += 1
	elif (textIndex >= dialogueArray.size()): #Upon reaching the end of the dialogueArray, close the dialogue and reset index to 0
		state = "endDialogue"
		dialogueArray = endStored
		dialogueItemReference.visible = false
		dialogueIsOpen = false
		textIndex = 0
		dialogueTextReference.text = dialogueArray[textIndex]
	
func _on_player_detection_box_area_entered(area): #Checks if player enters NPC conversation range, enabling player to press "e" to talk
	if (area.name == "PlayerArea"):
		playerIsDialogueReady = true


func _on_player_detection_box_area_exited(area):
	if (area.name == "PlayerArea"):
		playerIsDialogueReady = false

#Transfers data from NPC1 to DialogueManger
func _on_script_holder_npc_text(questDialogue, regularDialogue, endDialogue): #Do not change the variable "data" in the parenthesis, you will get a lot of errors
	regularStored = regularDialogue
	questStored = questDialogue
	endStored = endDialogue
	
	if state == "questDialogue":
		dialogueArray = questDialogue
	if state == "regularDialogue":
		dialogueArray = regularDialogue
	if state == "endDialogue":
		dialogueArray = endDialogue
	textIndex = 0
	dialogueTextReference.text = dialogueArray[textIndex]
	print($"..".name)

func _on_temporary_run_simulator_reseting_hub(): #When user presses "R" the hub resets. Signal comes from TemporaryRunSimulator
	state = "regularDialogue"
	
func _on_temporary_run_simulator_objective_one_complete(): #When user presses "Shift + R" the quest is marked as complete. Signal comes from TemporaryRunSimulator
	state = "questDialogue"
