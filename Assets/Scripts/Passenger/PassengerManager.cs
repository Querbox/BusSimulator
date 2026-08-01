using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Verwaltet Passagiere und deren Verhalten
/// </summary>
public class PassengerManager : MonoBehaviour
{
    [SerializeField] private int maxPassengers = 50;
    [SerializeField] private float passengerSpawnRate = 2f;
    
    private List<Passenger> passengers = new List<Passenger>();
    private float spawnTimer = 0f;
    
    private void Update()
    {
        spawnTimer += Time.deltaTime;
        
        if (spawnTimer >= passengerSpawnRate && passengers.Count < maxPassengers)
        {
            SpawnPassenger();
            spawnTimer = 0f;
        }
    }
    
    private void SpawnPassenger()
    {
        // TODO: Passagier an zufälliger Haltestelle spawnen
        Debug.Log("Passagier gespawnt!");
    }
    
    public int GetPassengerCount()
    {
        return passengers.Count;
    }
}

/// <summary>
/// Einzelner Passagier
/// </summary>
public class Passenger
{
    public int Id { get; set; }
    public Vector3 TargetStop { get; set; }
    public Vector3 CurrentPosition { get; set; }
    public bool IsInBus { get; set; }
}
