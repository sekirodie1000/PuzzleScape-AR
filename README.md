# PuzzleScape AR

The **PuzzleScape AR** is an augmented reality-based educational game developed using **Unity**, **Vuforia**, and **Blender**. This interactive game allows players to solve circuit-related puzzles by arranging AR cards to form correct circuit layouts or diagnose and fix circuit issues. The game provides an engaging way to learn about electrical circuits and troubleshoot problems, combining education with fun.

## Features

1. **Interactive AR Gameplay**  
   Players use AR marker cards to interact with the game. By physically arranging the cards, they can form circuits or identify and fix faults.

2. **Three Levels of Challenges**
   - **Tutorial**: Learn the basics of arranging series and parallel circuits to complete a working layout.
   - **Level 1**: Use an ammeter (current meter) to locate and fix faults in the circuit.
   - **Level 2**: Use a voltmeter to identify and replace faulty components in the circuit.

3. **Educational Focus**  
   - Understand the difference between series and parallel circuits.
   - Learn to use measuring devices like an ammeter and voltmeter for circuit diagnosis.

4. **Realistic Circuit Representation**  
   - Each card represents a component (e.g., lamps, switches, battery).
   - AR overlays simulate connections and measurements, providing a clear visualization of circuit behaviors.

## Game Levels

### **Tutorial: Series and Parallel Circuits**
- Objective: Arrange the AR marker cards to create functioning **series** and **parallel** circuits.  
- Learn about the differences between the two types of connections and how they affect the circuit's performance.

### **Level 1: Diagnosing Faults with an Ammeter**
- Objective: Diagnose and fix the circuit using an **ammeter**.  
- Tasks:  
  - Arrange the AR cards to match the initial circuit diagram.  
  - Use the ammeter to detect the presence of current and identify any faults.  
  - Adjust the circuit to resolve the issue.  
- Success: Fix the fault and create a functional circuit.

### **Level 2: Diagnosing Faults with a Voltmeter**
- Objective: Replace a faulty lamp by using a **voltmeter** for diagnosis.  
- Tasks:  
  - Replace the faulty lamp card with a new one (e.g., `lampImageTarget2`).  
  - Ensure all other components remain in their correct positions as per the initial diagram.  
  - Use the voltmeter to verify the new lamp placement by checking its position relative to the circuit.  
- Success: Correctly replace the faulty lamp while maintaining the original circuit layout.

## Components and Tools

### Hardware Requirements
- Physical AR markers (image targets).
- A device capable of running AR-supported applications (Android).

### Software Stack
- **Unity**: Game engine for development and rendering.
- **Vuforia**: AR platform for marker-based tracking.
- **Blender**: Used to model and animate the 3D components.
- **C#**: Programming language for game logic and interaction.

### AR Markers
1. **Battery**: Represents a power source.  
2. **Switch**: Toggles circuit connections.  
3. **Lamp**: Represents a load, with both normal and glowing states.  
4. **Ammeter**: Measures current flow in Level 1.  
5. **Voltmeter**: Measures voltage across a faulty lamp in Level 2.

## Getting Started

### Installation
1. Download and install **Unity Hub**.
2. Clone this repository or download the project files.
3. Open the project in Unity.
4. Set up Vuforia.
5. Import the provided image targets into Vuforia's target manager and download the target database.
6. Deploy the project to an AR-capable device(Android).
   
### Running the Game
1. Print the provided image targets.
2. Launch the application on your AR-capable device(Android).
3. Follow the on-screen instructions for each level.

## Example Video

[will be added]

## Future Enhancements

- Add more levels focusing on advanced electrical concepts (e.g., capacitors and resistors).  
- Include interactive tutorials on circuit theory basics.  
- Multiplayer mode for collaborative learning experiences.  



Enjoy your journey into the world of AR-based circuit puzzles!
