﻿
namespace UDPLibrary
{
    public enum EventCategoryType : sbyte
    {
        //one category for each group of events in EventType
        MainMenu,
        UIMenu,
        Video,
        Sound,
        TextRender,
        Zone,
        Camera,
        Player,
        NonPlayer,
        Pickup,
        Puzzle,

        //all other categories of sender...
    }
}
