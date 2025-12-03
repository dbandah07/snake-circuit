# 

# Circuit Snake

Name: Daniela Banda
email: bandaher@usc.edu



Synopsis: Circuit Snake is a cyberpunk-inspired snake game where the player controls a Trojan-virus navigating inside a corrupted computer system. Collect data packets, avoid evolving security hazards, and ultimately face off against an AI Antivirus Snake in a high-stakes final race.



Play:

Keyboard (Desktop)

* W/A/S/D – move (movement begins after first input)



Touch Controls (Mobile)

* Swipe Up/Down/Left/Right to change direction



Goal:

* Collect data packets to grow
* Avoid firewalls and antivirus drones
* Survive each layer 
* Reach Boss layer and beat the AI Antivirus Snake in a packet-collecting race



Snake Movement/Growth:

* Grid-based movement
* Snake grows with each data packet collected
* Swipes and keyboard supported



Packet System:

* Packets spawn dynamically within safe bounds
* Boss LVL: both player and AI snake collect packets
* Packet requirements (to move forward to another layer) differ layer by layer 



Layer System/Progression:

* Advancement through layers by collecting fewer packets each time (more obstacles as layers progress)
* Layer 1: 5 packets, Antivirus drones (vertical obstacle)
* Layer 2: 4 packets, Moving firewalls introduced (horizontal movement)
* Layer 3: 3 packets, Hazard repositioning + Circuit repositioning
* Layer 4: 2 packets, Hazard repositioning + Circuit repositioning
* Layer 5 (Boss): 10 packets, AI Snake race



Each layer transition uses: 

* Screen glitch
* Warning + 'Siren' visuals + System scan (transition to boss LVL + cutscenes)
* Camera zoom in (layer 3 + 4)
* Circuit rearrangement (layer 3 + 4)



Hazards:

* Firewalls (boarder)
* Moving Firewalls (horizontal movement)
* Antivirus Drones (vertical movement)
* AI Snake (LVL 5)



Touching any hazard triggers:

* Glitch
* Death -> auto-restart
* Lose segments (only LVL 5, when head collided with AI Snake)



Layer 5 (Boss LVL):

* Race the AI to 10 packets
* AI actively hunts the closest packet
* If the player collides with the AI Snake’s head or a segment:
* &nbsp;	Player loses one segment
* &nbsp;	If player is only a head → instant defeat, AI wins
* First to 10 packets wins the entire game
* If Player Wins -> player win cutscene with UI elements 
* If AI Wins -> AI win cutscene with UI elements
* All other gameplay elements freeze once the race ends.
