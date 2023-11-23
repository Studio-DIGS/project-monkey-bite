extends Node
class_name CameraProcesser;

func process_cam(_dir : float, current_position : Vector3, _delta : float, _camera : Camera3D) -> Vector3:
	return current_position;
