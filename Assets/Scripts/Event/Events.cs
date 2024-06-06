namespace Event
{
    public enum EventPriority : int
    {
        INNER,
        OUTER,
        
        END
    }
    public enum Events : int
    {
        START,
        UPDATE,
        FIXED_UPDATE,
        TEST,
        
        ON_PLACE_GRID_CHANGE,
        ON_PLACE,
        
        TIME_SCALE_CHANGE,
    }
}