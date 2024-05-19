namespace Event
{
    public enum EventPriority : int
    {
        INNER,
        OUTER,
    }
    public enum Events : int
    {
        START,
        UPDATE,
        TEST,
        
        ON_PLACE_GRID_CHANGE,
    }
}