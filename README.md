# 2D Procedural Platformer Multiplayer (Unity 6)

## 📌 Overview

This project is a **2D procedural platformer game with real-time multiplayer support** built using **Unity 6**, **Netcode for GameObjects (NGO)**, **Unity Relay**, and **Unity Lobby**.

The player must navigate through a randomly generated platformer level, avoid obstacles and gaps, and reach the finish point. The game supports **PvP real-time multiplayer** where two real players compete in the same procedurally generated level.

---

## 🎮 Features

### Core Gameplay

* 2D platformer with **movement and jump controls**
* Obstacles and gaps to avoid
* Finish point to complete the level

### 🎮 Conrols
* **Tap and swipe input controls** (mobile-friendly)
* Swipe left side of screen (left to Right or Right to left) for Forward and Backward Movements.
* Swipe Up on Right side of screen to Jump.
* Swipe Down on Right side of screen to Slide.

---

## 🌍 Procedural Level Generation

* Levels are generated **randomly at runtime**
* Platforms, gaps, and obstacles are spawned dynamically
* Configurable difficulty parameters :

  * Obstacle frequency
  * Level length and density

* Configurations are in Scriptable Object - Path Assets/ScriptableObjects/PlatformData

This ensures **unique levels every time the game starts**.

---

## 🌐 Multiplayer (Real-Time PvP)

Multiplayer is implemented using **Unity Gaming Services**.

### Multiplayer Flow

**Host:**

* Creates a public lobby
* Waits for players to join
* Starts the match

**Client:**

* Fetches available public lobbies
* Selects a lobby from the list
* Joins and connects via Relay

---

## 🧠 Networking Architecture

### Lobby (Matchmaking Layer)

* Used to discover available game sessions
* Stores Relay Join Code
* Manages player discovery and lobby lifecycle

### Relay (Connection Layer)

* Provides NAT traversal and secure P2P connectivity
* Generates Join Code for clients
* Enables real internet multiplayer without port forwarding

### Netcode for GameObjects (Gameplay Sync)

* Player movement synchronization
* Transform replication
* Game state synchronization

---

## 🖥 UI Flow

* **Start Screen**

  * Create Lobby (Host)
  * Join Lobby (Client)
  * List of available public lobbies
  * Player count display
* **Gameplay Screen**
* **Game Over Screen** (Timer expired or finish reached)

---

## ⚙ Difficulty Control

Difficulty is controlled through configurable parameters in the Level Generator:

* Platform distance
* Obstacle spawn rate

These values can be tuned via the Unity Inspector or ScriptableObjects.

---

## 📱 Build Information

* **Platform:** Android
* **Unity Version:** Unity 6 (6000.2.9f1)
* **Multiplayer Backend:** Unity Gaming Services (Lobby + Relay)

---

## ⚙ Project SetUp Instruction
* Clone this repository.
* Open in unity editor version - 6000.2.9f1.
* Goto Assets/Scenes/GamePlay.
* Build for Android and Run.

---

## 🧪 Known Limitations

* Host-based multiplayer (if host leaves, session ends)
* No host migration 
* Placeholder visuals used for platforms, obstacles, and characters

---

## 🧑‍💻 Developer Notes

This project focuses on **gameplay logic, procedural generation, and multiplayer architecture** rather than visual polish. The architecture is modular and scalable for future improvements.

---

## 🚀 Future Improvements

* Host migration
* Dedicated server support
* Advanced obstacle patterns
* Power-ups and abilities
* Leaderboards and scoring system

---

**Developed by:** Nawin Sekar
