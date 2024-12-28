using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhraseGenerator : MonoBehaviour
{
  private List<string> mots = new List<string>
{
    "Galaxies contain billions",
    "Jupiter's largest moon Ganymede",
    "Rockets escape Earth's gravity",
    "Solar flares emit radiation",
    "Asteroids orbit the Sun",
    "Nebulas form new stars",
    "Astronauts explore outer space",
    "Lunar surface has craters",
    "Meteor showers are annual",
    "Voyager probes travel far",
    "Solar winds affect satellites",
    "Galactic collisions create galaxies",
    "Comets have icy cores",
    "Planetary orbits are elliptical",
    "Constellations are star patterns",
    "Space stations orbit Earth",
    "Black holes warp space-time",
    "Supernovas create heavy elements",
    "Celestial alignments are rare",
    "Satellites provide global communication",
    "Interstellar space is vast",
    "Alien worlds may exist",
    "Galactic cores are dense",
    "Space debris orbits Earth",
    "Observatories study the universe",
    "Magnetic fields protect planets",
    "Cosmic rays penetrate atmospheres",
    "Space shuttles transport astronauts",
    "Lunar eclipses are spectacular",
    "Interplanetary travel is challenging",
    "Gravity wells bend light",
    "Milky Way is spiral",
    "Extraterrestrial life is unknown",
    "Dark matter is invisible",
    "Solar system has planets",
    "Radio telescopes detect signals",
    "Galactic mergers are violent",
    "Planetary rings are stunning",
    "Deep space is cold",
    "Star clusters are bright",
    "Space elevators are theoretical",
    "Quantum entanglement is puzzling",
    "Lunar bases support life",
    "Expanding nebulae are colorful",
    "Asteroid belts are dangerous",
    "Space tourism is emerging",
    "Cosmic strings are hypothetical",
    "Exoplanets orbit other stars",
    "Satellite arrays monitor Earth",
    "Mars missions are planned"
};


    System.Random random = new System.Random();
    string currentPhrase; // Use a local variable instead of static field

    public Text displayText;

    void Start()
    {
        DisplayRandomPhrase();
    }

    void DisplayRandomPhrase()
    {
        // Generate a random index to select a phrase
        int index = random.Next(mots.Count);
        currentPhrase = mots[index]; // Assign to local variable

        // Set the text of the displayText component to the random phrase
        displayText.text = currentPhrase;
    }

    public string GetCurrentPhrase()
    {
        return currentPhrase; // Return the local variable
    }
}
