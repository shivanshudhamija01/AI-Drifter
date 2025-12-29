using UnityEngine;

// public enum CarState
// {
//     Locked,
//     Unlocked
// }
[CreateAssetMenu(fileName = "Car", menuName = "SO/CarData")]
public class CarSO : ScriptableObject
{
    [Header("Identity")]
    public int carID;

    [Header("Stats")]
    public string CarName;
    public int CarPrice;
    public float speed;
    public float acceleration;
    public float handling;

    [Header("Model")]
    public GameObject CarModel;
}
