# Circuit Snake
Synopsis: Circuit Snake is a cyberpunk-inspired snake game where the player controls a Trojan virus navigating inside a corrupted computer system. Collect data packets, avoid evolving security hazards, and ultimately face off against an AI Antivirus Snake in a high-stakes final race.

## Controls
### Desktop
- Keyboard input
  - W / A / S / D — Move  
  - Movement begins after the first input

### Mobile
- Touch controls
  - Swipe Up / Down / Left / Right — Change direction

## Goal
- Collect data packets to grow
- Avoid firewalls and antivirus drones
- Survive each system layer
- Reach the Boss layer and defeat the AI Antivirus Snake in a packet-collecting race

## Snake Movement & Growth
- Grid-based movement
- Snake grows with each data packet collected
- Supports both keyboard and touch input

## Packet System
- Packets spawn dynamically within safe bounds
- Boss Level: both player and AI snake collect packets
- Packet requirements to advance layers vary by level

## Layer System & Progression
- Progress through layers by collecting fewer packets as the difficulty increases
- Additional obstacles are introduced with each layer

### Layer Breakdown
- **Layer 1:** 5 packets, antivirus drones (vertical movement)
- **Layer 2:** 4 packets, moving firewalls (horizontal movement)
- **Layer 3:** 3 packets, hazard repositioning + circuit rearrangement
- **Layer 4:** 2 packets, hazard repositioning + circuit rearrangement
- **Layer 5 (Boss):** 10 packets, AI Snake race

## Layer Transitions
Each layer transition includes:
- Screen glitch effect
- Warning + siren visuals + system scan  
  - Used for Boss level transition and cutscenes
- Camera zoom-in (Layers 3 & 4)
- Circuit rearrangement (Layers 3 & 4)

## Hazards
- Firewalls (border)
- Moving firewalls (horizontal movement)
- Antivirus drones (vertical movement)
- AI Snake (Layer 5)

## Hazard Collisions
Touching any hazard triggers:
- Glitch effect
- Death → automatic restart
- Segment loss (Boss Level only, when colliding with AI Snake)

## Layer 5: Boss Level
- Race the AI Snake to collect 10 packets
- AI actively hunts the closest packet

### Collisions with AI Snake
- Player loses one segment upon head or segment collision
- If the player is only a head → instant defeat, AI wins

### Win Conditions
- First to 10 packets wins the game
- Player win → victory cutscene with UI elements
- AI win → AI victory cutscene with UI elements
- All gameplay elements freeze once the race ends
