@tool     # makes script run from within the editor 
extends EditorScript    # gives you access to editor functions

func _run():   # this is the main function
	var selection = get_editor_interface().get_selection()
	selection = selection.get_selected_nodes()  # get the actual AnimationPlayer node
	if selection.size()!=1 and not selection is AnimationPlayer: # if the wrong node is selected, do nothing
		return
	else:
		var animlist = selection[0].get_animation_list() # finds every animation on the selected AnimationPlayer
		print("animations found: "+str(animlist))
		for anm in animlist: # loops between every animation and apply the fix
			interpolation_change(selection,anm)

func interpolation_change(selection,strnum):
	var anim_track_1 = selection[0].get_animation(strnum) # get the Animation that you are interested in (change "default" to your Animation's name)
	var count  = anim_track_1.get_track_count() # get number of tracks (bones in your case)
	print("fixed "+strnum)
	for i in count:
		anim_track_1.track_set_interpolation_type(i, 0) # change interpolation mode for every track
