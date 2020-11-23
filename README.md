# The House of Glow

## About

You're trapped in the House of Glow! Fight your way out!

## TODO: V1

### Player

1. nicer attack animation: sword trail? swinging sword down?

### Audio

2. game over sound
3. better jumping and damage sounds
4. attacking sound
5. ambient menu music
6. game music

## TODO: V2

### Audio

- consolidate audio sources

### Menus

- menu buttons broadcast "Action" message, which is heard by child component
- disable animation for the unpause button, or smooth out the timing issue

### Player

- no enemy collisions when invincible
- stuck-on-corner bug
- regular attack only casts forwards and up
- attack still casts everywhere while spinning, but not as far
- can be damaged if hit from behind while attacking forwards
- programmatic hearts
- wall-gripping attack animation

### Scenes

- Empty starting room --> hall -> empty room with NPC --> first room
- empty room after first arena, with NPC
- second arena
- final room with an NPC that says that the game isn't done yet
- rooms have background scenery instead of a wall

### Arena elements

- moving platforms
- vertical platforms for wall jumping
- platforms pulse with the music
- hearts beat with the music

### High Score Table

- local high scores
- global high scores: post to S3 with secret key? (separate scores by easy/hard mode)

### Drops

- hearts restore health
- blue power-up doubles the attack effect area
- red power-up grants a special wide-range force field attack on N

### Enemies

- more of them!
- enemies with more health
- enemies with projectiles
- enemies with attack moves

## Credits

Main menu buttons created using sample assets by Thomas Brush.

- Youtube: https://www.youtube.com/watch?v=vqZjZ6yv1lA&ab_channel=ThomasBrush
- Github: https://github.com/atmosgames/unityMenuTutorial
