namespace AmiableNext.SDK;

public class AppService
{
    private List<IBotEvent> _events = new();


    public void Init()
    {
        this._events.Clear();
    }
    
    public void RegEvent<T>(string key = "") where T : IBotEvent
    {
        _events.Add(Activator.CreateInstance<T>());
#if DEBUG
        Console.WriteLine(typeof(T).Name + "已载入");
#endif
    }

    public void Invoke(int type, AmiableEventContext ctx)
    {
        this._events.FindAll(x => (int)x.EventType == type).ForEach(x => x.Process(ctx));
    }
}