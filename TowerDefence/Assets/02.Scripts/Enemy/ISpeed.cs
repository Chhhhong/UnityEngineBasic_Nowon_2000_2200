using System;

public interface ISpeed
{
    float speed { get; set; }
    float speedOrigin { get; }
    event Action<float> OnSpeedChanged;
}