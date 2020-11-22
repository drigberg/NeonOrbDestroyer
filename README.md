# The House of Glow

## About

You're trapped in the House of Glow! Fight your way out!

## TODO: V1

### Menus

1. mute toggle in main menu and pause screen
2. only one "new game" button

### Camera

3. camera and text are clearly positioned

### Audio

4. game over sound
5. better jumping and damage sounds
6. attacking sound
7. ambient menu music
8. game music

### Player

9. small window for wall-jumping after breaking contact
10. regular attack only casts forwards
11. attack still casts everywhere while spinning, but not as far
12. nicer attack animation: sword trail? swinging sword down?

## TODO: V2

### Audio

- consolidate audio sources

### Menus

- menu buttons broadcast "Action" message, which is heard by child component
- disable animation for the unpause button, or smooth out the timing issue

### Player

- allow multiple wall jumps after contact with the wall is broken
- while jumping, attack is forward plus both forward diagonals
- while gripping the wall, attack is up and down
- wall-gripping attack animation

### Scenes

- bigger rooms, zoomed out
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
