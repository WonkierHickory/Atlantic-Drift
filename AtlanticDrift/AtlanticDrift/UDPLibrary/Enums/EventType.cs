﻿
namespace UDPLibrary
{
    public enum EventType : sbyte
    {
        /*
           //main menu
        OnMainMenuPlay,
        OnMainMenuExit,
        OnMainMenuPause,
        OnMainMenuRestart,
        OnMainMenuVolumeUp,     //affects all sounds
        OnMainMenuVolumeDown,   //affects all sounds
        OnMainMenuMute,         //affects all sounds

        //ui
        OnUIMenuShow,
        OnUIMenuHide,
        OnUIMenuHoverClickable,
        OnUIMenuClick,

        //video
        OnVideoPlay,
        OnVideoStop,
        OnVideoPause,
        OnVideoRestart,
        OnVideoVolumeUp,
        OnVideoVolumeDown,
        OnVideoMute,

        //sound
        OnSoundPlay,
        OnSoundStop,
        OnSoundPause,
        OnSoundRestart,
        OnSoundVolumeUp,    //affects a single sound
        OnSoundVolumeDown,  //affects a single sound
        OnSoundMute,        //affects a single sound
       
        //zones
        OnZoneEnter,
        OnZoneExit,

        //camera
        OnCameraChanged,

        //player
        OnPlayerLoseHealth,
        OnPlayerGainHealth,
        OnPlayerLose,
        OnPlayerWin,

        //nonplayer
        OnNonPlayerLoseHealth,
        OnNonPlayerGainHealth,
        OnNonPlayerLose,
        OnNonPlayerWin,

        //pickups
        OnPickup,
         */

        //sent by menu, audio, video
        OnPlay,
        OnExit,
        OnStop,
        OnPause,
        OnRestart,
        OnVolumeUp,
        OnVolumeDown,
        OnMute,
        OnClick,
        OnHover,

        //zones
        OnZoneEnter,
        OnZoneExit,

        //camera
        OnCameraChanged,

        //player or  nonplayer
        OnLoseHealth,
        OnGainHealth,
        OnLose,
        OnWin,

        //pickups
        OnPickup,

        //text renderer event
        OnTextRender,

        //all other events...
        OnPuzzle
    }
}
