using UnityEngine;

[System.Serializable]
public class Mail
{
    [SerializeField] private string _from;
    [SerializeField] private object _data;
    [SerializeField] private bool _read;
    [SerializeField] private bool _collected;
    [SerializeField] private long _timestamp;

    public string From { get => _from; set => _from = value; }
    public object Data { get => _data; set => _data = value; }
    public bool Read { get => _read; set => _read = value; }
    public bool Collected { get => _collected; set => _collected = value; }
    public long Timestamp { get => _timestamp; set => _timestamp = value; }
}
