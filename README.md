# 

# Circuit Snake

Name: Daniela Banda
email: bandaher@usc.edu



Synopsis: This is the playable prototype for Trojan Snake Virus, a cyberpunk-inspired snake game where the player controls a Trojan-virus navigating inside a corrupted computer system. The prototype demonstrates the core mechanics described in the design document.



Play:

Keyboard (Desktop)

* W/A/S/D – move (movement begins after first input)



Touch Controls (Mobile)

* Swipe Up/Down/Left/Right to change direction



Goal:

* Collect data packets to grow
* Avoid firewalls and antivirus drones
* Survive as long as you can



Implementations:

* Snake movement system (grid-based)
* Touch + keyboard input
* Collectible packets (random safe-area spawning)
* Snake growth with segment prefabs
* Hazards:
* &nbsp;	Firewalls (all 4 sides)
* &nbsp;	Antivirus drones (horizontal + vertical movement)
* Collision + restart loop
* Score UI and simple layer progression
* Glitch animation feedback on death or layer-up



Layer System/Progression:

The game includes a basic layer counter, which increases every 5 packets collected.

At each layer increase, a short glitch animation plays as a placeholder for future level transitions.



\*Note:

* Only Layer 1 is implemented in this prototype.
* The glitch effect at 5 points is intentional and serves to show the start of the planned progression system.
* Additional layer features (new obstacles, layout changes, boss level, etc.) will be implemented in the final project.





\*Note:

* Shapes will be switched out to more descriptive/artistic depictions to bring the game alive.
* Background will also be changed to reflect 'computer system' more. 





