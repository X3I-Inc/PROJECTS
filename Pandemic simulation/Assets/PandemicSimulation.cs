using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PandemicSimulation : MonoBehaviour
{
    private int startsim = 0;
    public GameObject personPrefab;
    public GameObject plane; // Prefab for person object
    public int initialPopulation = 20;

    public float infectionChance = 0.3f;
    public float baseChance = 0.05f; 
    public float contactIncreaseFactor = 0.2f;
    public float vulnerability = 0.3f; 
    public float interventionEffectiveness = 0.3f; 
    public float decayConstant = 0.5f; 
    public float infectionDistanceThreshold = 2.0f; 

    public TMP_InputField inputFieldinitialPopulation; // Initial population
    public TMP_InputField inputFieldbaseChance;
    public TMP_InputField inputFieldcontactIncreaseFactor;
    public TMP_InputField inputFieldvulnerability;
    public TMP_InputField inputFieldinterventionEffectiveness;
    public TMP_InputField inputFieldinfectionChance;
    public TMP_InputField inputFielddecayConstant;
    public TMP_InputField inputFieldinfectionDistanceThreshold;

    private List<MovingSphere> people = new List<MovingSphere>();

    void Start()
    {
        
    }

    void Update()
    {
        if(startsim != 0){
            if(startsim == 1){
            SpawnPeople();
            startsim = 2;
            }
            SpreadInfection();
        }
        
    }

    void SpawnPeople()
    {
        for (int i = 0; i < initialPopulation; i++)
        {
            // Spawn each person randomly on the roads (grid edges)
            Vector3 position = new Vector3(Random.Range(plane.transform.position.x-4f, plane.transform.position.x+4f), 0, Random.Range(plane.transform.position.z-4f, plane.transform.position.z+4f));
            GameObject newPerson = Instantiate(personPrefab, position, Quaternion.identity);
            MovingSphere person = newPerson.GetComponent<MovingSphere>();

            // Randomly set the person as safe or infected
            person.isInfected = Random.Range(0, 20) == 0; // 50% chance to start infected
            person.SetColor();
            people.Add(person);
        }
    }
     public float CalculateInfectionChance(MovingSphere other)
    {
        // Calculate the distance between the two spheres
        float distance = Vector3.Distance(transform.position, other.transform.position);

        // Only calculate if within infection distance
        if (distance > infectionDistanceThreshold)
            return 0f;

        // Calculate the Contact Factor based on the number of contacts
        float contactFactor = 1 + (contactIncreaseFactor * 1); // Assuming 1 contact

        // Calculate the Distance Factor
        float distanceFactor = Mathf.Exp(-distance / decayConstant);

        // Calculate total Infection Chance
        float infectionChance = baseChance * contactFactor * (1 + vulnerability) * distanceFactor * (1 - interventionEffectiveness);
        return infectionChance;
    }

    public void startbtn(){
        initialPopulation = int.Parse(inputFieldinitialPopulation.text);
        infectionChance = float.Parse(inputFieldinfectionChance.text);
        baseChance = float.Parse(inputFieldbaseChance.text);
        contactIncreaseFactor = float.Parse(inputFieldcontactIncreaseFactor.text);
        vulnerability = float.Parse(inputFieldvulnerability.text);
        interventionEffectiveness = float.Parse(inputFieldinterventionEffectiveness.text);
        decayConstant = float.Parse(inputFielddecayConstant.text);
        infectionDistanceThreshold = float.Parse(inputFieldinfectionDistanceThreshold.text);
        if(startsim == 0)
        {
            startsim = 1;
        }
    }

    void SpreadInfection()
    {
        foreach (MovingSphere person in people)
        {
            if (person.isInfected)
            {
                foreach (MovingSphere other in people)
                {
                   
                         float infectionChance = CalculateInfectionChance(other);
                        if (Random.value < infectionChance)
                        {
                            other.isInfected = true;
                            other.SetColor();
                        }
                    
                }
            }
        }
    }
}
