shader_type spatial;
render_mode unshaded;

uniform sampler2D SCREEN_TEXTURE : hint_screen_texture, filter_linear_mipmap;

void vertex() {
  POSITION = vec4(VERTEX, 1.0);
}

void fragment() {
	vec3 color = texture(SCREEN_TEXTURE, SCREEN_UV).xyz;
	ALBEDO = 1.0f - color;
	ALBEDO = vec3(1.0f,0.0f, 0.0f);
}
