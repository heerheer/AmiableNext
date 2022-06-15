namespace AmiableNext.SDK;

public interface IBotEvent
{
    CommonEventType EventType { get; set; }
    void Process(AmiableEventContext ctx);
}